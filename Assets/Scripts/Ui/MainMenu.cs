using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    [SerializeField] private GameObject playButton;
    [SerializeField] private GameObject exitButton;
    [SerializeField] private GameObject loadingScreen;
    private Canvas _mainMenuCanvas;

    public string Level_One;

    private void Start()
    {
        _mainMenuCanvas = GetComponent<Canvas>();
    }

    public void Start_Game()
    {
        playButton.SetActive(false);
        exitButton.SetActive(false);
        loadingScreen.SetActive(true);
        SceneManager.LoadScene(Level_One);
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
