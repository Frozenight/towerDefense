using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    private EnemyManager enemyManager;
    private BossManager bossManager;
    public Transform destination;
    public Vector3 lastSpeed = new Vector3();
    public Transform enemyAim;

    private float speed;
    private Vector3 lastPosition = new Vector3();

    // Start is called before the first frame update
    void Start()
    {
        if(GetComponent<EnemyManager>() != null)
        {
            enemyManager = GetComponent<EnemyManager>();
        }
        else if (GetComponent<BossManager>() != null) 
        {
            bossManager = GetComponent<BossManager>();
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
        speed = (transform.position - lastPosition).magnitude / Time.deltaTime;
        lastPosition = transform.position;

        if (speed <= 0)
            lastSpeed = new Vector3(0,0,0);
        else if(enemyManager != null)
        {
            lastSpeed = enemyAim.TransformDirection(Vector3.forward) * enemyManager.speed;
        }
        else if(bossManager != null)
        {
            lastSpeed = enemyAim.TransformDirection(Vector3.forward) * bossManager.speed;
        }
    }

    public Vector3 enemyAimPos
    {
        get { return enemyAim.position; }
    }
}

//speed = (transform.position - lastPosition).magnitude / Time.deltaTime;
//lastPosition = transform.position;
