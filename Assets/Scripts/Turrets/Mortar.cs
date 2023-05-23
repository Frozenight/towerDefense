using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Mortar: Turret
{
    private GameObject _target;


    private const float INCREASE_DAMAGE = 5f;
    private const float INCREASE_FIRERATE_MULT = 1.1f;
    private const float INCREASE_RANGE = 2.5f;


    public Transform launchPoint;

    public Target targetScript;

    private float bulletSpeed = 8f;
    private Vector3 hitPoint;


    public override string buildingName { 
        get { return NAME_MORTAR; } 
    }

    public override int buildingPrice
    {
        get { return price; }
    }


    public override void UpgradeBuilding() {
              
        // Debug.Log("UpgradeBuilding() Turret class, obj " + this.GetHashCode());
        if (GameController.instance.resources < m_upgrade_price)
            return;
        
        
        fireRate *= INCREASE_FIRERATE_MULT;
        damage += INCREASE_DAMAGE;
        GameController.instance.resources -= m_upgrade_price;
        m_level += 1;
        price = m_upgrade_price;
        m_upgrade_price = price + (m_level * 2);
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
            _target = nearestEnemy;
            nearestEnemyHealth = nearestEnemy.GetComponent<EnemyHealth>();
            targetScript = _target.GetComponent<Target>();
        }
        else
        {

            _target = null;
        }
    }

    // Update is called once per frame
    protected override void Update()
    {
        if (_target == null)
        {
            IsShooting = false;
            return;
        }
        IsShooting = true;
        Vector3 dir = _target.transform.position - transform.position;
        Quaternion lookRotation = Quaternion.LookRotation(dir);
        Vector3 rotation = lookRotation.eulerAngles;
        partToRotate.rotation = Quaternion.Euler(rotateX, rotation.y, rotation.z);

        if (fireCountdown <= 0f)
        {
            Fire();
            fireCountdown = 1f / fireRate;
        }
        fireCountdown -= Time.deltaTime;
    }



    protected override void Fire()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        if (fireSound != null)
        {
            audioSource.clip = fireSound;
            audioSource.Play();
        }
        GetComponent<Animator>().SetTrigger("Shoot");
        GameObject newBullet = (GameObject)Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        GameObject newSmoke = (GameObject)Instantiate(explosionPrefab, firePoint.position, firePoint.rotation);
        Projectile bullet = newBullet.GetComponent<Projectile>();
        bullet.SetDamage(damage);
        bullet.transform.position = launchPoint.position;
        float time = 0;
        hitPoint = GetHitPoint(targetScript.enemyAimPos, targetScript.lastSpeed, transform.position, bulletSpeed, out time);
        Vector3 aim = hitPoint - transform.position;
        aim.y = 0;

        //

        float antiGravity = -Physics.gravity.y * time / 2;
        float deltaY = (hitPoint.y - bullet.transform.position.y) / time;

        Vector3 arrowSpeed = aim.normalized * bulletSpeed;
        arrowSpeed.y = antiGravity + deltaY;


        partToRotate.LookAt(hitPoint);
        bullet.Go(arrowSpeed, hitPoint);
    }

    Vector3 GetHitPoint(Vector3 targetPosition, Vector3 targetSpeed, Vector3 attackerPosition, float bulletSpeed, out float time)
    {
        Vector3 q = targetPosition - attackerPosition;
        //Ignoring Y for now. Add gravity compensation later, for more simple formula and clean game design around it
        q.y = 0;
        targetSpeed.y = 0;
        //Debug.Log("Tpos = " + targetPosition);
        //solving quadratic ecuation from t*t(Vx*Vx + Vy*Vy - S*S) + 2*t*(Vx*Qx)(Vy*Qy) + Qx*Qx + Qy*Qy = 0

        float a = Vector3.Dot(targetSpeed, targetSpeed) - (bulletSpeed * bulletSpeed); //Dot is basicly (targetSpeed.x * targetSpeed.x) + (targetSpeed.y * targetSpeed.y)
        float b = 2 * Vector3.Dot(targetSpeed, q); //Dot is basicly (targetSpeed.x * q.x) + (targetSpeed.y * q.y)
        float c = Vector3.Dot(q, q); //Dot is basicly (q.x * q.x) + (q.y * q.y)

        //Discriminant
        float D = Mathf.Sqrt((b * b) - 4 * a * c);
        float t1 = (-b + D) / (2 * a);
        float t2 = (-b - D) / (2 * a);


        time = Mathf.Max(t1, t2);
        if (time > 20)
            time = MathF.Min(t1, t2);

        Vector3 ret = targetPosition + targetSpeed * time;
        return ret;
    }


    public override Transform GetTarget()
    {
        return _target.transform;
    }
}
