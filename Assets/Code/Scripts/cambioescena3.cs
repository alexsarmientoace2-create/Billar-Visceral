using UnityEngine;
using UnityEngine.SceneManagement; // Necesario para cambiar de escena

public class CambioEscena3 : MonoBehaviour
{
    // Carga la escena de desarrollo de Oliver
    public void EscenaDesarrolloOliver()
    {
        SceneManager.LoadScene("EscenaDesarrolloOliver_Scene");
    }
}