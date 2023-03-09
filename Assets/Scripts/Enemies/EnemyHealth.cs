using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    float health;

    //enemy gets hit
    //damage here is the damage dealt by the player
    public void GetHit(float damage)
    {

        health -= damage;
        if (health <= 0)
            Die();
        else
            gameObject.GetComponent<Animator>().Play("getHit");

    }

    void Die()
    {
        gameObject.GetComponent<Animator>().Play("death");
        Destroy(gameObject, 5);
    }
}
