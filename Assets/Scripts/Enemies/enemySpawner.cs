using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemySpawner : MonoBehaviour
{
    public static enemySpawner instance { get; private set; }
    public float spawnCoolDown = 1f;
    float spawnCoolDownRemaining = 0;
    float offsetZ = 5;


    [System.Serializable]
    public class WaveComponent {
        public GameObject enemyPrefab;
        public int enemyAmount;
        [System.NonSerialized]
        public int spawned = 0;
    }
    public WaveComponent waveComponents;

    public bool startSpawn {get; set;}
    void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        startSpawn = false;
    }

    // Update is called once per frame

    public void spawnWave()
    {
        for(int i = 0; i < waveComponents.enemyAmount; i++)
        {
            var offset = new Vector3(0, 0, Random.Range(-offsetZ, offsetZ));
            Instantiate(waveComponents.enemyPrefab, transform.position + offset, Quaternion.identity);
            
        }
        waveComponents.enemyAmount++;
    }

    public void OnBuildingDestroyed()
    {
        // Call a method on the rest of the enemies
        EnemyNavmesh[] enemies = FindObjectsOfType<EnemyNavmesh>();
        foreach (EnemyNavmesh e in enemies)
        {
            e.OnBuildingDestroyed();
        }
        EnemyManager[] enemies1 = FindObjectsOfType<EnemyManager>();
        foreach (var e in enemies1)
        {
            e.ResetObjective();
        }
    }
}
