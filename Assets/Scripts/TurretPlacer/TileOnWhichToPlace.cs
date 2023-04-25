using System.Collections;
using System.Collections.Generic;
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
    private float timeDelay;
    public bool placed = false;

    protected GameMode gameMode;

    private static float minXOfTab = 0f;
    private static float maxXOfTab = 0f;
    private static float minYOfTab = 0f;
    private static float maxYOfTab = 0f;


    BuildingManager buildingManager;
    void Start(){        
        gameController = GameController.instance;
        buildingManager=BuildingManager.instance;
        rend = GetComponent<Renderer>();
        startColor=rend.material;
        timeDelay = 0.5f;
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
        if (CustomInput.ClickedOnObject(this) && !PressedOnOpenTab(Input.mousePosition))
            BuildStructure();
    }

    void BuildStructure(){
        if ((turret != null) || (buildingManager.GetTurret() == null) || (gameMode.isBiuldMode == false)) 
        { 
                return;
        }
        
        GameObject selectedTurret = buildingManager.GetTurret();
        ManageableBuilding manageableBuilding = selectedTurret.GetComponent<ManageableBuilding>();
        if (manageableBuilding != null && !placed
            && gameController.resources >= manageableBuilding.buildingPrice) {
            animationPrefab1 = (GameObject)Instantiate(animationPrefab1, transform.position + offsetFromPlacer, transform.rotation);
            animationPrefab2 = (GameObject)Instantiate(animationPrefab2, transform.position + offsetFromPlacer, transform.rotation);
            animationPrefab1.transform.parent = gameController.vfx.transform;
            animationPrefab2.transform.parent = gameController.vfx.transform;
            animationPrefab1.SetActive(true);
            animationPrefab2.SetActive(true);
            StartCoroutine(AnimationTimer(timeDelay*2));
            StartCoroutine(SpawnTurretAfterTime(timeDelay, selectedTurret));

            gameController.resources -= manageableBuilding.buildingPrice;
            placed = true;
        }
    }

    void OnMouseEnter(){
        rend.material = hoverColor;
    }

    void OnMouseExit(){
        rend.material = startColor;
    }
    IEnumerator AnimationTimer(float time)
    {
        yield return new WaitForSeconds(time);

        animationPrefab1.SetActive(false);
        animationPrefab2.SetActive(false);
        //Object.Destroy(animationPrefab1);
        //Object.Destroy(animationPrefab2);

    }
    IEnumerator SpawnTurretAfterTime(float time, GameObject selectedTurret)
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
