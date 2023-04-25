using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public float health;
    private bool burning = false;
    private float Fire_Damage;
    private float Burn_Time;
    private float Turret_Damage;

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
            health -= ((Fire_Damage+Turret_Damage) * 0.1f) * Time.deltaTime * 0.1f;
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        burning = false;

    }
    public void SetFire(float fireDamage, float burnTime, float turretDamage)
    {

        Fire_Damage = fireDamage;
        Burn_Time = burnTime;
        Turret_Damage = turretDamage;
        burning = true;
    }
    public void GetHit(float damage)
    {
        health -= damage;
    }
    public float getHealth()
    {
        return health;
    }
}
