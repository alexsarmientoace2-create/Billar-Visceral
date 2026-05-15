using UnityEngine;

public class BallController : MonoBehaviour
{
    public bool esrayada;
    public bool es8;


    public Collider tronera;

    private void OnTriggerEnter(Collider triggerTronera)
    {
        if (triggerTronera == tronera)
        Destroy(this.gameObject);
    }

    private void Awake()
    {
        
    }

    private void Update()
    {
        
    }

    
}
