using UnityEngine;
using UnityEngine.InputSystem;

public class ControladorBillar : MonoBehaviour
{
    [Header("Referencias a Inputs")]
    public InputActionReference accionGolpear;
    private InputAction actionPosicionRaton;

    [Header("Componentes de la Bola y el Taco")]
    public Rigidbody bolaBlanca;
    public Transform modeloTaco;
    public Transform PosicionBolaBlanca;
    public Transform culopalo;
    private Vector3 vectorpalo;

    // Guarda el punto exacto del espacio 3D donde el jugador hizo click
    private Vector3 puntoClickMundo;

    [Header("Ajustes de Arrastre 3D")]
    [Tooltip("Capa de la mesa o el suelo para que el ratón sepa dónde está pisando en 3D")]
    [SerializeField] private LayerMask capaMesa = ~0;
    [Tooltip("Distancia máxima en metros del juego que debes alejar el ratón (hacia cualquier lado) desde el click para la fuerza tope")]
    [SerializeField] private float distanciaMaximaMundo = 2f;

    [Header("Ajustes de Fuerza y Visuales")]
    public float fuerzaMaxima = 20f;
    public float maxRetrocesoTacoMetros = 0.5f;

    private bool cargando = false;
    private float fuerzaActual = 0f;

    /// <summary>
    /// Activa las acciones del Input System y suscribe los eventos del Click del ratón.
    /// </summary>
    void OnEnable()
    {
        actionPosicionRaton = InputSystem.actions.FindAction("Point");

        accionGolpear.action.Enable();
        accionGolpear.action.started += IniciarCarga;
        accionGolpear.action.canceled += EjecutarGolpe;
    }

    /// <summary>
    /// Limpia los eventos en memoria al desactivar el script para evitar errores raros en Unity.
    /// </summary>
    void OnDisable()
    {
        accionGolpear.action.started -= IniciarCarga;
        accionGolpear.action.canceled -= EjecutarGolpe;
        accionGolpear.action.Disable();
    }

    /// <summary>
    /// El núcleo del script. Controla el apuntado libre en 360 grados y, durante la carga, 
    /// congela la rotación permitiendo acumular fuerza moviendo el ratón hacia cualquier sitio.
    /// </summary>
    void Update()
    {
        // Guardamos la dirección básica del taco (del culo a la punta) para los cálculos de movimiento.
        vectorpalo = culopalo.position - PosicionBolaBlanca.position;

        // El pivote del script se pega a la bola blanca en cada frame, pero mantiene su propia rotación.
        transform.position = PosicionBolaBlanca.position;

        // Buscamos dónde está apuntando el cursor en el suelo 3D de la mesa de billar.
        Vector3 posicionRatonEnMundo = ObtenerPosicionRatonEnMundo();

        if (!cargando)
        {
            // --- MODO APUNTADO LIBRE ---
            // El taco gira libremente siguiendo al cursor antes de hacer click.
            Vector3 direccionHaciaRaton = posicionRatonEnMundo - PosicionBolaBlanca.position;
            direccionHaciaRaton.y = 0;

            if (direccionHaciaRaton.sqrMagnitude > 0.001f)
            {
                transform.forward = direccionHaciaRaton.normalized;
            }

            // Mantenemos el taco en su posición de reposo inicial justo al lado de la bola.
            modeloTaco.position = PosicionBolaBlanca.position + vectorpalo.normalized * 0.05f;
        }
        else
        {
            // --- MODO CARGA (ROTACIÓN CONGELADA + DIRECCIÓN LIBRE) ---
            // Calculamos el vector desde el punto donde hiciste click hasta donde tienes el ratón ahora.
            Vector3 vectorArrastre = posicionRatonEnMundo - puntoClickMundo;
            vectorArrastre.y = 0;

            // Medimos la distancia absoluta en línea recta (magnitud). 
            // Al no usar 'Dot Product', da igual si vas hacia atrás, hacia los lados o en diagonal; 
            // solo importa cuánto te alejas físicamente del punto donde hiciste click.
            float distanciaArrastre = vectorArrastre.magnitude;

            // Convertimos esa distancia de arrastre en un porcentaje limpio (0.0 a 1.0).
            float porcentajeCarga = Mathf.Clamp01(distanciaArrastre / distanciaMaximaMundo);

            // Traducimos ese porcentaje a la fuerza real de disparo y al desplazamiento visual del taco.
            fuerzaActual = porcentajeCarga * fuerzaMaxima;
            float retrocesoVisual = porcentajeCarga * maxRetrocesoTacoMetros;

            // El taco retrocede estrictamente en su carril congelado, pero la fuerza responde a cualquier dirección.
            modeloTaco.position = PosicionBolaBlanca.position + vectorpalo.normalized * (0.05f + retrocesoVisual);
        }
    }

    /// <summary>
    /// Proyecta un rayo invisible (Raycast) desde la lente de la cámara hacia la pantalla en la posición del cursor, 
    /// permitiéndonos averiguar las coordenadas X, Y, Z exactas de la mesa de billar que está señalando el usuario.
    /// </summary>
    private Vector3 ObtenerPosicionRatonEnMundo()
    {
        Vector2 mouseScreenPos = actionPosicionRaton.ReadValue<Vector2>();
        Ray rayoCamara = Camera.main.ScreenPointToRay(mouseScreenPos);

        if (Physics.Raycast(rayoCamara, out RaycastHit hit, Mathf.Infinity, capaMesa))
        {
            return hit.point;
        }

        return PosicionBolaBlanca.position;
    }

    /// <summary>
    /// Se ejecuta automáticamente en el instante en que haces click izquierdo. 
    /// Bloquea la rotación del taco y define el "centro" desde donde empezarás a medir la distancia de arrastre.
    /// </summary>
    private void IniciarCarga(InputAction.CallbackContext context)
    {
        cargando = true;
        fuerzaActual = 0f;

        // Registramos exactamente en qué coordenada de la mesa estaba el cursor al hacer click.
        puntoClickMundo = ObtenerPosicionRatonEnMundo();
    }

    /// <summary>
    /// Se ejecuta automáticamente en el instante en que sueltas el click izquierdo. 
    /// Transfiere la fuerza acumulada a la bola en la dirección en la que quedó bloqueado el taco.
    /// </summary>
    private void EjecutarGolpe(InputAction.CallbackContext context)
    {
        if (!cargando) return;

        // Golpeamos la bola usando el transform.forward congelado.
        bolaBlanca.AddForce(transform.forward * fuerzaActual, ForceMode.Impulse);

        // Reseteamos el estado para el siguiente turno, liberando la rotación.
        cargando = false;
        fuerzaActual = 0f;
    }
}