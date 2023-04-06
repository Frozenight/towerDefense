using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public int maxHealth;
    public int TowerHealth1;
    public int TowerDamage1;
    public int WallHealth;
    public float workerSpeed;

    public GameData()
    {
        this.maxHealth = 1000;
        this.TowerHealth1 = 200;
        this.WallHealth = 200;
        this.TowerDamage1 = 5;
        this.workerSpeed = 3.5f;
    }
}
