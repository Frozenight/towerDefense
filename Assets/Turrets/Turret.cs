using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{

    public Transform target;

    [Header("Attributes")]

    public float range = 15f;

    public float fireRate = 1f;
    private float fireCountdown = 0f;

    [Header("Static")]

    public string enemyTag= "Enemy";

    public Transform partToRotate;

    public GameObject bulletPrefab;
    public Transform firePoint;

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating ("UpdateTarget", 0f, 0.5f);
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
        }else{
            target=null;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(target==null){
            return;
        }

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
        Ammunition bullet = newBullet.GetComponent<Ammunition>();
        if(bullet==null){
            return;
        }
        bullet.Seek(target);
    }

    void OnDrawGizmosSelected ()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
