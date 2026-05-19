using UnityEngine;

public class círculodentro : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("circulo dentro"))
        {
            GetComponent<AudioSource>().Play();
        }
    }
}
