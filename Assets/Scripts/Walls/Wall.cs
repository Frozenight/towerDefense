using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class Wall : ManageableBuilding
{
    private enemySpawner enemyController;
    public int price;
    
    private void Start()
    {
        enemyController = enemySpawner.instance;
        GetComponent<Building_Base>().maxHealth = GameController.instance.GetWallHealth();
        GetComponent<Building_Base>()._currentHealth = GameController.instance.GetWallHealth();
    }
    public override string buildingName
    {
        get { return NAME_WALL; }
    }

    void Start()
    {
        m_upgrade_price = price + (m_level * 5);
    }

    public override void UpgradeBuilding()
    {
        // Debug.Log("UpgradeBuilding() Turret class, obj " + this.GetHashCode());
        if (gameController.resources < m_upgrade_price)
            return;

        gameController.resources -= m_upgrade_price;
        m_level += 1;
        m_upgrade_price = price + (m_level * 5);
        price = m_upgrade_price;
    }

    public override int buildingPrice
    {
        get { return price; }
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
