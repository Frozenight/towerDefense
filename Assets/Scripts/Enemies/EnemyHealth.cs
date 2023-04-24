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
            health -= Fire_Damage * Time.deltaTime * 0.1f;
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        burning = false;

    }
    public void SetFire(float fireDamage, float burnTime)
    {

        Fire_Damage = fireDamage;
        Burn_Time = burnTime;
        burning = true;
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
