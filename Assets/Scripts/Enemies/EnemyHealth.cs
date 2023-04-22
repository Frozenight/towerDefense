using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public float health;
    private bool burning = false;
    private float Fire_Damage;
    private float Burn_Time;

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
        if (GetComponent<BossManager>() != null) { Fire_Damage *= 1.5f; }
            
        Burn_Time = burnTime;
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
