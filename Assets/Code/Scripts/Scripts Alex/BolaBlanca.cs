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

    private void Update()
    {
        if (rb.angularVelocity.sqrMagnitude < 1f)
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
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


