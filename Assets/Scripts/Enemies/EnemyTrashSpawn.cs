using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyTrashSpawn : MonoBehaviour
{
    [SerializeField] private GameObject objectToSpawn;
    [SerializeField] private string tag;

    //private void OnDestroy()
    //{
    //     SpawnTrash();
    //}

    public void SpawnTrash()
    {
        Vector3 spawnLoc = gameObject.transform.position;
        TrashObject trash;
        trash = Instantiate(objectToSpawn, spawnLoc, Quaternion.identity).GetComponent<TrashObject>();
        trash.gameObject.tag = tag;
        GameController.instance.AddTrash(trash);
    }
}
