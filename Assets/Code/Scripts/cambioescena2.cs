using UnityEngine;
using UnityEngine.SceneManagement; // Necesario para cambiar de escena

public class CambioEscena2 : MonoBehaviour
{
    // Carga la escena del menú principal
    public void VolverAlMenu()
    {
        SceneManager.LoadScene("MenuPrincipal_Scene");
    }
}