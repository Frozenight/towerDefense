using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceSpawner: MonoBehaviour
{
    public GameObject objectToSpawn;
    public int spawnNumber;
    private Transform spawner;

    [SerializeField]
    private float spawnTimer = 3f;

    // Start is called before the first frame update
    void Start()
    {
        spawner = GetComponent<Transform>();
        spawnStart();
        //Debug.Log($"{TrashManager.instance.trashObjects.Count}");
        StartCoroutine(SpawnNew());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void spawnStart()
    {
        while (spawnNumber != 0)
        {
            Vector3 randomPosition = new Vector3(
                Random.Range(spawner.position.x - 3, spawner.position.x + 3),
                Random.Range(spawner.position.y, spawner.position.y),
                Random.Range(spawner.position.z - 3, spawner.position.z + 3)
            );

            TrashObject trash;

            trash = Instantiate(objectToSpawn, randomPosition, Quaternion.identity).GetComponent<TrashObject>();

            GameController.instance.AddTrash(trash);

            spawnNumber = spawnNumber - 1;
        }
        
    }

    private IEnumerator SpawnNew()
    {

        yield return new WaitForSeconds(spawnTimer);

        if(GameController.instance.trashObjects.Count == 0)
        {
            spawnNumber = 5;
            while (spawnNumber != 0)
            {
                Vector3 randomPosition = new Vector3(
                Random.Range(spawner.position.x - 3, spawner.position.x + 3),
                Random.Range(spawner.position.y, spawner.position.y),
                Random.Range(spawner.position.z - 3, spawner.position.z + 3)
            );

                TrashObject trash;

                trash = Instantiate(objectToSpawn, randomPosition, Quaternion.identity).GetComponent<TrashObject>();

                GameController.instance.AddTrash(trash);

                spawnNumber = spawnNumber - 1;
            } 
        }
        StartCoroutine(SpawnNew());
    }
}
