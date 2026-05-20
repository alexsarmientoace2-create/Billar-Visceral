using UnityEngine;
using UnityEngine.SceneManagement;

public class CambioEscena : MonoBehaviour
{
    // Esta función cargará directamente la escena "instrucciones"
    public void IrAInstrucciones()
    {
        SceneManager.LoadScene("instrucciones");
    }

    public void SalirDelJuego()
    {
        Debug.Log("Saliendo del juego...");
        Application.Quit();
    }
}