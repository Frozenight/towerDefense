using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public List<TrashObject> trashObjects;
    public float speed = 1f;
    public bool going = false;
    public GameObject PointB;     //end point of movement (resource collection point)

    private float time;
    private float timeDelay;

    // Start is called before the first frame update
    void Start()
    {
        trashObjects = TrashManager.instance.trashObjects;
        //transform.position = PointA.transform.position;
        //going = true;
        time = 0f;
        timeDelay = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        if(NearestObject() != null)
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
                transform.position = Vector3.MoveTowards(transform.position, NearestObject().transform.position, step);
                if (Vector3.Distance(transform.position, NearestObject().transform.position) < 0.001f)
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
        else
        {
            var step = speed * Time.deltaTime; // calculate distance to move
            transform.position = Vector3.MoveTowards(transform.position, PointB.transform.position, step);
        }
    }

    public TrashObject NearestObject()
    {
        var nearestDist = float.MaxValue;
        TrashObject nearestObj = null;

        foreach (var trash in trashObjects)
        {
            var dist = Vector3.Distance(gameObject.transform.position, trash.transform.position);
            if (dist < nearestDist)
            {
                nearestDist = dist;
                nearestObj = trash;
            }
        }
        //Debug.Log(nearestObj);
        //Debug.DrawLine(gameObject.transform.position, nearestObj.transform.position, Color.red, duration: 2);

        return nearestObj;
    }


}
