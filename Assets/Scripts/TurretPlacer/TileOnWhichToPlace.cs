using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class TileOnWhichToPlace : MonoBehaviour
{

    public Material hoverColor;
    public Vector3 offsetFromPlacer;
    public GameObject animationPrefab1;
    public GameObject animationPrefab2;

    private GameObject turret;
    private Renderer rend;
    private Material startColor;
    private GameController gameController;
    public float timeDelay;
    public bool placed = false;

    protected GameMode gameMode;

    private static float minXOfTab = 0f;
    private static float maxXOfTab = 0f;
    private static float minYOfTab = 0f;
    private static float maxYOfTab = 0f;


    //BuildingManager buildingManager;
    void Start()
    {
        gameController = GameController.instance;
        //buildingManager = BuildingManager.instance;
        rend = GetComponent<Renderer>();
        startColor=rend.material;
        timeDelay = 0.3f;
        gameMode = gameController.GetComponent<GameMode>();
    }

    void Update()
    {
        DetectTileClick();
    }

    public static void SetBoundaries(RectTransform rect)
    {
        Vector3[] corners = new Vector3[4];
        rect.GetWorldCorners(corners);
        minXOfTab = corners[0].x;
        maxXOfTab = corners[2].x;
        minYOfTab = corners[0].y;
        maxYOfTab = corners[2].y;
    }

    private bool PressedOnOpenTab(Vector3 mousePos)
    {
        if (Mathf.Clamp(mousePos.x, minXOfTab, maxXOfTab) == mousePos.x)
        {
            if (Mathf.Clamp(mousePos.y, minYOfTab, maxYOfTab) == mousePos.y)
            {
                return true;
            }
        }
        return false;
    }

    private void DetectTileClick()
    {
        if (!PressedOnOpenTab(Input.mousePosition))
        {
            if (CustomInput.ClickedOnObject(this) && gameMode.isDefendMode == false)
            {
                BuildingManager.instance.SetSelectedTile(this);
                BuildStructure();
            }
            else if (CustomInput.GetOneTouchDown())
            {
                OnSelectExit();
            }
        }

    }
    void BuildStructure()
    {
        if (gameMode.isDefendMode == true)
        {
            return;
        }
        GameController.instance.BuildingSelectUI.SetActive(true);
        Vibration.Vibrate(30);
        rend.material = hoverColor;
    }

    void OnSelectExit(){
        rend.material = startColor;
        GameController.instance.BuildingSelectUI.SetActive(false);
    }
    public IEnumerator AnimationTimer(float time)
    {
        yield return new WaitForSeconds(time);

        animationPrefab1.SetActive(false);
        animationPrefab2.SetActive(false);

    }
    public IEnumerator SpawnTurretAfterTime(float time, GameObject selectedTurret)
    {
        yield return new WaitForSeconds(time);
        turret = (GameObject)Instantiate(selectedTurret, transform.position + offsetFromPlacer, transform.rotation);
        turret.gameObject.GetComponent<Building_Base>().tile = gameObject;
    }

    public void ChangePlacedState()
    {
        placed = false;
    }
    public void Reset()
    {
        
    }
}
