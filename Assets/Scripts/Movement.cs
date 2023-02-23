using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public float speed = 1f;
    private bool going = false;
    public GameObject PointA;     //starting point of movement (main building)
    public GameObject PointB;     //end point of movement (resource collection point)

    private float time;
    private float timeDelay;

    // Start is called before the first frame update
    void Start()
    {
        transform.position = PointA.transform.position;
        going = true;
        time = 0f;
        timeDelay = 3f;
    }

    // Update is called once per frame
    void Update()
    {
        if (going)
        {
            var step = speed * Time.deltaTime; // calculate distance to move
            transform.position = Vector3.MoveTowards(transform.position, PointB.transform.position, step);
            if (Vector3.Distance(transform.position, PointB.transform.position) < 0.001f)
            {
                time = time + 1f * Time.deltaTime;

                if (time >= timeDelay)
                {
                    time = 0f;
                    going = false;
                }
            }
        }
        else
        {
            var step = speed * Time.deltaTime; // calculate distance to move
            transform.position = Vector3.MoveTowards(transform.position, PointA.transform.position, step);
            if (Vector3.Distance(transform.position, PointA.transform.position) < 0.001f)
            {
                time = time + 1f * Time.deltaTime;

                if (time >= timeDelay)
                {
                    time = 0f;
                    going = true;
                }
            }
        }
    }

   
}
