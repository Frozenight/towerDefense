using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Turret : ManageableBuilding
{
    [SerializeField] GameObject rangeIndicator;
    [SerializeField] float offsetRangeHeight;


    private enemySpawner enemyController;
    protected Animator animator;

    public AudioClip fireSound;
    private AudioSource audioSource;

    public bool IsShooting;
    private const float INCREASE_DAMAGE = 5f;
    private const float INCREASE_FIRERATE_MULT = 1.1f;
    private const float INCREASE_RANGE = 2.5f;

    protected Transform target;
    protected EnemyHealth nearestEnemyHealth;

    [Header("Attributes")]

    public float range = 15f;

    public float fireRate = 1f;
    public float fireCountdown = 0f;
    public float damage = 10f;

    [Header("Static")]

    protected string enemyTag= "Enemy";

    public Transform partToRotate;

    public GameObject bulletPrefab;
    public GameObject explosionPrefab;
    public Transform firePoint;

    public float rotateX;
    public int price;

    

    public override string buildingName { 
        get { return NAME_TURRET; } 
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

        //if (m_level % 5 == 0 && UpdateObjectModel(out GameObject newModel)) {
        //    partToRotate = transform.Find(currModelName + "/Armature/main");
        //    // Debug.Log(currModelName + "/Armature/main");
        //    // Debug.Log(partToRotate.GetHashCode());
        //}
    }

    // Start is called before the first frame update
    void Start()
    {
        UnhighlightBuilding();
        InvokeRepeating("UpdateTarget", 0f, 0.1f);
        enemyController = enemySpawner.instance;
        GetComponent<Building_Base>().maxHealth = GameController.instance.GetTurretHealth();
        GetComponent<Building_Base>()._currentHealth = GameController.instance.GetTurretHealth();
        animator = GetComponent<Animator>();
        m_upgrade_price = 5 + (m_level * 2);
    }

    protected virtual void UpdateTarget()
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
    protected virtual void Update()
    {
        if (target == null)
        {
            IsShooting = false;
            return;
        }
            IsShooting = true;
        Vector3 dir = target.position - transform.position;
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



    protected virtual void Fire(){
        audioSource = gameObject.AddComponent<AudioSource>();
        if (fireSound != null)
        {
            audioSource.clip = fireSound;
            audioSource.Play();
        }
        animator.SetTrigger("Shoot");
        GameObject newBullet = (GameObject)Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        GameObject newSmoke = (GameObject)Instantiate(explosionPrefab, firePoint.position, firePoint.rotation);
        Ammunition bullet = newBullet.GetComponent<Ammunition>();
        newSmoke.transform.parent = this.transform;
        bullet.transform.parent = this.transform;
        bullet.transform.LookAt(target);
        if(bullet==null){
            return;
        }
        bullet.Seek(target);
        if (bullet.HitTarget() == true)
        {
            nearestEnemyHealth.GetHit(damage);
            Bullet_Effect();
        }
    }
    protected virtual void Bullet_Effect()
    {
        return;
    }

    public override void DestroyBuilding()
    {
        int sell_price = buildingPrice;
        int one_level_price = 5;
        for (int i = 1; i < m_level; i++)
        {
            sell_price += one_level_price;
            one_level_price += 5;
        }
        GameController.instance.resources += sell_price / 2;
        Destroy(gameObject);
    }

    public override void HighlightBuilding()
    {
        rangeIndicator.SetActive(true);
        rangeIndicator.transform.localPosition = new Vector3(0, offsetRangeHeight + range * 2f, 0);
    }

    public override void UnhighlightBuilding()
    {
        rangeIndicator.SetActive(false);
    }

    void OnDrawGizmosSelected ()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }

    public Transform GetTarget()
    {
        return target;
    }

    private void OnDestroy()
    {
        enemyController.OnBuildingDestroyed();
    }

    public float GetUpgradeDamage()
    {
        return INCREASE_DAMAGE;
    }
    public float GetUpgradeAttackSpeed()
    {
        return INCREASE_FIRERATE_MULT;
    }
    public float GetUpgradeRange()
    {
        return INCREASE_RANGE;
    }
}
