using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Frost_Turret : Turret
{
    EnemyManager nearestEnemyManager;
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
            nearestEnemyManager = nearestEnemy.GetComponent<EnemyManager>();
        }
        else
        {

            target = null;
        }
    }
    protected override void Bullet_Effect()
    {
            nearestEnemyManager.ReduceSpeed(reduce_Speed, reduce_Time);
;   }
}
