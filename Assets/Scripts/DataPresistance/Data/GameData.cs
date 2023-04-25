using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public int maxBaseHealth;
    public int towerHealth;
    public int towerDamage1;
    public int wallHealth;
    public float workerSpeed;

    public GameData()
    {
        this.maxBaseHealth = 1000;
        this.towerHealth = 200;
        this.wallHealth = 200;
        this.towerDamage1 = 5;
        this.workerSpeed = 3.5f;
    }
}
