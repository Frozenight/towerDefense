using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire_Turret : Turret
{
    public float fire_damage;
    public float burn_time;

    public override string buildingName
    {
        get { return NAME_FIRE_TURRET; }
    }
    protected override void Bullet_Effect()
    {
        nearestEnemyHealth.SetFire(fire_damage, burn_time);
    }

}
