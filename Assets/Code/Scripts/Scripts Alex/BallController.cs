using UnityEngine;

public class BallController : MonoBehaviour
{
    public bool esrayada;
    public bool es8;
    Rigidbody rb;


    public Collider tronera;

    private void OnTriggerEnter(Collider trigger)
    {
        if (trigger == tronera)
        {
            if (es8 == true)
            {
                GameManager.gM.bolanegraentra();
            }
            else if (esrayada == false)
            {
                GameManager.gM.bolaLisaEntra();
            }
            else if (esrayada == true)
            {
                GameManager.gM.bolaRayadaEntra();
            }
            Destroy(this.gameObject);
        }

    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (rb.angularVelocity.sqrMagnitude < 1)
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }
    }


}
