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
    protected override void Update()
    {
        if (target == null)
        {
            IsShooting = false;
            return;
        }
        IsShooting = true;
        //Vector3 dir = target.position - transform.position;
        //Quaternion lookRotation = Quaternion.LookRotation(dir);
        //Vector3 rotation = lookRotation.eulerAngles;
        transform.LookAt(target);
        //transform. = Quaternion.Euler(-90f, rotation.y, 180f);


        if (fireCountdown <= 0f)
        {
            Fire();
            fireCountdown = 1f / (fireRate * FireRateSlow);
        }
        fireCountdown -= Time.deltaTime;
    }

    protected override void Fire()
    {
        GameObject newBullet = (GameObject)Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        GameObject newSmoke = (GameObject)Instantiate(explosionPrefab, firePoint.position, firePoint.rotation);
        Ammunition bullet = newBullet.GetComponent<Ammunition>();
        if (bullet == null)
        {
            return;
        }
        bullet.Seek(target);
        if (bullet.HitTarget() == true)
        {
            nearestEnemyHealth.GetHit(damage * damageMultiplier);
        }
    }
}
