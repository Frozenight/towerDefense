using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemySpawner : MonoBehaviour
{
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
    public WaveComponent[] waveComponents;

    bool startSpawn = false;
    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetButtonDown("Fire2"))
        {
            startSpawn = true;
        }
        spawnWave();
    }
    void spawnWave()
    {
        bool didSpawn = false;
        spawnCoolDownRemaining -= Time.deltaTime;
        if (spawnCoolDownRemaining < 0 && startSpawn == true)
        {
            spawnCoolDownRemaining = spawnCoolDown;

            foreach (WaveComponent wc in waveComponents)
            {
                if (wc.spawned < wc.enemyAmount)
                {
                    wc.spawned++;
                    var offset = new Vector3(0, 0, Random.Range(-offsetZ, offsetZ));
                    Instantiate(wc.enemyPrefab, transform.position + offset, Quaternion.identity);
                    didSpawn = true;
                    break;
                }
            }

            if (didSpawn == false)
            {

                Destroy(gameObject);

            }
        }
    }
}
