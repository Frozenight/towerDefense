using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementAnimated : MonoBehaviour, IGameController
{

    private const float SPEED_INC_ON_UPGRADE = 3f;

    public List<TrashObject> trashObjects;
    public float speed = 1f;
    public bool goingBackward = false;
    public GameObject PointB;     //end point of movement (resource collection point)
    private float time;
    private float timeDelay;
    private PickUpAnimated PickUp;

    // Start is called before the first frame update
    void Start()
    {
        trashObjects = GameController.instance.trashObjects;
        time = 0f;
        timeDelay = 0.7f;
        PickUp = GetComponent<PickUpAnimated>();
    }

    // Update is called once per frame
    void Update()
    {
        if (NearestObject() != null)
        {
            if (goingBackward && PickUp.HasTrash)
            {
                transform.LookAt(PointB.transform.position);
                var step = speed * Time.deltaTime; // calculate distance to move
                transform.position = Vector3.MoveTowards(transform.position, PointB.transform.position, step);
                
                if (Vector3.Distance(transform.position, PointB.transform.position) < 0.001f)
                {
                    PickUp.HasTrash = false;
                    goingBackward = false;
                }
            }
            if (!goingBackward && !PickUp.HasTrash)
            {
                transform.LookAt(NearestObject().transform.position);
                var step = speed * Time.deltaTime; // calculate distance to move
                transform.position = Vector3.MoveTowards(transform.position, NearestObject().transform.position, step);
            }
            else if (!goingBackward && PickUp.HasTrash)
            {
                time += Time.deltaTime;
                if (time > timeDelay)
                { 
                    goingBackward = true;
                    time = 0f;
                }
            }
        }
        else
        {
            transform.LookAt(PointB.transform.position);
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

    public void Upgrade()
    {
        speed += SPEED_INC_ON_UPGRADE;
    }
}
