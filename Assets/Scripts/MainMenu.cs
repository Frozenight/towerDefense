using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    private Canvas _mainMenuCanvas;

    private void Start()
    {
        _mainMenuCanvas = GetComponent<Canvas>();
    }

    public void Start_Game()
    {
        SceneManager.LoadScene("GameSceneName");
    }

    public void Open_Settings()
    {
        _mainMenuCanvas.enabled = false;
    }

    public void Exit_Game()
    {
        Application.Quit();
    }
}
