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


