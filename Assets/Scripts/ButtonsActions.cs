using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonsActions : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private GameObject optionsCanvas;
    [SerializeField] private GameObject controlsCanvas;

    void Start()
    {
        optionsCanvas.SetActive(false);  // Asegura que el canvas de opciones esté deshabilitado al inicio
        controlsCanvas.SetActive(false); // Asegura que el canvas de controles esté deshabilitado al inicio
    }

    // Función para el botón de Jugar
    public void OnPlayButtonClicked()
    {
        SceneManager.LoadScene("Level1"); // Cambia "Level1" por el nombre de tu escena
    }

    // Función para el botón de Opciones
    public void OnOptionsButtonClicked()
    {
        optionsCanvas.SetActive(true);  // Muestra el canvas de opciones
        controlsCanvas.SetActive(false); // Asegura que el canvas de controles se oculte si está visible
    }

    // Función para el botón de ver controles
    public void OnControlsButtonClicked()
    {
        controlsCanvas.SetActive(true); // Muestra el canvas de controles
        optionsCanvas.SetActive(false); // Asegura que el canvas de opciones se oculte si está visible
    }

    // Función para el botón de Salir
    public void OnExitButtonClicked()
    {
        Application.Quit();  // Cierra la aplicación
    }

    // Función para ocultar el canvas de opciones
    public void OnCloseOptionsButtonClicked()
    {
        optionsCanvas.SetActive(false);  // Oculta el canvas de opciones
    }

    // Función para ocultar el canvas de controles
    public void OnCloseControlsButtonClicked()
    {
        controlsCanvas.SetActive(false);  // Oculta el canvas de controles
    }
}
