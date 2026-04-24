using UnityEngine;
using UnityEngine.InputSystem; // Obligatorio para el nuevo sistema

public class ControladorBillar : MonoBehaviour
{
    [Header("Referencias a Inputs (Arrastrar desde el Project)")]
    public InputActionReference accionApuntar;
    public InputActionReference accionGolpear;

    [Header("Física y Objetos")]
    public Rigidbody bolaBlanca;
    public Transform modeloTaco; // El hijo que se mueve hacia atrás al cargar

    [Header("Ajustes")]
    public float sensibilidadGiro = 0.5f;
    public float fuerzaMaxima = 20f;
    public float multiplicadorCarga = 15f;

    private bool cargando = false;
    private float fuerzaActual = 0f;
    private Vector3 posicionLocalOriginalTaco;

    void Awake()
    {
        // Guardamos la posición inicial del taco para que sepa a dónde volver
        posicionLocalOriginalTaco = modeloTaco.localPosition;
    }

    void OnEnable()
    {
        // 1. Activar las acciones
        accionApuntar.action.Enable();
        accionGolpear.action.Enable();

        // 2. Suscribirse a los eventos del clic (Delegados de C#)
        accionGolpear.action.started += IniciarCarga;   // Cuando el clic baja
        accionGolpear.action.canceled += EjecutarGolpe; // Cuando el clic sube
    }

    void OnDisable()
    {
        // Limpiar la memoria al desactivar el objeto
        accionGolpear.action.started -= IniciarCarga;
        accionGolpear.action.canceled -= EjecutarGolpe;
        
        accionApuntar.action.Disable();
        accionGolpear.action.Disable();
    }

    void Update()
    {
        // 1. El pivote siempre sigue a la bola (pero no adopta su rotación)
        transform.position = bolaBlanca.position;

        // 2. Rotar el pivote usando el movimiento del ratón
        Vector2 deltaRaton = accionApuntar.action.ReadValue<Vector2>();
        transform.Rotate(Vector3.up, deltaRaton.x * sensibilidadGiro);

        // 3. Efecto visual y cálculo de carga continua
        if (cargando)
        {
            fuerzaActual += Time.deltaTime * multiplicadorCarga;
            fuerzaActual = Mathf.Clamp(fuerzaActual, 0, fuerzaMaxima);

            // Mover el taco hacia atrás visualmente (eje Z local)
            float retrocesoVisual = fuerzaActual * 0.05f;
            modeloTaco.localPosition = posicionLocalOriginalTaco - new Vector3(0, 0, retrocesoVisual);
        }
    }

    // Método que se llama automáticamente al PRESIONAR el clic
    private void IniciarCarga(InputAction.CallbackContext context)
    {
        cargando = true;
        fuerzaActual = 0f;
    }

    // Método que se llama automáticamente al SOLTAR el clic
    private void EjecutarGolpe(InputAction.CallbackContext context)
    {
        if (!cargando) return;

        // Aplicar la fuerza a la bola en la dirección hacia la que mira el pivote
        bolaBlanca.AddForce(transform.forward * fuerzaActual, ForceMode.Impulse);

        // Reiniciar variables y devolver el taco a su posición original
        cargando = false;
        fuerzaActual = 0f;
        modeloTaco.localPosition = posicionLocalOriginalTaco;
    }
}