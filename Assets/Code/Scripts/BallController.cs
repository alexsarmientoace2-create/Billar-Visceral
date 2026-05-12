using UnityEngine;

public class BallController : MonoBehaviour
{
    public bool esrayada;
    public bool es8;
    public bool esblanca;


    public Collider tronera;

    private void OnTriggerEnter(Collider tronera)
    {
        Destroy(this.gameObject);
    }

    private void Awake()
    {
        
    }

    private void Update()
    {
        
    }

    
}
