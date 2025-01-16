using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonsActions : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private GameObject optionsCanvas;
    [SerializeField] private GameObject controlsCanvas;

    void Start()
    {
        optionsCanvas.SetActive(false);  
        controlsCanvas.SetActive(false); 
    }

    
    public void OnPlayButtonClicked()
    {
        SceneManager.LoadScene("Level1"); 
    }

    
    public void OnOptionsButtonClicked()
    {
        optionsCanvas.SetActive(true);  
        controlsCanvas.SetActive(false); 
    }

    
    public void OnControlsButtonClicked()
    {
        controlsCanvas.SetActive(true); 
        optionsCanvas.SetActive(false); 
    }

    
    public void OnExitButtonClicked()
    {
        Application.Quit();  
    }

    
    public void OnCloseOptionsButtonClicked()
    {
        optionsCanvas.SetActive(false);  
    }

    
    public void OnCloseControlsButtonClicked()
    {
        controlsCanvas.SetActive(false);  
    }
}
