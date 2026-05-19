using UnityEngine;
using UnityEngine.UI; // Necesario para usar UI como la Scrollbar

public class MenuOpciones : MonoBehaviour
{
    [Header("Referencias de UI")]
    [Tooltip("Arrastra aquí el Canvas de Opciones desde la jerarquía")]
    public GameObject canvaOpciones;

    [Tooltip("Arrastra aquí tu Scrollbar de sonido")]
    public Scrollbar scrollbarSonido;

    void Start()
    {
        // Opcional: Que la scrollbar empiece en el nivel de volumen actual del juego
        if (scrollbarSonido != null)
        {
            scrollbarSonido.value = AudioListener.volume;
        }
    }

    // --- NUEVA FUNCIÓN PARA EL MENÚ PRINCIPAL ---
    public void AbrirMenuOpciones()
    {
        // Activa el Canvas de opciones (lo muestra)
        canvaOpciones.SetActive(true);
    }

    // Este método lo conectaremos a la Scrollbar
    public void CambiarVolumenGeneral(float volumen)
    {
        // AudioListener.volume controla el sonido maestro del juego (va de 0.0 a 1.0)
        AudioListener.volume = volumen;
    }

    // Este método lo conectaremos al Botón de Salir
    public void CerrarMenuOpciones()
    {
        // Desactiva el Canvas de opciones (lo oculta)
        canvaOpciones.SetActive(false);
    }
}