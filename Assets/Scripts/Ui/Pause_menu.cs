using UnityEngine;
using UnityEngine.SceneManagement;

public class Pause_menu : MonoBehaviour
{
    [SerializeField] GameObject _pause_menu;
    GameMode gameMode;

    public void Start()
    {
        gameMode = GameObject.FindObjectOfType<GameController>().GetComponent<GameMode>();
    }

    public void Pause_Game()
    {
        Time.timeScale = 0;
        _pause_menu.SetActive(true);
        gameMode.changeGameMode(2);
        
    }

    public void Resume_Game()
    {
        Time.timeScale = 1;
        _pause_menu.SetActive(false);
        gameMode.changeGameMode(1);
    }

    public void Disconnect()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
