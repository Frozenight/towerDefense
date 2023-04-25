using UnityEngine;
using UnityEngine.SceneManagement;

public class Pause_menu : MonoBehaviour
{
    [SerializeField] GameObject _pause_menu;

    public void Pause_Game()
    {
        Time.timeScale = 0;
        _pause_menu.SetActive(true);
        GameMode gameMode = GameObject.FindObjectOfType<GameController>().GetComponent<GameMode>();
        gameMode.changeGameMode(3);
        
    }

    public void Resume_Game()
    {
        Time.timeScale = 1;
        _pause_menu.SetActive(false);
    }

    public void Disconnect()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
