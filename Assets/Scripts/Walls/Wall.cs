using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : ManageableBuilding
{
    private enemySpawner enemyController;
    private void Awake()
    {
        enemyController = enemySpawner.instance;
        GetComponent<Building_Base>().maxHealth = GameController.instance.GetWallHealth();
    }
    public override string buildingName
    {
        get { return NAME_WALL; }
    }

    public override bool canDestroyManually
    {
        get { return true; }
    }
    private void OnDestroy()
    {
        enemyController.OnBuildingDestroyed();
    }
}
