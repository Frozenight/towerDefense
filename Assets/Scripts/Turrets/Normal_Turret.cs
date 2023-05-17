using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Normal_Turret : Turret
{
    public new const int m_typeIndex =3;
    
    public GameObject Earth_Turret;
    public GameObject Fire_Turret;
    public GameObject Frost_Turret;

    public override BuildingData GetExportedData() {
        return new BuildingData(
            GameController.instance.GetTileIndex(
                gameObject.GetComponent<Building_Base>().tile),
            m_level,
            buildingPrice,
            m_typeIndex
        );
    }

    public override void ChangeTypeFire()
    {
        gameObject.SetActive(false);
        Fire_Turret.transform.parent = gameObject.transform.parent;
        Fire_Turret.transform.position = gameObject.transform.position;
        Fire_Turret.transform.rotation = gameObject.transform.rotation;
        Instantiate(Fire_Turret, Fire_Turret.transform.parent);
        DestroyImmediate(gameObject);
    }

    public override void ChangeTypeFrost()
    {
        gameObject.SetActive(false);
        Frost_Turret.transform.parent = gameObject.transform.parent;
        Frost_Turret.transform.position = gameObject.transform.position;
        Frost_Turret.transform.rotation = gameObject.transform.rotation;
        Instantiate(Frost_Turret, Frost_Turret.transform.parent);   
        DestroyImmediate(gameObject);
    }

    public override void ChangeTypeEarth()
    {
        gameObject.SetActive(false);
        Earth_Turret.transform.parent = gameObject.transform.parent;
        Earth_Turret.transform.position = gameObject.transform.position;
        Earth_Turret.transform.rotation = gameObject.transform.rotation;
        Instantiate(Earth_Turret, Earth_Turret.transform.parent);
        DestroyImmediate(gameObject);
    }
}
