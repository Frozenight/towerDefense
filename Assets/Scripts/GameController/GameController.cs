using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    [Header("File Storage Config")]
    [SerializeField] private string fileName;
    public static GameController instance { get; private set; }
    public List<TrashObject> trashObjects;
    public GameOverPowerUP gameOverScreen;
    public BossAppear bossAppear;
    public int resources = 0;
    public GameObject BuildingSelectUI;
    public Show_BuildingSelection buildingSelection;
    public Image Tower1;
    public Image Wall;
    public Sprite Tower1Color;
    public Sprite WallColor;
    public Sprite Tower1Grey;
    public Sprite WallGrey;
    [SerializeField] private int trashGainSpawned;
    [SerializeField] private int trashGainEnemyDroped;

    public Rounds rounds;
    public OpenAiAPI aiAPI;

    private GameData gameData;
    private List<IGameController> gameControllerObjects;
    private FileDataHandler dataHandler;

    [SerializeField] private GridManager gridManager;
    [SerializeField] private GameObject wall;
    [SerializeField] public GameObject vfx;

    private void Awake()
    {
        instance = this;
    }

    public void AddTrash(TrashObject trash)
    {
        trashObjects.Add(trash);
    }

    public void GameOver()
    {
        rounds.NewGame();
        gameOverScreen.Setup();

    }

    public void BossWarning()
    {
        bossAppear.Setup();
    }
    public void AddCountResource(string name)
    {
        if (name == "trash")
        {
            resources += trashGainSpawned;
        }

        else if (name == "enemyTrash")
        {
            resources += trashGainEnemyDroped;
        }
    }

    private void Start()
    {
        BuildingSelectUI.SetActive(false);
        this.dataHandler = new FileDataHandler(Application.persistentDataPath, fileName);
        this.gameControllerObjects = FindAllGameControllerObjects();
        LoadGame();
        aiAPI.GetData();
        vfx = (GameObject)Instantiate(vfx, new Vector3(0, 0, 0), Quaternion.identity);
    }

    private void Update()
    {
        if(resources < 25)
        {
            Tower1.sprite = Tower1Grey;
        }
        else
        {
            Tower1.sprite = Tower1Color;
        }
        if(resources < 3)
        {
            Wall.sprite = WallGrey;
        }
        else
        {
            Wall.sprite = WallColor;
        }
    }

    public int GetTurretHealth()
    {
        return gameData.towerHealth;
    }

    public int GetWallHealth()
    {
        return gameData.towerHealth;
    }

    private List<IGameController> FindAllGameControllerObjects()
    {
        IEnumerable<IGameController> gameControllerObjects = FindObjectsOfType<MonoBehaviour>().OfType<IGameController>();
        return new List<IGameController>(gameControllerObjects);
    }

    //New game when no data exists
    public void NewGame()
    {
        this.gameData = new GameData();
    }

    //Loads game
    public void LoadGame()
    {
        this.gameData = dataHandler.Load();

        if (this.gameData == null)
        {
            Debug.Log("No data was found. Initializing data to defaults");
            NewGame();
        }

        foreach (IGameController gameControllerObj in gameControllerObjects)
        {
            gameControllerObj.LoadData(gameData);
        }
    }

    //Saves game
    public void SaveGame()
    {
        foreach (IGameController gameControllerObj in gameControllerObjects)
        {
            gameControllerObj.SaveData(ref gameData);
        }

        dataHandler.Save(gameData);
    }

    //Saves game when you close it
    public void OnApplicationQuit()
    {
        SaveGame();
    }

    public void SpawnWalls()
    {
        Quaternion quaternion = Quaternion.Euler(gridManager.tiles[1].gameObject.transform.rotation.x, -90, gridManager.tiles[1].gameObject.transform.rotation.z);
        for (int i = 126; i < 130; i++)
        {
            GameObject startWall = Instantiate(wall, gridManager.tiles[i].gameObject.transform.position, quaternion);
        }
        quaternion = Quaternion.Euler(gridManager.tiles[1].gameObject.transform.rotation.x, 90, gridManager.tiles[1].gameObject.transform.rotation.z);
        for (int i = 100; i < 104; i++)
        {
            GameObject startWall = Instantiate(wall, gridManager.tiles[i].gameObject.transform.position, quaternion);
        }

    }
}
