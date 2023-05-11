using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectControler : MonoBehaviour
{
    private EnemyHealth enemyHealth;
    private EnemyManager enemyManager;
    [SerializeField]
    private GameObject fireEffect;
    [SerializeField]
    private GameObject iceEffect;
    private void Start()
    { 
        fireEffect.SetActive(false);
        iceEffect.SetActive(false);

        enemyHealth = GetComponent<EnemyHealth>();
        if (GetComponent<BossManager>() != null)
            enemyManager = GetComponent<BossManager>();
        else
            enemyManager = GetComponent<EnemyManager>();
        
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

        if (enemyManager.slowed == true)
        {
            iceEffect.SetActive(true);
        }
        else
        {
            iceEffect.SetActive(false);
        }
    }
}
