using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingSelectionManager : MonoBehaviour
{
    GameMode gameMode;
    [SerializeField] private ManageableBuildingTab manageBuildingTab;
    private string[] ClickableBuildingTags = new string[] 
    { 
        "Tower",
        "Base",
        "Wall"
    }; 
    private bool manageTabOpen = false;
    private float minXOfTab = 0f;
    private float maxXOfTab = 0f;
    private float minYOfTab = 0f;
    private float maxYOfTab = 0f;

    private ManageableBuilding oldBuilding;


    private int clickType = -1;
    RaycastHit lastRaycast;

    void Awake() {
        RectTransform rt = manageBuildingTab.GetComponent<RectTransform>();
        Vector3[] corners = new Vector3[4];
        rt.GetWorldCorners(corners);
        minXOfTab = corners[0].x;
        maxXOfTab = corners[2].x;
        minYOfTab = corners[0].y;
        maxYOfTab = corners[2].y;        
        manageBuildingTab.closeTab = UnselectB;
        manageBuildingTab.gameObject.SetActive(false);
        
    }

    private void SelectB(RaycastHit _hit)
    {
        if (ManageableBuilding.selectedBuilding != null)
        {
            ManageableBuilding.selectedBuilding.GetComponent<Outline>().enabled = false;
        }
        ManageableBuilding building =
            _hit.collider.GetComponent<ManageableBuilding>();
        oldBuilding = building;
        building.SelectBuilding();
        if (manageBuildingTab.gameObject.activeSelf)
            manageBuildingTab.ResetNamePulse();
        else
            manageBuildingTab.gameObject.SetActive(true);
        manageBuildingTab.FillBuildingData(building);
        manageTabOpen = true;
        building.GetComponent<Outline>().enabled = true;
    }

    private void UnselectB()
    {
        if (ManageableBuilding.selectedBuilding != null)
        {
            ManageableBuilding.selectedBuilding.GetComponent<Outline>().enabled = false;
        }
        ManageableBuilding.selectedBuilding = null;
        manageBuildingTab.gameObject.SetActive(false);
        manageTabOpen = false;
        gameMode = GameController.instance.GetComponent<GameMode>();
        gameMode.changeGameMode(1);
        // Debug.Log("Unselected building");
    }

    private void DetectBuildingClick()
    {
        if (CustomInput.GetOneTouchDown())
        {
            Vector3 mousePos = Input.mousePosition;            
            if (PressedOnOpenTab(mousePos))
                return;
            RaycastHit _hit;
            Ray _ray = Camera.main.ScreenPointToRay(mousePos);
            if (Physics.Raycast(_ray, out _hit))
            {
                lastRaycast = _hit;
                Transform objTransform = _hit.collider.transform;
                while (objTransform.parent != null)
                    objTransform = objTransform.parent;
                string objTag = objTransform.gameObject.tag;
                bool validClick = false;
                foreach (string tag in ClickableBuildingTags)
                {
                    if (tag == objTag)
                    {
                        if (_hit.collider.GetComponent<ManageableBuilding>() 
                            != ManageableBuilding.selectedBuilding)
                            validClick = true;
                        break;
                    }
                }
                if (validClick)
                {
                    clickType = 0;
                }
                else
                {
                    clickType = 1;
                }
                // Debug.Log(objTransform.name + " " + objTag + " " + validClick);
            }
        } else if (CustomInput.GetNoTouchOrTouchUp()) {
            Vector3 mousePos = Input.mousePosition;            
            if (PressedOnOpenTab(mousePos))
                return;
            RaycastHit _hit;
            Ray _ray = Camera.main.ScreenPointToRay(mousePos);
            if (Physics.Raycast(_ray, out _hit))
            { 
                if (lastRaycast.collider == _hit.collider) {
                    if (clickType == 0) {
                        SelectB(_hit);
                    } else if (clickType == 1) {
                        UnselectB();
                    }
                }
            }
            clickType = -1;
        }
    }

    private bool PressedOnOpenTab(Vector3 mousePos)
    {
        if (manageTabOpen) {
            if (Mathf.Clamp(mousePos.x, minXOfTab, maxXOfTab) == mousePos.x) {
                if (Mathf.Clamp(mousePos.y, minYOfTab, maxYOfTab) == mousePos.y) {
                    return true;
                }
            }
        }        
        return false;
    }

    void LateUpdate()
    {
        DetectBuildingClick();
    }    
}
