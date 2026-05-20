using UnityEngine;
using UnityEngine.InputSystem;

public class BolaBlanca : MonoBehaviour
{
    [Header("Configuración de Movimiento")]
    public float velocidad = 1f;

    [Header("Referencias")]
    Rigidbody rb;
    
    public GameObject palo;
    public Collider tronera;
    public Transform posicionInicial;

    private void Awake()
    {
        
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        
        

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
            rb.linearVelocity = Vector3.zero;

            rb.angularVelocity = Vector3.zero;

            GameManager.gM.CambioDeTurno();

            transform.position = posicionInicial.position;
        }

    }




   
}


