using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Movement : MonoBehaviour, IGameController
{
    public List<TrashObject> trashObjects;
    private float speed;
    public bool going = false;
    public GameObject PointB;     //end point of movement (resource collection point)

    private float time;
    private float timeDelay;


    // Start is called before the first frame update
    void Start()
    {
        trashObjects = GameController.instance.trashObjects;
        //transform.position = PointA.transform.position;
        //going = true;
        time = 0f;
        timeDelay = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 targetPos = new Vector3(PointB.transform.position.x, transform.position.y, PointB.transform.position.z);
        if (NearestObject() != null)
        {
            Vector3 newTargetPos = new Vector3(NearestObject().transform.position.x, transform.position.y, NearestObject().transform.position.z);
            if (going)
            {
                var step = speed * Time.deltaTime; // calculate distance to move

                transform.position = Vector3.MoveTowards(transform.position, targetPos, step);

                transform.LookAt(targetPos);

                transform.Rotate(new Vector3(-90, transform.rotation.y, transform.rotation.z));

                if (Vector3.Distance(transform.position, targetPos) < 0.001f)
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
                transform.position = Vector3.MoveTowards(transform.position, newTargetPos, step);

                transform.LookAt(newTargetPos);
                transform.Rotate(new Vector3(-90, transform.rotation.y, transform.rotation.z));

                if (Vector3.Distance(transform.position, newTargetPos) < 0.001f)
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
            transform.position = Vector3.MoveTowards(transform.position, targetPos, step);
            transform.LookAt(targetPos);
            transform.Rotate(new Vector3(-90, transform.rotation.y, transform.rotation.z));

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

    public void LoadData(GameData data)
    {
        this.speed = data.workerSpeed;
    }

    public void SaveData(ref GameData data)
    {
        data.workerSpeed = this.speed;
    }

    public void IncreaseMS()
    {
        speed += 1f;
    }
}
