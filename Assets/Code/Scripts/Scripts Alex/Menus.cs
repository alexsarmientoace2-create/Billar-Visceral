using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menus : MonoBehaviour
{
   
    public GameObject menu;
    public GameObject AjustesPanel;

    public void CambiarAMenu()
    {
        SceneManager.LoadScene(0);
    }
    public void CambiarAJuego()
    {
        SceneManager.LoadScene(1);
    }


    public void Salir()
    {
        Application.Quit();
    }

    public void Ajustes()
    {
        menu.SetActive(false);
        AjustesPanel.SetActive(true);
    }

    public void SalirAjustes()
    {
        menu.SetActive(true);
        AjustesPanel.SetActive(false);
    }


}
