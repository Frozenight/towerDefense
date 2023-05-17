using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Frost_Turret : Turret
{
    public new const int m_typeIndex =4;
    
    EnemyManager nearestEnemyManager;
    BossManager nearestBossManager;
    public float reduce_Speed;
    public float reduce_Time;
    
    public override BuildingData GetExportedData() {
        return new BuildingData(
            GameController.instance.GetTileIndex(
                gameObject.GetComponent<Building_Base>().tile),
            m_level,
            buildingPrice,
            m_typeIndex
        );
    }

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
    protected override void Bullet_Effect()
    {
        if (nearestEnemyManager != null)
            nearestEnemyManager.ReduceSpeed(reduce_Speed, reduce_Time);
        else
            nearestBossManager.ReduceSpeed(reduce_Speed, reduce_Time);

;   }
}
