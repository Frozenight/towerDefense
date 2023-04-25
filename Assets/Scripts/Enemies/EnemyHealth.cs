using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    private float maxHealth;
    public float health;
    private bool burning = false;
    private float Fire_Damage;
    private float Burn_Time;
    private float Turret_Damage;

    public event Action<float> OnHealthChanged = delegate { };

    private void Start()
    {
        maxHealth = health;
    }
    void Update()
    {
        if (burning == true)
        {
            StartCoroutine(Burn());
        }
    }
    IEnumerator Burn()
    {
        float elapsedTime = 0f;
        while (elapsedTime < Burn_Time)
        {
            health -= ((Fire_Damage + Turret_Damage) * 0.1f) * Time.deltaTime * 0.1f;
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        burning = false;

    }
    public void SetFire(float fireDamage, float burnTime, float turretDamage)
    {

        Fire_Damage = fireDamage;
        if (GetComponent<BossManager>() != null) { Fire_Damage *= 1.5f; }
            
        Burn_Time = burnTime;
        burning = true;
        Turret_Damage = turretDamage;
    }
    public void GetHit(float damage)
    {
        health -= damage;
        float currentHealthPct = (float)health / (float)maxHealth;
        OnHealthChanged(currentHealthPct);
    }
    public float getHealth()
    {
        return health;
    }
}
