using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingSelectionManager : MonoBehaviour
{
    [SerializeField] private ManageableBuildingTab manageBuildingTab;
    private string[] ClickableBuildingTags = new string[] 
    { 
        "Tower"
    }; 
    private bool manageTabOpen = false;
    private float minXOfTab = 0f;
    private float maxXOfTab = 0f;
    private float minYOfTab = 0f;
    private float maxYOfTab = 0f;

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
        ManageableBuilding building =
            _hit.collider.GetComponent<ManageableBuilding>();
        building.SelectBuilding();
        if (manageBuildingTab.gameObject.activeSelf)
            manageBuildingTab.ResetNamePulse();
        else
            manageBuildingTab.gameObject.SetActive(true);
        manageBuildingTab.FillBuildingData(building);
        manageTabOpen = true;
    }

    private void UnselectB()
    {
        ManageableBuilding.selectedBuilding = null;
        manageBuildingTab.gameObject.SetActive(false);
        manageTabOpen = false;
        // Debug.Log("Unselected building");
    }

    private void DetectBuildingClick()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePos = Input.mousePosition;            
            if (PressedOnOpenTab(mousePos))
                return;
            RaycastHit _hit;
            Ray _ray = Camera.main.ScreenPointToRay(mousePos);
            if (Physics.Raycast(_ray, out _hit))
            {

                Transform objTr = _hit.collider.transform;
                while (objTr.parent != null)
                    objTr = objTr.parent;
                string objTag = objTr.gameObject.tag;
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
                    SelectB(_hit);
                }
                else
                {
                    UnselectB();
                }
                // Debug.Log(objTr.name + " " + objTag + " " + validClick);
            }
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
        // Debug.Log(rt.rect);
        DetectBuildingClick();
    }    
}
