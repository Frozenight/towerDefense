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
    public GameOverPowerUP gameOverScreen;
    public BossAppear bossAppear;
    public int resources = 0;
    public int trashResourceGainAmount;
    public Rounds rounds;
    public OpenAiAPI aiAPI;

    private GameData gameData;
    private List<IGameController> gameControllerObjects;
    private FileDataHandler dataHandler;

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
        aiAPI.GetData();
    }

    public int GetTurretHealth()
    {
        return gameData.TowerHealth1;
    }

    public int GetWallHealth()
    {
        return gameData.TowerHealth1;
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
