using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpScreen : MonoBehaviour

{
    // Start is called before the first frame update
    public Building_Base building_base;
    public MovementAnimated WorkerMovement;
  //  public int WallHealthAdd = 100;
    
    public void Setup()
    {
        Time.timeScale = 0;
        gameObject.SetActive(true);
    }

    public void TurnOff()
    {
        gameObject.SetActive(false);
    }

    public void UpgraeWAlls()
    {
       
        Time.timeScale = 1;
        GameObject[] Walls = GameObject.FindGameObjectsWithTag("Wall");
        foreach (GameObject wall in Walls)
        {
            Debug.Log(wall.GetComponent<Building_Base>().maxHealth);
            wall.GetComponent<Building_Base>().IncreaseHp();
            Debug.Log(wall.GetComponent<Building_Base>().maxHealth);
        }
        GameController.instance.WallHealthInscrease();
        TurnOff();
    }

    public void addRecourse()
    {
        Time.timeScale = 1;
        GameController.instance.resources =+ 10;
        TurnOff();
    }
    public void upgradeMovement()
    {
        Time.timeScale = 1;
        WorkerMovement.IncreaseMS();
        TurnOff();
    }

    public void UpgradeBase()
    {
        Time.timeScale = 1;
        building_base.TestIncreaseHp();
        TurnOff();
    }
}
