using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOverPowerUP : MonoBehaviour
{
    public Building_Base building_base;
    public Building_Base building_base2;

    public MovementAnimated WorkerMovement;

    public GameOverScreen gameOverScreen;
    public void Setup()
    {
        Time.timeScale = 0;
        gameObject.SetActive(true);
    }

    public void TurnOff()
    {
        gameObject.SetActive(false);
    }

    public void PickFirstButton()
    {
        Time.timeScale = 1;
        WorkerMovement.IncreaseMS();
        gameOverScreen.Setup();
        TurnOff();
    }

    public void PickSecondButton()
    {
        Time.timeScale = 1;
        building_base.TestIncreaseHp();
        //building_base2.TestIncreaseHp();
        gameOverScreen.Setup();
        TurnOff();
    }
}
