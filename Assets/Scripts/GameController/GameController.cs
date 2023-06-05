using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using UnityEngine.UI;
using TMPro;
using System.Threading.Tasks;
using GoogleMobileAds.Api;

public class GameController : MonoBehaviour
{
    [Header("Building Prefabs")]
    [SerializeField] private GameObject wall;
    [SerializeField] private GameObject turret_normal;
    [SerializeField] private GameObject turret_frost;
    [SerializeField] private GameObject turret_fire;
    [SerializeField] private GameObject turret_earth;

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
    [SerializeField] private Interstitial ads;
    [SerializeField] private int trashGainEnemyDroped;

    public Rounds rounds;
    public OpenAiAPI aiAPI;
    public enemySpawner spawner;

    private GameData gameData;
    private List<IGameController> gameControllerObjects;
    private FileDataHandler dataHandler;
    // private bool sessionIsOver = false;

    [SerializeField] private EventManager eventManager;
    [SerializeField] private GridManager gridManager;
    [SerializeField] public GameObject vfx;
    [SerializeField] private GameObject bonusText;
    private int resourceBonusFlat = 0;
    private int m_currResourceGain = 0;

    private int WallHealthIncrease = 0;

    [SerializeField] private GameObject[] trashbag_images;
    [SerializeField] private GameObject worker;
    void LoadSessionData() {
        dataHandler = new FileDataHandler(Application.persistentDataPath, fileName);
        SessionData session = dataHandler.LoadSessionData();
        bool newSession = session == null;
        LoadGame(newSession);
        if (newSession) {
            SpawnWalls();
            Building_Base mainbase = GameObject.FindGameObjectWithTag("Base")
                        .GetComponent<Building_Base>();
            mainbase.UpdateWorkerSpeed(0);
        } else {
            if (enemySpawner.instance != null) {
                enemySpawner.instance
                    .ImportSessionData(session.completedWaves);
            } else {
                UnityEngine.Object.FindObjectOfType<enemySpawner>()
                    .ImportSessionData(session.completedWaves);
            }
            resources = session.resources;
            Quaternion quaternion = Quaternion.Euler(gridManager.tiles[1].gameObject.transform.rotation.x, 90, gridManager.tiles[1].gameObject.transform.rotation.z);
            foreach (var b in session.buildings) {
                GameObject usedPrefab = wall;
                switch(b.BuildingType) {
                    case Normal_Turret.m_typeIndex:
                        usedPrefab = turret_normal;
                        break;
                    case Fire_Turret.m_typeIndex:
                        usedPrefab = turret_fire;                        
                        break;
                    case Frost_Turret.m_typeIndex:
                        usedPrefab = turret_frost;                        
                        break;
                    case Earth_Turret.m_typeIndex:
                        usedPrefab = turret_earth;                        
                        break;
                }                    
                if (b.BuildingType > 0) { // not main base
                    GameObject obj = PlaceBuilding(b.TileIndex, usedPrefab, quaternion);
                    if (b.BuildingType > 1)
                        foreach (var t in obj.GetComponents<ManageableBuilding>())
                            if (t.GetType() != typeof(Building_Base))
                                t.ImportSessionData(b);
                            else 
                                (t as Building_Base).ImportSessionData(b, false);
                } else {
                    Building_Base mainbase = GameObject.FindGameObjectWithTag("Base")
                        .GetComponent<Building_Base>();
                    mainbase.ImportSessionData(b, true);
                    mainbase.UpdateWorkerSpeed(session.workerSpeed);
                }
            }
            return;
        }
    }

    private GameObject PlaceBuilding(int i, GameObject prefab, Quaternion quaternion) {
        GameObject building = Instantiate(prefab, gridManager.tiles[i].gameObject.transform.position, quaternion);
        building.GetComponent<Building_Base>().tile = gridManager.tiles[i];
        gridManager.tiles[i].GetComponent<TileOnWhichToPlace>().placed = true;
        return building;
    }

    public int currResourceGain {
        get {
            return m_currResourceGain;
        }
    }

    private void Awake()
    {
        gridManager.GenerateGrid();
        instance = this;
        this.gameControllerObjects = FindAllGameControllerObjects();
        LoadSessionData();
        BuildingSelectUI.SetActive(false);
        aiAPI.GetData();
        vfx = (GameObject)Instantiate(vfx, new Vector3(0, 0, 0), Quaternion.identity);
        SetResourceBonus();
        bonusText.gameObject.SetActive(false);  
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
        bonusText.transform.GetChild(0).transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = GameTexts.NewBonus(bonus.Type, bonus.Value);
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
        SaveSession(sessionIsOver: true);
        MobileAds.Initialize(initstatus => { });
        ads.LoadInterstitialAd();
        if (ads.interstitialAd != null)
        {
            ads.interstitialAd.Show();
        }
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
    public void LoadGame(bool newSession)
    {        
        this.gameData = dataHandler.LoadGameData();

        if (gameData == null) {            
            Debug.Log("No data was found. Initializing data to defaults");
            NewGame();
            // if gameData is null, sessionData should also be null
            if (!newSession) {
                SaveSession(emptySession: true);
                newSession = true;                
            }
        }

        if (!newSession)
        {
            return;
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
        PlayerPrefs.SetInt("CurrentRound", 0);
        dataHandler.SaveGameData(gameData);
    }

    public int GetTileIndex(GameObject tile) {
        return gridManager.tiles.IndexOf(tile);
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
        for (int i = 100; i < 104; i++)
        {
            GameObject startWall = Instantiate(wall, gridManager.tiles[i].gameObject.transform.position, quaternion);
            startWall.GetComponent<Building_Base>().tile = gridManager.tiles[i];
            startWall.gameObject.GetComponent<Building_Base>().tile.GetComponent<TileOnWhichToPlace>().placed = true;
        }

    }

    public void SaveSession(bool emptySession = false, bool sessionIsOver = false, bool endWave =false) {
        if (!sessionIsOver && !endWave)
            if (UnityEngine.Object.FindObjectOfType<EventManager>()
                .currentState == EventManager.Event.defending)
                    return;
        if (emptySession || sessionIsOver) {
            dataHandler.SaveSessionData(default(SessionData));
            return;
        }
        SessionData session = new SessionData();
        session.resources = resources;
        session.completedWaves = enemySpawner.instance.completedWaves;
        ManageableBuilding[] buildings = UnityEngine.Object.FindObjectsOfType<ManageableBuilding>();
        foreach(var b in buildings) {
            if (b.GetType() == typeof(Building_Base)) {
                if (b.gameObject.tag == "Base") {
                    session.workerSpeed = (b as Building_Base).GetWorkerSpeed;
                } else {
                    continue;
                }
            }
            var bd = b.GetExportedData();
            session.buildings.Add(bd);
        }
        dataHandler.SaveSessionData(session);
    }

    public void NullifySessionData() {
        dataHandler.SaveSessionData(default(SessionData));
        return;
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

    public void RemoveTrashBagImage()
    {
        for (int i = 0; i < 5; i++)
        {
            if (trashbag_images[i].activeSelf)
            {
                if (GameObject.FindGameObjectsWithTag("Trash").Length != 5)
                    trashbag_images[i].SetActive(false);
                break;
            }

        }
    }

    public void WallHealthInscrease()
    {
        WallHealthIncrease += 10;
    }

    public int getWallHealthIncrese()
    {
        return WallHealthIncrease;
    }
}
