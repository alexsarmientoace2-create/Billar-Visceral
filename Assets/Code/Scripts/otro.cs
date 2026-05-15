/*
using UnityEngine;

public class codebate PLAYERCONTROLLER: MonoBehaviour
{
    public float moveSpeed = 10f;
    public float rotationSpeed = 100f; // Para que el taco gire alrededor de la bola

    private Rigidbody rbody;

    void Start()
    {
        rbody = GetComponent<Rigidbody>();
        // Si es un taco de billar, normalmente no queremos que se caiga con la gravedad
        rbody.useGravity = false;
    }

    void Update()
    {
        // 1. Obtener entradas (Funciona con WASD y Flechas por defecto)
        float horizontal = Input.GetAxis("Horizontal"); // A/D o Izquierda/Derecha
        float vertical = Input.GetAxis("Vertical");     // W/S o Arriba/Abajo

        // 2. Calcular dirección de movimiento relativo al taco
        Vector3 movement = new Vector3(horizontal, 0, vertical) * moveSpeed * Time.deltaTime;

        // 3. Mover el taco
        transform.Translate(movement, Space.Self);

        // EXTRA: Si quieres que el taco "golpee" con la tecla E
        if (Input.GetKeyDown(KeyCode.E))
        {
            GolpearBola();
        }
    }

    void GolpearBola()
    {
        // Aquí puedes añadir una pequeña animación o impulso hacia adelante
        Debug.Log("¡Golpe!");
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "Bouncer")
        {
            // 1. Buscamos el Rigidbody de la pelota
            Rigidbody rbBola = collision.gameObject.GetComponent<Rigidbody>();

            if (rbBola != null)
            {
                // 2. Calculamos la dirección (desde el palo hacia la bola)
                Vector3 direccionGolpe = collision.contacts[0].point - transform.position;
                direccionGolpe.y = 0; // Evitamos que la bola salga volando hacia arriba

                // 3. Aplicamos un impulso manual (Aquí controlas TÚ la fuerza)
                float fuerzaImpacto = 20f;
                rbBola.AddForce(direccionGolpe.normalized * fuerzaImpacto, ForceMode.Impulse);
            }

            // Detenemos el palo para que no siga avanzando
            rbody.linearVelocity = Vector3.zero;
        }
    }
}
*/

/*
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Configuración de Movimiento")]
    public float velocidad = 500f;

    [Header("Referencias")]
    Rigidbody rb;
    InputAction actionMovimiento;
    Animator anim;
    public Rigidbody bolablanca;
    public GameObject palo;

    private void Awake()
    {
        actionMovimiento = InputSystem.actions.FindAction("Move");
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        Vector2 movimiento = actionMovimiento.ReadValue<Vector2>();
        Mover(movimiento);

        if (bolablanca.linearVelocity.sqrMagnitude > 0.0001f)
        {
            palo.SetActive(false);
        }
        else
        {
            palo.SetActive(true);
        }

        void Mover(Vector2 movimiento)
        {
            Vector3 direccionRelativa = (transform.right * movimiento.x) + (transform.forward * movimiento.y);
            direccionRelativa = direccionRelativa.normalized;
            Vector3 nuevaPosicion = rb.position + direccionRelativa * velocidad * Time.fixedDeltaTime;
            rb.MovePosition(nuevaPosicion);
        }
    }
}
*/