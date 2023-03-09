using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class navmeshMovement : MonoBehaviour
{
    
    private Transform target;
    private NavMeshAgent agent;
    public float enemyDistance = 14;

    private void Start()

    {

        target = GameObject.FindWithTag("Base").transform;
        agent = GetComponent<NavMeshAgent>();

    }

    //Call every frame
    void Update()

    {
        //Look at the player
        transform.LookAt(target);
        agent.SetDestination(target.position);

        //if (Vector3.Distance(transform.position, player.position) < 0)

        //{

        //    gameObject.GetComponent<NavMeshAgent>().velocity = Vector3.zero;

        //    gameObject.GetComponent<Animator>().Play("attack");

        //}

    }

}

