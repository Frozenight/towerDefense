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

    public override void ChangeTypeFire()
    {
        gameObject.SetActive(false);
        Fire_Turret.GetComponent<Building_Base>().tile = gameObject.GetComponent<Building_Base>().tile;
        Fire_Turret.GetComponent<Fire_Turret>().m_level = level;
        Fire_Turret.GetComponent<Fire_Turret>().m_upgrade_price = m_upgrade_price; 
        Fire_Turret.GetComponent<Fire_Turret>().price = upgrade_Price;
        Fire_Turret.transform.parent = gameObject.transform.parent;
        Fire_Turret.transform.position = gameObject.transform.position;
        Fire_Turret.transform.rotation = gameObject.transform.rotation;
        GameObject t = Instantiate(Fire_Turret, Fire_Turret.transform.parent);
        SetNewTurretsTile(t);
        DestroyImmediate(gameObject);
    }

    public override void ChangeTypeFrost()
    {
        gameObject.SetActive(false);
        Frost_Turret.GetComponent<Building_Base>().tile = gameObject.GetComponent<Building_Base>().tile;
        Frost_Turret.GetComponent<Frost_Turret>().m_level = level;
        Frost_Turret.GetComponent<Frost_Turret>().m_upgrade_price = m_upgrade_price;
        Frost_Turret.GetComponent<Frost_Turret>().price = price;
        Frost_Turret.transform.parent = gameObject.transform.parent;
        Frost_Turret.transform.position = gameObject.transform.position;
        Frost_Turret.transform.rotation = gameObject.transform.rotation;
        GameObject t = Instantiate(Frost_Turret, Frost_Turret.transform.parent);   
        SetNewTurretsTile(t);
        DestroyImmediate(gameObject);
    }

    public override void ChangeTypeEarth()
    {
        gameObject.SetActive(false);
        Earth_Turret.GetComponent<Building_Base>().tile = gameObject.GetComponent<Building_Base>().tile;
        Earth_Turret.GetComponent<Earth_Turret>().m_level = level;
        Earth_Turret.GetComponent<Earth_Turret>().m_upgrade_price = m_upgrade_price;
        Earth_Turret.GetComponent<Earth_Turret>().price = price;
        Earth_Turret.transform.parent = gameObject.transform.parent;
        Earth_Turret.transform.position = gameObject.transform.position;
        Earth_Turret.transform.rotation = gameObject.transform.rotation;
        GameObject t = Instantiate(Earth_Turret, Earth_Turret.transform.parent);
        SetNewTurretsTile(t);
        DestroyImmediate(gameObject);
    }

    protected override void setBeginingPrice()
    {
        m_upgrade_price = 5 + (m_level * 2);
    }
    private void SetNewTurretsTile(GameObject turret) {
        GameObject tile = gameObject.GetComponent<Building_Base>().tile;
        turret.GetComponent<Building_Base>().tile = tile;
    }
}
