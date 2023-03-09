using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDamage : MonoBehaviour
{
    public int damage;
    public Building_Base MainBase;
    
    // Start is called before the first frame update
    void Start()
    {
        MainBase = GameObject.FindGameObjectWithTag("Base").GetComponent<Building_Base>();
    }

    // Update is called once per frame
    void Update()
    {
         if (gameObject.gameObject.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("attack"))
        {
            MainBase.ModifyHealth(damage);
        }
    }
}
