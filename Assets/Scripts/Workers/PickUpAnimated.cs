using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpAnimated : MonoBehaviour
{
    public bool HasTrash;
    public bool Idle;
    private float time;
    private float timeDelay;
    private float timeDelay2;
    private Collider other;
    // Start is called before the first frame update
    void Start()
    {
        HasTrash = false;
        time = 0f;
        timeDelay = 0.1f;
    }

    // Update is called once per frame
    void Update()
    {
        if (HasTrash && other != null)
        {
            time += Time.deltaTime;
            if (time > timeDelay)
            {
                GameController.instance.trashObjects.Remove(other.gameObject.GetComponent<TrashObject>());
                Destroy(other.gameObject);
                time = 0f;
                other = null;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Trash"))
        {
            this.other = other;
            HasTrash = true;
            GameController.instance.AddCountRecource("trash");
            Debug.Log(GameController.instance.resources);
        }
    }
}
