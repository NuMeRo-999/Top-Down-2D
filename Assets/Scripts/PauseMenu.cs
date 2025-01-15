using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private GameObject pauseMenuCanvas;
    [SerializeField] private GameObject optionsCanvas;  
    [SerializeField] private GameObject controlsCanvas; 
    private bool isPaused = false;

    void Update()
    {
    
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }


    public void PauseGame()
    {
        pauseMenuCanvas.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
    }


    public void ResumeGame()
    {
        pauseMenuCanvas.SetActive(false);
        optionsCanvas.SetActive(false);
        controlsCanvas.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
    }


    public void GoToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }


    public void ExitGame()
    {
        Application.Quit();
    }


    public void ShowOptions()
    {
        optionsCanvas.SetActive(true);
        pauseMenuCanvas.SetActive(false);
    }


    public void BackToPauseMenuFromOptions()
    {
        optionsCanvas.SetActive(false);
        pauseMenuCanvas.SetActive(true);
    }


    public void ShowControls()
    {
        controlsCanvas.SetActive(true);
        pauseMenuCanvas.SetActive(false);
    }


    public void BackToPauseMenuFromControls()
    {
        controlsCanvas.SetActive(false);
        pauseMenuCanvas.SetActive(true);
    }
}
