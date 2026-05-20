using UnityEngine;

public class filodelamesa : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Bola"))
        {
            GetComponent<AudioSource>().Play();
        }
    }
}
