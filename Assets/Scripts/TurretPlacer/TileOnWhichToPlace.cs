using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileOnWhichToPlace : MonoBehaviour
{
    public Color hoverColor;
    public Vector3 offsetFromPlacer;

    private GameObject turret;

    private Renderer rend;
    private Color startColor;
    private GameController gameController;

    BuildingManager buildingManager;
    void Start(){        
        gameController = GameController.instance;
        buildingManager=BuildingManager.instance;
        rend = GetComponent<Renderer>();
        startColor=rend.material.color;
    }

    void OnMouseDown(){
        if((turret!=null)||(buildingManager.GetTurret()==null)){
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
        rend.material.color = hoverColor;
    }

    void OnMouseExit(){
        rend.material.color = startColor;
    }
}
