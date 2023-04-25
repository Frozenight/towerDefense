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
    protected override void Fire()
    {
        GameObject newBullet = (GameObject)Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        GameObject newSmoke = (GameObject)Instantiate(explosionPrefab, firePoint.position, firePoint.rotation);
        Ammunition bullet = newBullet.GetComponent<Ammunition>();
        newSmoke.transform.parent = this.transform;
        bullet.transform.parent = this.transform;
        if (bullet == null)
        {
            return;
        }
        bullet.Seek(target);
        if (bullet.HitTarget() == true)
        {
            //nearestEnemyHealth.GetHit(damage);
            Bullet_Effect();
        }
    }
    protected override void Bullet_Effect()
    {
        nearestEnemyHealth.SetFire(fire_damage, burn_time, damage);
    }

}
