using System.Collections;
using System.Collections.Generic;
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


    BuildingManager buildingManager;
    void Start(){        
        gameController = GameController.instance;
        buildingManager=BuildingManager.instance;
        rend = GetComponent<Renderer>();
        startColor=rend.material;
        timeDelay = 0.5f;
    }

    void Update()
    {
        DetectTileClick();
    }

    private void DetectTileClick()
    {
        if (CustomInput.ClickedOnObject(this))
            BuildStructure();
    }

    void BuildStructure(){
        if((turret!=null)||(buildingManager.GetTurret()==null)){
            return;
        }
        
        GameObject selectedTurret = buildingManager.GetTurret();
        ManageableBuilding manageableBuilding = selectedTurret.GetComponent<ManageableBuilding>();
        if (manageableBuilding != null 
            && gameController.resources >= manageableBuilding.buildingPrice) {
            animationPrefab1 = (GameObject)Instantiate(animationPrefab1, transform.position + offsetFromPlacer, transform.rotation);
            animationPrefab2 = (GameObject)Instantiate(animationPrefab2, transform.position + offsetFromPlacer, transform.rotation);
            StartCoroutine(AnimationTimer(timeDelay*2));
            StartCoroutine(SpawnTurretAfterTime(timeDelay, selectedTurret));
            
            gameController.resources -= manageableBuilding.buildingPrice;
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

        Object.Destroy(animationPrefab1);
        Object.Destroy(animationPrefab2);
    }
    IEnumerator SpawnTurretAfterTime(float time, GameObject selectedTurret)
    {
        yield return new WaitForSeconds(time);

        turret = (GameObject)Instantiate(selectedTurret, transform.position + offsetFromPlacer, transform.rotation);

    }
}
