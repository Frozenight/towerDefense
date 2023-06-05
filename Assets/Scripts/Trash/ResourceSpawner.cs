using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    [SerializeField] float offsetX = 3f;
    [SerializeField] float offsetZ = 3f;

    [SerializeField] private EventManager eventManager;

    [SerializeField] private GameObject[] trashbag_images;
    [SerializeField] private PickUpAnimated worker;
    [SerializeField] private TrashUI trashUI;

    // Start is called before the first frame update
    void Start()
    {
        spawner = GetComponent<Transform>();
        StartNewSpawn();
        //Debug.Log($"{TrashManager.instance.trashObjects.Count}");
    }

    public void StartNewSpawn()
    {
        if (this != null)
            if (bagsPerRound <= maxBagsPerRound)
            {
                StartCoroutine(SpawnNew());
            }
    }

    public bool AllTrashPickedUP()
    {
        for (int i = 0; i < 4; i++)
        {
            if (trashbag_images[i].activeSelf)
            {
                return false;
            }

        }
        return true;
    }


    private IEnumerator SpawnNew()
    {
        yield return new WaitForSeconds(spawnTimer);
        if (bagsPerRound < maxBagsPerRound)
        {
            if (GameController.instance.trashObjects.Count == 0)
            {
                spawnNumber = 5;
                trashUI.SlideIn();
                Debug.Log("Sliding In");
                for (int i = 0; i < spawnNumber; i++)
                {
                    trashbag_images[i].SetActive(true);
                }
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
