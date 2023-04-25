using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceSpawner: MonoBehaviour
{
    public GameObject objectToSpawn;
    public int spawnNumber;
    private Transform spawner;

    public int maxBagsPerRound = 5;
    [System.NonSerialized]
    public int bagsPerRound = 0;

    [SerializeField]
    private float spawnTimer = 3f;

    [SerializeField] private EventManager eventManager;

    // Start is called before the first frame update
    void Start()
    {
        spawner = GetComponent<Transform>();
        spawnStart();
        //Debug.Log($"{TrashManager.instance.trashObjects.Count}");
        StartCoroutine(SpawnNew());
    }

    public void StartNewSpawn()
    {
        if (this != null)
            if (bagsPerRound <= maxBagsPerRound)
            {
                StartCoroutine(SpawnNew());
            }
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
            trash.transform.parent = this.transform;

            GameController.instance.AddTrash(trash);

            spawnNumber = spawnNumber - 1;
        }
        
    }

    private IEnumerator SpawnNew()
    {

        yield return new WaitForSeconds(spawnTimer);
        if (bagsPerRound < maxBagsPerRound)
        {
            if (GameController.instance.trashObjects.Count == 0)
            {
                spawnNumber = 5;
                bagsPerRound += spawnNumber;
                while (spawnNumber != 0)
                {
                    Vector3 randomPosition = new Vector3(
                    Random.Range(spawner.position.x - 3, spawner.position.x + 3),
                    Random.Range(spawner.position.y, spawner.position.y),
                    Random.Range(spawner.position.z - 3, spawner.position.z + 3)
                );

                    TrashObject trash;

                    trash = Instantiate(objectToSpawn, randomPosition, Quaternion.identity).GetComponent<TrashObject>();
                    trash.transform.parent = this.transform;
                    GameController.instance.AddTrash(trash);

                    spawnNumber = spawnNumber - 1;
                }
            }
            StartCoroutine(SpawnNew());
        }
    }
}
