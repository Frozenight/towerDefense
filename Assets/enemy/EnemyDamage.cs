using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDamage : MonoBehaviour
{
    public float damage;
    public Building_Base MainBase;
    
    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
         if (gameObject.gameObject.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("attack"))
        {
            MainBase.GetHit(damage);
        }
    }
}
