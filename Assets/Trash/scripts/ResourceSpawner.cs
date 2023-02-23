using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceSpawner: MonoBehaviour
{
    public GameObject objectToSpawn;
    public Vector3 minPosition;
    public Vector3 maxPosition;
    public int spawnNumber;

    // Start is called before the first frame update
    void Start()
    {
        spawnStart();
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
                Random.Range(minPosition.x, maxPosition.x),
                Random.Range(minPosition.y, maxPosition.y),
                Random.Range(minPosition.z, maxPosition.z)
            );
            Instantiate(objectToSpawn, randomPosition, Quaternion.identity);
            spawnNumber = spawnNumber - 1;
        }
        
    }
}
