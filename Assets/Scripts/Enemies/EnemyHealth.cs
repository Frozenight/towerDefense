using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public float health;
    public void GetHit(float damage)
    {
        health -= damage;;
    }
    public float getHealth()
    {
        return health;
    }
}
