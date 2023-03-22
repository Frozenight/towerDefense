using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class GameController : MonoBehaviour
{
    [Header("File Storage Config")]
    [SerializeField] private string fileName;
    public static GameController instance { get; private set; }
    public List<TrashObject> trashObjects;
    public GameOverScreen gameOverScreen;
    public int resources = 0;
    public int trashResourceGainAmount;

    private GameData gameData;
    private List<IGameController> gameControllerObjects;
    private FileDataHandler dataHandler;

    public  Building_Base building_base;
    public Building_Base building_base2;

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
        gameOverScreen.Setup();
        building_base.TestIncreaseHp();
        building_base2.TestIncreaseHp();
    }

    public void AddCountRecource(string name)
    {
        if(name == "trash")
        {
            resources += trashResourceGainAmount;
        }
    }

    private void Start()
    {
        this.dataHandler = new FileDataHandler(Application.persistentDataPath, fileName);
        this.gameControllerObjects = FindAllGameControllerObjects();
        LoadGame();
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

        if(this.gameData == null)
        {
            Debug.Log("No data was found. Initializing data to defaults");
            NewGame();
        }

        foreach(IGameController gameControllerObj in gameControllerObjects)
        {
            gameControllerObj.LoadData(gameData);
        }
    }

    //Saves game
    public void SaveGame()
    {
        foreach(IGameController gameControllerObj in gameControllerObjects)
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
}
