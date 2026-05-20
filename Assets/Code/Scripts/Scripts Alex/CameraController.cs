using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    private CinemachineCamera vcam;
    private CinemachineFollow followComponent;

    // Acciones del New Input System
    private InputAction scrollAction;
    private InputAction mousePositionAction;

    [Header("Configuración de Entrada")]
    [SerializeField] private string scrollActionName = "scroll";
    [SerializeField] private string mousePositionActionName = "point"; // Acción por defecto del Input System para la posición

    [Header("Límites de Zoom")]
    [SerializeField] private float zoomSpeed = 0.05f; // Ajustado para el valor raw de la rueda del ratón (e.g., 120)
    [SerializeField] private float minDistance = 5f;
    [SerializeField] private float maxDistance = 30f;
    [SerializeField] private float smoothTime = 0.15f;

    [Header("Ajuste de Plano")]
    [SerializeField] private LayerMask planeLayerMask = ~0; // Capa en la que está tu plano 3D

    private float targetDistance;
    private Vector3 zoomVelocity;

    // Variables para el desplazamiento del objetivo
    public Transform dummyTarget;
    private Vector3 targetFollowPosition;
    private Vector3 positionVelocity;

    private void Start()
    {
        vcam = GetComponent<CinemachineCamera>();
        if (vcam != null)
        {
            followComponent = vcam.GetComponent<CinemachineFollow>();
        }

        // Buscar acciones globales en Project Settings
        scrollAction = InputSystem.actions.FindAction(scrollActionName);
        mousePositionAction = InputSystem.actions.FindAction(mousePositionActionName);

        // Si "point" no existe por defecto, intentamos buscar "position" o similar
        if (mousePositionAction == null)
        {
            mousePositionAction = InputSystem.actions.FindAction("position");
        }

        if (scrollAction == null)
        {
            Debug.LogError($"No se encontró la acción '{scrollActionName}' en InputSystem.actions.");
            return;
        }

        if (followComponent == null || vcam.Follow == null)
        {
            Debug.LogError("Asegúrate de que la CinemachineCamera tiene asignado un 'Follow' inicial y el componente CinemachineFollow.");
            return;
        }

        // Configurar el sistema de Dummy Target para no modificar el objeto original del jugador
        InitDummyTarget();
    }

    private void InitDummyTarget()
    {
        targetFollowPosition = vcam.Follow.position;
        vcam.Follow = dummyTarget;
        targetDistance = followComponent.FollowOffset.magnitude;
    }

    private void Update()
    {
        if (followComponent == null || scrollAction == null || Camera.main == null) return;

        // 1. Leer el valor del scroll (Eje de la rueda del ratón, habitualmente da valores como 120 o -120)
        float scrollInput = scrollAction.ReadValue<Vector2>().y;

        if (Mathf.Abs(scrollInput) > Mathf.Epsilon)
        {
            // Conseguir la posición en pantalla del ratón
            Vector2 mouseScreenPos = mousePositionAction != null ? mousePositionAction.ReadValue<Vector2>() : Mouse.current.position.ReadValue();

            // Trazar un rayo desde la cámara principal hacia el plano 3D
            Ray ray = Camera.main.ScreenPointToRay(mouseScreenPos);

            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, planeLayerMask))
            {
                // Solo desplazamos el objetivo si estamos haciendo zoom HACIA ADENTRO (scrollInput > 0)
                if (scrollInput > 0 && targetDistance > minDistance)
                {
                    // Calculamos un porcentaje de acercamiento hacia el punto del ratón basado en la fuerza del scroll
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

