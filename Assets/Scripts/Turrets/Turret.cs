using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : ManageableBuilding
{
    public bool IsShooting;
    private const float INCREASE_DAMAGE = 5f;
    private const float INCREASE_FIRERATE_MULT = 1.1f;
    private const float INCREASE_RANGE = 2.5f;

    public Transform target;
    private EnemyHealth nearestEnemyHealth;

    [Header("Attributes")]

    public float range = 15f;

    public float fireRate = 1f;
    private float fireCountdown = 0f;
    private float damage = 10f;

    [Header("Static")]

    public string enemyTag= "Enemy";

    public Transform partToRotate;

    public GameObject bulletPrefab;
    public GameObject explosionPrefab;
    public Transform firePoint;


    public override string buildingName { 
        get { return NAME_TURRET; } 
    }

    public override void UpgradeBuilding() { 
        // Debug.Log("UpgradeBuilding() Turret class, obj " + this.GetHashCode());
        range += INCREASE_RANGE;
        fireRate *= INCREASE_FIRERATE_MULT;
        damage += INCREASE_DAMAGE;
        m_level++;
        if (UpdateObjectModel(out GameObject newModel)) {
            partToRotate = transform.Find(currModelName + "/Armature/main");
            // Debug.Log(currModelName + "/Armature/main");
            // Debug.Log(partToRotate.GetHashCode());
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating ("UpdateTarget", 0f, 0.1f);
    }

    void UpdateTarget()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(enemyTag);
        float shortestDistance = Mathf.Infinity;
        GameObject nearestEnemy = null;
        foreach (GameObject enemy in enemies){
            float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
            if(distanceToEnemy<shortestDistance){
                shortestDistance=distanceToEnemy;
                nearestEnemy=enemy;
            }
        }

        if(nearestEnemy!=null&&shortestDistance<range){
            target=nearestEnemy.transform;
            nearestEnemyHealth = nearestEnemy.GetComponent<EnemyHealth>();
        }
        else{
            target=null;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(target==null){
            IsShooting = false;
            return;
        }
        IsShooting = true;
        Vector3 dir = target.position - transform.position;
        Quaternion lookRotation = Quaternion.LookRotation(dir);
        Vector3 rotation = lookRotation.eulerAngles;
        partToRotate.rotation = Quaternion.Euler(-90f, rotation.y, 180f);

        if(fireCountdown<=0f){
            
            Fire();
            fireCountdown=1f/fireRate;
        }
        fireCountdown-= Time.deltaTime;
    }

    void Fire(){
        GameObject newBullet = (GameObject)Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        GameObject newSmoke = (GameObject)Instantiate(explosionPrefab, firePoint.position, firePoint.rotation);
        Ammunition bullet = newBullet.GetComponent<Ammunition>();
        if(bullet==null){
            return;
        }
        bullet.Seek(target);
        if (bullet.HitTarget() == true)
        {
            nearestEnemyHealth.GetHit(damage);
        }
    }

    void OnDrawGizmosSelected ()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
