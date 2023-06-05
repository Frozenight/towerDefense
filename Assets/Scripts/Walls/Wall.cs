using UnityEngine;

public class Wall : ManageableBuilding
{
    public new const int m_typeIndex =1;

    private enemySpawner enemyController;
    public int price;
    
    public override BuildingData GetExportedData() {
        var bbase = gameObject.GetComponent<Building_Base>();
        return new BuildingData(
            GameController.instance.GetTileIndex(
                gameObject.GetComponent<Building_Base>().tile),
            m_level,
            buildingPrice,
            m_typeIndex,
            bbase.currentHealth,
            bbase.maxHealth
        );
    }

    private void Start()
    {
        if (transform.rotation.eulerAngles.y == 180)
        {
            transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, 0, transform.rotation.eulerAngles.z);
        }

        enemyController = enemySpawner.instance;
        m_upgrade_price = price + (m_level * 5);
    }
    public override string buildingName
    {
        get { return NAME_WALL; }
    }

    public override void UpgradeBuilding()
    {
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
        enemyController?.OnBuildingDestroyed();
    }
}
