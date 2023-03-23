using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOverScreen : MonoBehaviour
{
    public void Setup()
    {
        gameObject.SetActive(true);
    }

    public void RestartButton()
    {
        GameController.instance.SaveGame();
        SceneManager.LoadScene("Level_One");
    }

    public void ExitButton()
    {
        GameController.instance.SaveGame();
        SceneManager.LoadScene("MainMenu");
    }
}
