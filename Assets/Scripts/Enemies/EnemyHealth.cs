using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    private float maxHealth;
    public float health;
    public bool burning = false;
    private float Fire_Damage;
    private float Burn_Time;
    private float Turret_Damage;
    float elapsedTime = 0f;
    public event Action<float> OnHealthChanged = delegate { };
    bool hit;

    private void Start()
    {
        maxHealth = health;
    }
    void Update()
    {
        if (burning == true)
        {
            Burn();
            //StartCoroutine(Burn());
        }
    }
    //IEnumerator Burn()
    //{
    //    float elapsedTime = 0f;
    //    while (elapsedTime < Burn_Time)
    //    {
    //        GetHit((Fire_Damage + Turret_Damage) * 0.1f * Time.deltaTime * 0.1f);
    //        elapsedTime += Time.deltaTime;
    //        yield return null;
    //    }
    //    burning = false;

    //}
    public void SetFire(float fireDamage, float burnTime, float turretDamage)
    {
        hit = true;
        Fire_Damage = fireDamage;
        if (GetComponent<BossManager>() != null) { Fire_Damage *= 1.5f; }
            
        Burn_Time = burnTime;
        if (burning == false)
        {
            burning = true;
        }
        Turret_Damage = turretDamage;
    }
    public void GetHit(float damage)
    {
        health -= damage;
        float currentHealthPct = (float)health / (float)maxHealth;
        OnHealthChanged(currentHealthPct);
        if (GetComponent<EnemyNavmesh>() != null)
            GetComponent<EnemyNavmesh>().UpdateSpeed();
        else
            GetComponent<BossNavmesh>().UpdateSpeed();
    }
    public float getHealth()
    {
        return health;
    }

    private void Burn()
    {
        if (hit == true)
        {
            elapsedTime = 0;
            hit = false;
        }
        if (elapsedTime < Burn_Time)
        {
            elapsedTime += Time.deltaTime;
            health -= ((Fire_Damage + Turret_Damage) * 0.1f) * Time.deltaTime;
        }
        else
        {
            elapsedTime = 0;
            burning = false;
        }

    }
}
