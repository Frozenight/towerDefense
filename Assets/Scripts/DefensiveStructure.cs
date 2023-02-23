using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefensiveStructure : MonoBehaviour
{
    [SerializeField] private float Health;
    [SerializeField] private float Damage;
    [SerializeField] private float FireRate;

    public void TakeDamage(float damage)
    {
        Health -= damage;
    }
}
