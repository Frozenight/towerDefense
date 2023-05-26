using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectControler : MonoBehaviour
{
    private EnemyHealth enemyHealth;
    private EnemyManager enemyManager;
    private BossManager bossManager;
    [SerializeField]
    private GameObject fireEffect;
    [SerializeField]
    private GameObject iceEffect;
    private bool isBoss;
    private void Start()
    { 
        fireEffect.SetActive(false);
        iceEffect.SetActive(false);

        enemyHealth = GetComponent<EnemyHealth>();
        if (GetComponent<BossManager>() != null)
        {
            bossManager = GetComponent<BossManager>();
            isBoss = true;
        }
        else
        {
            enemyManager = GetComponent<EnemyManager>();
            isBoss = false;
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (enemyHealth.burning == true)
        {
            fireEffect.SetActive(true);
        }
        else
        {
            fireEffect.SetActive(false);
        }

        SlowCheck(isBoss);
      
    }

    private void SlowCheck(bool isBoss)
    {
        switch (isBoss)
        {
            case true:
                if (bossManager.slowed == true)
                {
                    iceEffect.SetActive(true);
                }
                else
                {
                    iceEffect.SetActive(false);
                }
                break;

            case false:
        if (enemyManager.slowed == true)
        {
            iceEffect.SetActive(true);
        }
        else
        {
            iceEffect.SetActive(false);
        }
                break;
            default:
        }
    }
}
