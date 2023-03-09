using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{
    public float speed = 5f;
    private Transform MainBiulding;
    public float enemyDistance;
    Vector3 toGround;

    // Start is called before the first frame update
    void Start()
{
    MainBiulding = GameObject.FindWithTag("Base").transform;
        toGround.x = transform.position.x;
        toGround.y = 0;
        toGround.z = transform.position.z;
        transform.position = toGround;

    }

// Update is called once per frame
void Update()
{
        if (MainBiulding == null)
        {
            gameObject.GetComponent<Animator>().Play("idle");
        }
        else
        {
            transform.LookAt(MainBiulding);
            if (Vector3.Distance(transform.position, MainBiulding.position) > enemyDistance)
            {
                transform.position = Vector3.MoveTowards(transform.position, MainBiulding.position, speed * Time.deltaTime);
            }
            else
            {
                GetComponent<Rigidbody>().velocity = Vector3.zero;
                gameObject.GetComponent<Animator>().Play("attack");
            }
        }
        


    }

}
