using UnityEngine;

public class choquebate : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("bate"))
        {
            GetComponent<AudioSource>().Play();
        }
    }
}
