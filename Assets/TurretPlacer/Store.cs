using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Store : MonoBehaviour
{

    BuildingManager buildingManager;
    // Start is called before the first frame update
    void Start()
    {
        buildingManager=BuildingManager.instance;
        
    }

    public void BuyTurretOne(){
        buildingManager.SelectTurret(buildingManager.standardTurretPrefab);
    }

    public void BuyTurretTwo(){
        buildingManager.SelectTurret(buildingManager.turretTwoPrefab);
    }
}
