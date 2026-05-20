using UnityEngine;
using UnityEngine.SceneManagement; // Requerido para gestionar escenas

public class CambioEscena : MonoBehaviour
{
    // Función para cargar una escena por su nombre
    public void CargarEscena(string nombreEscena)
    {
        SceneManager.LoadScene(nombreEscena);
    }

    // Función para salir del juego
    public void SalirDelJuego()
    {
        Debug.Log("Saliendo del juego..."); // Solo se ve en el editor
        Application.Quit(); // Cierra la aplicación (solo funciona en el build final)
    }
}