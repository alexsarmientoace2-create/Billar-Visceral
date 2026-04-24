using UnityEngine;

public class BallController : MonoBehaviour
{

    public Collider tronera;

    private void OnTriggerEnter(Collider tronera)
    {
        Destroy(this.gameObject);
    }
}
