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
                GameManager.gM.Bolanegraentra();
            }
            else if (esrayada == false)
            {
                GameManager.gM.BolaLisaEntra();
            }
            else if (esrayada == true)
            {
                GameManager.gM.BolaRayadaEntra();
            }
            Destroy(this.gameObject);
        }

    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }


}



