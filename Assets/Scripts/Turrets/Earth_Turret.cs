using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Earth_Turret : Turret
{
    public float damageMultiplier = 2;
    public float FireRateSlow = 0.5f;

    public override string buildingName
    {
        get { return NAME_EARTH_TURRET; }
    }
    

    protected override void Bullet_Effect()
    {
        nearestEnemyHealth.GetHit(damage * damageMultiplier);
    }
}
