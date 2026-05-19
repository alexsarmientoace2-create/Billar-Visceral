using UnityEngine;

public class SonidoChoqueBolas : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Bola"))
        {
            GetComponent<AudioSource>().Play();
        }
    }
}
