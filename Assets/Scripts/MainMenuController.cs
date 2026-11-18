using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    public GameObject panelMenuPrincipal;
    public GameObject panelHistoria1;
    public GameObject panelHistoria2;
    public GameObject panelHistoria3;
    public GameObject panelTutorial;

    public void IrAHistoria()
    {
        panelMenuPrincipal.SetActive(false);
        panelHistoria1.SetActive(true);
    }
    public void SiguienteAHistoria2()
    {
        panelHistoria1.SetActive(false);
        panelHistoria2.SetActive(true);
    }

    public void SiguienteAHistoria3()
    {
        panelHistoria2.SetActive(false);
        panelHistoria3.SetActive(true);
    }
    public void IrATutorial()
    {
        panelHistoria3.SetActive(false);
        panelTutorial.SetActive(true);
    }
    public void CargarJuego()
    {
        SceneManager.LoadScene("NombreDeTuEscenaDeJuego");
    }
    public void SalirDelJuego()
    {
        Debug.Log("Saliendo del juego...");
        Application.Quit();
    }
}