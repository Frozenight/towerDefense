using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUp : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Trash"))
        {
            GameController.instance.trashObjects.Remove(other.gameObject.GetComponent<TrashObject>());
            Destroy(other.gameObject);
            GetComponent<Movement>().going = true;
            GameController.instance.AddCountResource("trash");
        }

        if (other.gameObject.CompareTag("EnemyTrash"))
        {
            Debug.Log("EnemyTrash");
            GameController.instance.trashObjects.Remove(other.gameObject.GetComponent<TrashObject>());
            Destroy(other.gameObject);
            GetComponent<Movement>().going = true;
            GameController.instance.AddCountResource("enemyTrash"); 
        }
    }
}
