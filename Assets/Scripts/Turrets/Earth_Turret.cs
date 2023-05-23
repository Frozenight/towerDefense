using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Earth_Turret : Turret
{
    public new const int m_typeIndex =6;
    public float damageMultiplier = 2;
    public float FireRateSlow = 0.5f;

    public override string buildingName
    {
        get { return NAME_EARTH_TURRET; }
    }    

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

    protected override void Bullet_Effect()
    {
        nearestEnemyHealth.GetHit(damage * damageMultiplier);
    }
}
