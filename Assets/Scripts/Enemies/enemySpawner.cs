using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemySpawner : MonoBehaviour
{
    public static enemySpawner instance { get; private set; }
    public float spawnCoolDown = 1f;
    float offsetZ = 5;

    int waveCnt = 0;
    public int bossWave;

    private bool isInPlayMode = false;

    [SerializeField] private int scalingHealth  = 40;

    public int completedWaves {
        get {
            return waveCnt;
        }
    }


    [System.Serializable]
    public class WaveComponent {
        public GameObject enemyPrefab1;
        public GameObject enemyPrefab2;
        public GameObject enemyPrefab3;
        public GameObject BossPrefab;
        public int enemyAmount;
        [System.NonSerialized]
        public int spawned = 0;
    }
    public WaveComponent waveComponents;
    private GameObject[] enemies = new GameObject[3];
   

    public bool startSpawn {get; set;}
    void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        startSpawn = false;
        isInPlayMode = true;
        enemies[0] = waveComponents.enemyPrefab1;
        enemies[1] = waveComponents.enemyPrefab2;
        enemies[2] = waveComponents.enemyPrefab3;
    }

    public void ImportSessionData(int waveCount) {
        waveCnt = waveCount;
        for (int i = 0; i < waveCnt; i++) {
            scalingHealth = scalingHealth+(waveComponents.enemyAmount-1)*6;
            waveComponents.enemyAmount++;
        }
    }


    // Update is called once per frame

    public void spawnWave()
    {
        waveCnt++;
        
        if (!isInPlayMode)
            return;

        //5 15 25 35...
        if ((waveCnt - 5) % 10 == 0)
        { 
            var offset = new Vector3(0, 0, Random.Range(-offsetZ, offsetZ));
            Instantiate(waveComponents.BossPrefab, transform.position + offset, Quaternion.Euler(0, 180, 0)).transform.parent = this.transform;
            waveComponents.BossPrefab.GetComponent<EnemyHealth>().health += waveComponents.BossPrefab.GetComponent<EnemyHealth>().health;
        }
        else
            for (int i = 0; i < waveComponents.enemyAmount; i++)
            {
                var offset = new Vector3(0, 0, Random.Range(-offsetZ, offsetZ));
                var enemy = Instantiate(enemies[Random.Range(0, 3)], transform.position + offset, Quaternion.Euler(0, 180, 0));
                enemy.GetComponent<EnemyHealth>().health = scalingHealth;
            }
        scalingHealth = scalingHealth+(waveComponents.enemyAmount-1)*6;
        if(waveComponents.enemyAmount < 20)
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

        BossNavmesh[] bosses = FindObjectsOfType<BossNavmesh>();
        foreach (BossNavmesh b in bosses)
        {
            b.OnBuildingDestroyed();
        }
        BossManager[] bosses1 = FindObjectsOfType<BossManager>();
        foreach (var b in bosses1)
        {
            b.ResetObjective();
        }
    }

    private void OnDestroy()
    {
        isInPlayMode = false;
    }
}
