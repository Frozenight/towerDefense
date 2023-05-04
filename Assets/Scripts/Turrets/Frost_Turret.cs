using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Frost_Turret : Turret
{
    EnemyManager nearestEnemyManager;
    BossManager nearestBossManager;
    public float reduce_Speed;
    public float reduce_Time;

    public override string buildingName
    {
        get { return NAME_FROST_TURRET; }
    }
    protected override void UpdateTarget()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(enemyTag);
        float shortestDistance = Mathf.Infinity;
        GameObject nearestEnemy = null;
        foreach (GameObject enemy in enemies)
        {
            float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
            if (distanceToEnemy < shortestDistance)
            {
                shortestDistance = distanceToEnemy;
                nearestEnemy = enemy;
            }
        }

        if (nearestEnemy != null && shortestDistance < range)
        {
            target = nearestEnemy.transform;
            nearestEnemyHealth = nearestEnemy.GetComponent<EnemyHealth>();
            if(nearestEnemy.GetComponent<EnemyManager>()!=null)
                nearestEnemyManager = nearestEnemy.GetComponent<EnemyManager>();
            else
                nearestBossManager = nearestEnemy.GetComponent<BossManager>();
        }
        else
        {

            target = null;
        }
    }
    protected override void Fire()
    {
        animator.SetTrigger("Shoot");
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
        if (nearestEnemyManager != null)
            nearestEnemyManager.ReduceSpeed(reduce_Speed, reduce_Time);
        else
            nearestBossManager.ReduceSpeed(reduce_Speed, reduce_Time);

;   }
}
