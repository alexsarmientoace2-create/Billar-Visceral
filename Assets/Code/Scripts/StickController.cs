using UnityEngine;
using UnityEngine.InputSystem; // Obligatorio para el nuevo sistema

public class ControladorBillar : MonoBehaviour
{
    [Header("Referencias a Inputs (Arrastrar desde el Project)")]
    public InputActionReference accionApuntar;
    public InputActionReference accionGolpear;

    [Header("F�sica y Objetos")]
    public Rigidbody bolaBlanca;
    public Transform modeloTaco; // El hijo que se mueve hacia atr�s al cargar
    public Transform PosicionBolaBlanca;
    public Transform culopalo;
    Vector3 vectorpalo;
    public Rigidbody bolablanca;

    [Header("Ajustes")]
    public float sensibilidadGiro = 0.5f;
    public float fuerzaMaxima = 20f;
    public float multiplicadorCarga = 15f;

    private bool cargando = false;
    private float fuerzaActual = 0f;
    

    void Awake()
    {
      
    }

    void OnEnable()
    {
        // 1. Activar las acciones

        if (bolaBlanca.linearVelocity.sqrMagnitude < 0.01f)
        {
            accionApuntar.action.Enable();
            accionGolpear.action.Enable();

            // 2. Suscribirse a los eventos del clic (Delegados de C#)
            accionGolpear.action.started += IniciarCarga;   // Cuando el clic baja
            accionGolpear.action.canceled += EjecutarGolpe; // Cuando el clic sube}
        }

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

        // 0. Actualizamos el vector de direcci�n del palo para mas adelante usarlo en el retoceso visual y en las guias de tiro
        vectorpalo = culopalo.position - PosicionBolaBlanca.position;

        // 1. El pivote siempre sigue a la bola (pero no adopta su rotaci�n)
        transform.position = PosicionBolaBlanca.position;

        // 2. Rotar el pivote usando el movimiento del rat�n
        Vector2 deltaRaton = accionApuntar.action.ReadValue<Vector2>();
        transform.Rotate(Vector3.up, deltaRaton.x * sensibilidadGiro);

        // 3. Efecto visual y c�lculo de carga continua
        if (cargando)
        {
            fuerzaActual += Time.deltaTime * multiplicadorCarga;
            fuerzaActual = Mathf.Clamp(fuerzaActual, 0, fuerzaMaxima);

            // Mover el taco hacia atr�s visualmente respecto al vector creado en el paso 0
            float retrocesoVisual = fuerzaActual * 0.03f;
            modeloTaco.localPosition = PosicionBolaBlanca.position + vectorpalo * retrocesoVisual;
        }
    }

    // M�todo que se llama autom�ticamente al PRESIONAR el clic
    private void IniciarCarga(InputAction.CallbackContext context)
    {
        cargando = true;
        fuerzaActual = 0f;
    }

    // M�todo que se llama autom�ticamente al SOLTAR el clic
    private void EjecutarGolpe(InputAction.CallbackContext context)
    {
        if (!cargando) return;

        // Aplicar la fuerza a la bola en la direcci�n hacia la que mira el pivote
        bolaBlanca.AddForce(transform.forward * fuerzaActual, ForceMode.Impulse);

        // Reiniciar variables y devolver el taco a su posici�n original
        cargando = false;
        fuerzaActual = 0f;
        
    }
}