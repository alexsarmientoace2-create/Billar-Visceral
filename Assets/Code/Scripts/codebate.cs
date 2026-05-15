using UnityEngine;

public class PlayerMoveScript : MonoBehaviour
{
    public float moveSpeed = 10f;
    public float rotationSpeed = 100f;

    // Solo necesitamos el archivo de audio (.mp3 o .wav)
    public AudioClip sonidoChoque;

    private Rigidbody rbody;
    private AudioSource fuenteAudio; // Se configurará por código

    void Start()
    {
        rbody = GetComponent<Rigidbody>();
        rbody.useGravity = false;

        // --- CONFIGURACIÓN POR CÓDIGO ---
        // Creamos el componente de audio automáticamente si no existe
        fuenteAudio = gameObject.AddComponent<AudioSource>();
        fuenteAudio.playOnAwake = false;
        fuenteAudio.spatialBlend = 1.0f; // Esto hace que el sonido sea 3D
    }

    void Update()
    {
        // 1. Obtener entradas
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        // 2. Calcular dirección
        Vector3 movement = new Vector3(horizontal, 0, vertical) * moveSpeed * Time.deltaTime;

        // 3. Mover el taco
        transform.Translate(movement, Space.Self);

        if (Input.GetKeyDown(KeyCode.E))
        {
            GolpearBola();
        }
    }

    void GolpearBola()
    {
        Debug.Log("¡Golpe!");
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "Bouncer")
        {
            // --- REPRODUCCIÓN POR CÓDIGO ---
            if (sonidoChoque != null)
            {
                // Usamos PlayOneShot para que el sonido no se corte si el palo se para
                fuenteAudio.PlayOneShot(sonidoChoque);
            }

            // 1. Buscamos el Rigidbody de la pelota
            Rigidbody rbBola = collision.gameObject.GetComponent<Rigidbody>();

            if (rbBola != null)
            {
                // 2. Calculamos el punto exacto de contacto (EL DONDE)
                Vector3 direccionGolpe = collision.contacts[0].point - transform.position;
                direccionGolpe.y = 0;

                // 3. Aplicamos el impulso
                float fuerzaImpacto = 20f;
                rbBola.AddForce(direccionGolpe.normalized * fuerzaImpacto, ForceMode.Impulse);
            }

            // Detenemos el palo para que no atraviese la bola
            rbody.linearVelocity = Vector3.zero;
        }
    }
}