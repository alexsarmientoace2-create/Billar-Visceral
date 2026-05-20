using UnityEngine;
using UnityEngine.InputSystem;

public class BolaBlanca : MonoBehaviour
{
    [Header("Configuración de Movimiento")]
    public float velocidad = 1f;

    [Header("Referencias")]
    Rigidbody rb;
    InputAction actionMovimiento;
    public GameObject palo;
    public Collider tronera;
    public Transform posicionInicial;

    private void Awake()
    {
        actionMovimiento = InputSystem.actions.FindAction("Move");
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        Vector2 movimiento = actionMovimiento.ReadValue<Vector2>();
        Mover(movimiento);

        if (rb.linearVelocity.sqrMagnitude > 0.0001f)
        {
            palo.SetActive(false);
        }
        else
        {
            palo.SetActive(true);
        }


    }

    private void OnTriggerEnter(Collider trigger)
    {
        if (trigger == tronera)
        {

            GameManager.gM.CambioDeTurno();

            transform.position = posicionInicial.position;
        }

    }


    void Mover(Vector2 movimiento)
    {
        Vector3 direccion = (transform.forward * movimiento.y);
        direccion = direccion.normalized;
        Vector3 nuevaPosicion = rb.position + direccion * velocidad * Time.fixedDeltaTime;
        transform.position = nuevaPosicion;
    }
}


