using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building_Base : MonoBehaviour
{
    public float _health;

    private int _defence;

    public void GetHit(float damage)
    {

        _health -= damage;
        if (_health <= 0)
            Die();
    }
  
       private void Die()
        {
            Destroy(gameObject);
        }
    }
