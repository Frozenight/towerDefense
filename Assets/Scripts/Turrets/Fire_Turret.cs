using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire_Turret : Turret
{
    public new const int m_typeIndex =5;
    
    public float fire_damage;
    public float burn_time;

    public override string buildingName
    {
        get { return NAME_FIRE_TURRET; }
    }

    public override BuildingData GetExportedData() {
        return new BuildingData(
            GameController.instance.GetTileIndex(
                gameObject.GetComponent<Building_Base>().tile),
            m_level,
            buildingPrice,
            m_typeIndex
        );
    }

    protected override void Bullet_Effect()
    {
        nearestEnemyHealth.SetFire(fire_damage, burn_time, damage);
    }

}
