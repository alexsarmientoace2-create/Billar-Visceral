using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    private CinemachineCamera vcam;
    private CinemachineFollow followComponent;

    private InputAction scrollAction;
    private InputAction PointAction;

    [Header("Límites de Zoom")]
    [SerializeField] private float zoomSpeed = 0.05f;
    [SerializeField] private float minDistance = 5f;
    [SerializeField] private float maxDistance = 30f;
    [SerializeField] private float smoothTime = 0.15f;

    [Header("Ajuste de Plano")]
    [SerializeField] private LayerMask planeLayerMask = ~0;

    private float targetDistance;
    private Vector3 zoomVelocity;


    public Transform dummyTarget;
    private Vector3 targetFollowPosition;
    private Vector3 positionVelocity;

    private void Awake()
    {
        vcam = GetComponent<CinemachineCamera>();

        followComponent = vcam.GetComponent<CinemachineFollow>();

        scrollAction = InputSystem.actions.FindAction("Scroll");

        PointAction = InputSystem.actions.FindAction("Point");



        if (scrollAction == null)
        {
            Debug.LogError($"No se encontró la acción Scroll en InputSystem.actions.");
            return;
        }
        if (PointAction == null)
        {
            Debug.LogError($"No se encontró la acción Point en InputSystem.actions.");
            return;
        }
        /// <summary>
        /// el dummy (gameobject empty) debe de estar asignado como tracking target en
        /// el componente cinemachine camera. ademas debemos asignar "follow" en position control
        ///En el mismo componente de cinemachine camera.
        if (followComponent == null || vcam.Follow == null)
        {
            Debug.LogError("CinemachineCamera no tiene asignado 'Follow' o no tiene el componente CinemachineFollow.");
            return;
        }

        Dummy();
    }


    /// <summary>
    /// Inicializa el dummy de forma dinámica. 
    /// Hacemos esto por código en lugar de dejarlo fijo en el Inspector 
    /// porque al pulsar Play, si las variables internas de posición y distancia no se acoplan 
    /// exactamente al estado inicial de la cámara, el sistema de físicas/renderizado intentará 
    /// mandar el target al origen (0,0,0) o colapsará el cálculo matemático del zoom (LerpFactor).

    private void Dummy()
    {
        /// <summary>
        /// Evita que el dummy se quede en la coordenada por defecto del editor (0,0,0).
        /// Toma la posición de lo que sea que tuviera asignado la cámara originalmente como guía.
        ///en este caso esa guia es la posicion original en la que pusimos al dummy, osea, la altura a la que esta la layermask
        dummyTarget.position = vcam.Follow.position;

        /// <summary>
        /// C# inicializa los Vector3 en (0,0,0) por defecto. Si no igualamos esto aquí, 
        /// el método Vector3.SmoothDamp del Update interpretará que la cámara debe viajar 
        /// inmediatamente al centro del universo en el frame 1, provocando un zoom intantaneo que atravesara la mesa.
        targetFollowPosition = vcam.Follow.position;

        /// <summary>
        /// Lee el desfase físico real (Offset) que tiene la cámara respecto al objeto en el editor.
        /// Si no lo hacemos, 'targetDistance' valdrá 0. Al hacer el primer scroll, la fórmula 
        /// del zoom provocará una división por cero (zoomSpeed / 0), lo que romperá el suavizado.
        targetDistance = followComponent.FollowOffset.magnitude;

        // Acotación de seguridad para que el zoom de inicio respete las reglas del juego.
        targetDistance = Mathf.Clamp(targetDistance, minDistance, maxDistance);
    }



    private void Update()
    {
        //el scroll da valores entre 120 o -120 dependiendo si se esta scrolleando hacia arriba o hacia abajo)
        float scrollInput = scrollAction.ReadValue<Vector2>().y;

        /// <summary>
        ///Mathf.Abs (Abs de Absolute) lee el valor absoluto de algo, osea que vuelve a los negativos en positivos
        ///por lo que aunque hagas scroll hacia arriba o hacia abajo el valor siempre sera positivo para este if
        ///lo que hago es basicamente saber si estoy haciendo scroll sin importar la direccion
        if (Mathf.Abs(scrollInput) > 0)
        {
            Vector2 mouseScreenPos = PointAction.ReadValue<Vector2>();

            Ray rayoCamaraARaton = Camera.main.ScreenPointToRay(mouseScreenPos);

            /// <summary>
            ///miramos si el rayo ha chocado contra la LayerMask teniendo en cuenta que el rayo mide infinito (Mathf.Infinity).
            ///tengo que ponerme las pilas para entender mathf porque tiene cosas super utiles
            if (Physics.Raycast(rayoCamaraARaton, out RaycastHit hit, Mathf.Infinity, planeLayerMask))
            {
                // Solo desplazamos el objetivo si estamos haciendo scroll hacia arriba 
                if (scrollInput > 0 && targetDistance > minDistance)
                {
                    /// <summary>
                    /// Calculamos un porcentaje de acercamiento hacia el punto del ratón basado la velocidad a la que hacemos scroll fisicamente
                    ///multiplicada por el valor de velocidad que le demos y dividido entre la distancia a la que estemos del objetivo
                    ///mientras mas lejos, mas rapido hacemos scroll y mientras mas cerca, mas lento. Para poder darle un valor a cualquier
                    ///posicion entre el valor maximo (1) y el minimo (0) de acercamiento para modificarlo y poder movernos entre estos dos puntos
                    ///usamos lerp factor. Aqui esta la explicacion de que es lerp que me he encontrado en google por si se me olvida en un futuro xD:
                    ///El factor Lerp (interpolación lineal) es un valor numérico (generalmente entre \(0.0\) y \(1.0\))
                    ///que calcula un punto intermedio entre un valor inicial y uno final.
                    ///Representa el porcentaje de avance: \(0\) devuelve el valor inicial (0%), \(1\) devuelve el valor final (100%),
                    ///y \(0.5\) devuelve el punto medio exacto.
                    float lerpFactor = Mathf.Clamp01((scrollInput * zoomSpeed) / targetDistance);
                    targetFollowPosition = Vector3.Lerp(targetFollowPosition, hit.point, lerpFactor);
                }
            }

            // 2. Modificar la distancia del zoom
            targetDistance -= scrollInput * zoomSpeed;
            targetDistance = Mathf.Clamp(targetDistance, minDistance, maxDistance);
        }

        // 3. Aplicar suavizado a la posición del objetivo en el plano
        dummyTarget.position = Vector3.SmoothDamp(dummyTarget.position, targetFollowPosition, ref positionVelocity, smoothTime);

        // 4. Aplicar suavizado al zoom (distancia de la cámara)
        Vector3 direction = followComponent.FollowOffset.normalized;
        Vector3 targetOffset = direction * targetDistance;

        followComponent.FollowOffset = Vector3.SmoothDamp(
            followComponent.FollowOffset,
            targetOffset,
            ref zoomVelocity,
            smoothTime
        );
    }

}

