using UnityEngine;
using UnityEngine.InputSystem;

public class BolaBlancaController : MonoBehaviour
{
    [Header("Configuración de Movimiento")]
    public float velocidad = 1f;

    [Header("Configuración de Audio")]
    // Esta es la línea que crea la casilla en el Inspector
    public AudioClip sonidoChoque;
    private AudioSource fuenteAudio;

    [Header("Referencias")]
    private Rigidbody rb;
    private InputAction actionMovimiento;
    public GameObject palo;

    private void Awake()
    {
        // Configuramos la entrada
        actionMovimiento = InputSystem.actions.FindAction("Move");
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        // 1. Leer movimiento
        Vector2 inputMovimiento = actionMovimiento.ReadValue<Vector2>();
        Mover(inputMovimiento);

        // 2. Control del palo (se apaga si la bola se mueve)
        // Usamos sqrMagnitude para que sea más eficiente que Vector3.Distance
        if (rb.linearVelocity.sqrMagnitude > 0.001f)
        {
            if (palo.activeSelf) palo.SetActive(false);
        }
        else
        {
            if (!palo.activeSelf) palo.SetActive(true);
        }
    }

    void Mover(Vector2 movimiento)
    {
        // Solo movemos si hay input para evitar cálculos innecesarios
        if (movimiento.magnitude > 0.1f)
        {
            Vector3 direccion = transform.forward * movimiento.y;
            Vector3 nuevaPosicion = rb.position + direccion.normalized * velocidad * Time.fixedDeltaTime;
            rb.MovePosition(nuevaPosicion);
        }
    }

    // Evento de física que detecta el golpe del taco/palo
    private void OnCollisionEnter(Collision collision)
    {
        // Comprobamos si lo que nos golpeó es el palo (puedes usar el nombre o un Tag)
        if (collision.gameObject == palo || collision.gameObject.CompareTag("Player"))
        {
            if (sonidoChoque != null)
            {
                fuenteAudio.PlayOneShot(sonidoChoque);
                Debug.Log("¡Sonido reproducido!");
            }
        }
    }
}