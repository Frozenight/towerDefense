using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class MovementAnimated : MonoBehaviour, IGameController
{
    public GameObject FloatingTextPrefab;
    private const float SPEED_INC_ON_UPGRADE = 5f;

    public static bool ongoingRaid {get; private set;}

    public List<TrashObject> trashObjects;
    public float speed = 10f;
    private float savedSpeed;
    private float savedSpeedAfterRaid;
    public bool goingBackward = false;
    public GameObject PointB;     //end point of movement (resource collection point)
    private float time;
    private float timeDelay;
    private PickUpAnimated PickUp;
    private bool detectedRaidStart = true;
    [SerializeField] private TrashUI trashUI;

    private Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        EventManager.changeWorkerState = ChangeRaidState;
        trashObjects = GameController.instance.trashObjects;
        time = 0f;
        timeDelay = 0.7f;
        PickUp = GetComponent<PickUpAnimated>();
    }

    // Update is called once per frame
    void Update()
    {
        if (NearestObject() != null || PickUp.HasTrash)
        {
            if (!detectedRaidStart) {
                goingBackward = true;
                detectedRaidStart = true;
            }

            if (goingBackward)
            {
                transform.LookAt(PointB.transform.position);
                var step = speed * Time.deltaTime; // calculate distance to move
                transform.position = Vector3.MoveTowards(transform.position, PointB.transform.position, step);
                
                if (Vector3.Distance(transform.position, PointB.transform.position) < 0.001f)
                {
                    if (PickUp.HasTrash) {
                        PickUp.HasTrash = false;                                       
                        GameController.instance.AddCountResource("trash");
                        if (FloatingTextPrefab)
                        {
                            ShowFloatingText("+" + GameController.instance.currResourceGain);
                        }
                    }
                    goingBackward = false;                    
                    animator.SetTrigger("GoForward");
                    // Stops worker at base until ongoingRaid is set to false 
                    if (ongoingRaid) {
                        savedSpeed = speed;
                        speed = 0f;
                    }
                }
            }
            if (!goingBackward && !PickUp.HasTrash)
            {
                try
                {
                    transform.LookAt(NearestObject().transform.position);
                    var step = speed * Time.deltaTime; // calculate distance to move
                    transform.position = Vector3.MoveTowards(transform.position, NearestObject().transform.position, step);
                }
                catch { }
            }
            else if (!goingBackward && PickUp.HasTrash)
            {
                time += Time.deltaTime;
                if (time > timeDelay)
                {
                    animator.SetTrigger("GoBack");
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
        savedSpeed = speed;
    }

    public void SaveData(ref GameData data)
    {
        data.workerSpeed = this.savedSpeed;
    }

    public void IncreaseMS()
    {
        savedSpeed += 1f;
    }

    public void Upgrade()
    {
        savedSpeed += SPEED_INC_ON_UPGRADE;
        if (speed != 0)
            speed = savedSpeed;
    }

    public float GetSavedSpeed { 
        get {
            return savedSpeed; 
        }
    }

    public void SetSpeed(float _sp)
    {
        // no valid speed, set default value
        if (_sp == 0) {
            savedSpeed = speed;
        } else {
            savedSpeed = _sp;
            // shouldn't be necessary, but the check is needed
            // if there is an ongoing raid
            if (speed != 0)
                speed = savedSpeed;
        }
    }

    private void ChangeRaidState() {
        ongoingRaid = !ongoingRaid;
        if (!ongoingRaid) {
            speed = savedSpeed;
            if (!PickUp.HasTrash)
                goingBackward = false;
        } else {
            detectedRaidStart = false;
        }
    }

    private void ShowFloatingText(string text)
    {
        trashUI.SlideOut();
        GameController.instance.RemoveTrashBagImage();
        Vector3 position = new Vector3(transform.position.x, transform.position.y + 7, transform.position.z) ;
        var textObject = Instantiate(FloatingTextPrefab, position, Quaternion.identity);
        textObject.transform.LookAt(Camera.main.transform);
        textObject.transform.Rotate(0, 180, 0);
        textObject.GetComponent<TextMeshPro>().text = text;
    }
}
