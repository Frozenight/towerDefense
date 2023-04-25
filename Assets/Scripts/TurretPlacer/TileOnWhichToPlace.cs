using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileOnWhichToPlace : MonoBehaviour
{
    protected GameMode gameMode;
    public Material hoverColor;
    public Vector3 offsetFromPlacer;

    private GameObject turret;

    private Renderer rend;
    private Material startColor;
    private GameController gameController;

    BuildingManager buildingManager;
    void Start(){
        gameController = GameController.instance;
        buildingManager=BuildingManager.instance;
        rend = GetComponent<Renderer>();
        startColor=rend.material;
        gameMode = gameController.GetComponent<GameMode>();
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
        if((turret!=null)||(buildingManager.GetTurret()==null) || (gameMode.isBiuldMode == false))
        {
            return;
        }

        GameObject selectedTurret = buildingManager.GetTurret();
        ManageableBuilding manageableBuilding = selectedTurret.GetComponent<ManageableBuilding>();
        if (manageableBuilding != null 
            && gameController.resources >= manageableBuilding.buildingPrice) {
            turret = (GameObject)Instantiate(selectedTurret, transform.position+offsetFromPlacer, transform.rotation);
            gameController.resources -= manageableBuilding.buildingPrice;
        }
    }

    void OnMouseEnter(){
        rend.material = hoverColor;
    }

    void OnMouseExit(){
        rend.material = startColor;
    }
}
