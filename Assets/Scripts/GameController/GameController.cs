using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using UnityEngine.UI;
using TMPro;

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
    public enemySpawner spawner;

    private GameData gameData;
    private List<IGameController> gameControllerObjects;
    private FileDataHandler dataHandler;

    [SerializeField] private EventManager eventManager;
    [SerializeField] private GridManager gridManager;
    [SerializeField] private GameObject wall;
    [SerializeField] public GameObject vfx;
    [SerializeField] private TextMeshProUGUI bonusText;
    private int resourceBonusFlat = 0;
    private int m_currResourceGain = 0;

    public int currResourceGain {
        get {
            return m_currResourceGain;
        }
    }

    private void Awake()
    {
        instance = this;
    }

    public void AddBonusToGameData() {
        if (instance == this) {
            float randomFloat = UnityEngine.Random.Range(0f, 1f);
            GameBonus bonus = null;
            if (randomFloat > 0.5f) {
                int val = UnityEngine.Random.Range(1, 6);
                float value = (float)val / 100f;
                bonus = new GameBonus(BonusType.BaseToughness, value);
                gameData.bonuses.Add(bonus);   
                Building_Base[] buildings = UnityEngine.Object.FindObjectsOfType<Building_Base>();
                foreach (var building in buildings) {
                    building.SetArmorMultiplier();
                }
            } else {
                float value = (float)UnityEngine.Random.Range(1, 4);
                bonus = new GameBonus(BonusType.ResourceGainFlat, value);
                gameData.bonuses.Add(bonus);  
                SetResourceBonus();
            }
            StartCoroutine(ShowBonusInfo(bonus));
        }
    }

    IEnumerator ShowBonusInfo(GameBonus bonus) {
        bonusText.gameObject.SetActive(true);
        bonusText.text = GameTexts.NewBonus(bonus.Type, bonus.Value);
        yield return new WaitForSeconds(7f);
        bonusText.gameObject.SetActive(false);
    }

    public List<float> GetBonusValues(BonusType bonusType) {
        List<float> vals = new List<float>();
        if (this == instance) {
            foreach (var bonus in gameData.bonuses) {
                if (bonus.Type == bonusType) {
                    vals.Add(bonus.Value);
                }
            }
        }
        return vals;
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
            resources += m_currResourceGain;
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
        Debug.Log("Load");
        LoadGame();
        aiAPI.GetData();
        vfx = (GameObject)Instantiate(vfx, new Vector3(0, 0, 0), Quaternion.identity);
        SetResourceBonus();
        bonusText.gameObject.SetActive(false);
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
        if (PlayerPrefs.HasKey("twr2Unlocked") && PlayerPrefs.GetString("twr2Unlocked") == "false")
        {
            if (rounds.current_round == spawner.bossWave && eventManager.currentState == EventManager.Event.building)
            {
                PlayerPrefs.SetString("twr2Unlocked", "true");
            }
        }
        else if (PlayerPrefs.HasKey("twr2Unlocked") && PlayerPrefs.GetString("twr2Unlocked") == "true")
        {
            BuildingSelectUI.GetComponent<Show_BuildingSelection>().showTwr2();
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
        Quaternion quaternion = Quaternion.Euler(gridManager.tiles[1].gameObject.transform.rotation.x, 90, gridManager.tiles[1].gameObject.transform.rotation.z);
        for (int i = 123; i < 127; i++)
        {
            GameObject startWall = Instantiate(wall, gridManager.tiles[i].gameObject.transform.position, quaternion);
            startWall.GetComponent<Building_Base>().tile = gridManager.tiles[i];
            startWall.gameObject.GetComponent<Building_Base>().tile.GetComponent<TileOnWhichToPlace>().placed = true;
        }
        quaternion = Quaternion.Euler(gridManager.tiles[1].gameObject.transform.rotation.x, 90, gridManager.tiles[1].gameObject.transform.rotation.z);
        for (int i = 100; i < 104; i++)
        {
            GameObject startWall = Instantiate(wall, gridManager.tiles[i].gameObject.transform.position, quaternion);
            startWall.GetComponent<Building_Base>().tile = gridManager.tiles[i];
            startWall.gameObject.GetComponent<Building_Base>().tile.GetComponent<TileOnWhichToPlace>().placed = true;
        }

    }

    private void SetResourceBonus() {
        if (instance == this) {
            resourceBonusFlat = 0;
            foreach (var bonus in gameData.bonuses) {
                if (bonus.Type == BonusType.ResourceGainFlat) {
                    resourceBonusFlat += (int)bonus.Value;
                }
            }
            m_currResourceGain = trashGainSpawned + resourceBonusFlat;
        }
    }
}
