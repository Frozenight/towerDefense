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
        gameObject.SetActive(true);
    }

    public void TurnOff()
    {
        gameObject.SetActive(false);
    }

    public void PickFirstButton()
    {
        WorkerMovement.IncreaseMS();
        gameOverScreen.Setup();
        TurnOff();
    }

    public void PickSecondButton()
    {
        building_base.TestIncreaseHp();
        building_base2.TestIncreaseHp();
        gameOverScreen.Setup();
        TurnOff();
    }
}
