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


    BuildingManager buildingManager;
    void Start(){
        buildingManager=BuildingManager.instance;
        rend = GetComponent<Renderer>();
        startColor=rend.material.color;
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
        turret = (GameObject)Instantiate(selectedTurret, transform.position+offsetFromPlacer, transform.rotation);
    }

    void OnMouseEnter(){
        rend.material.color = hoverColor;
    }

    void OnMouseExit(){
        rend.material.color = startColor;
    }
}
