using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingSelectionManager : MonoBehaviour
{
    private string[] ClickableBuildingTags = new string[] 
    { 
        "Tower"
    }; 
    
    void LateUpdate() {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit _hit;
            Ray _ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(_ray, out _hit))
            {
                Transform objTr = _hit.collider.transform;
                while (objTr.parent != null)
                    objTr = objTr.parent;
                string objTag = objTr.gameObject.tag;
                bool validClick = false;
                foreach (string tag in ClickableBuildingTags) {
                    if (tag == objTag) {
                        validClick = true;
                        break;
                    }
                }
                if (validClick)
                {
                    ManageableBuilding building = 
                        _hit.collider.GetComponent<ManageableBuilding>();
                    building.SelectBuilding();
                } else {
                    ManageableBuilding.selectedBuilding = null;
                    Debug.Log("Unselected building");
                }
                // Debug.Log(objTr.name + " " + objTag + " " + validClick);
            } 
        } else if (Input.GetKeyDown(KeyCode.U) 
            && ManageableBuilding.selectedBuilding != null) {
            ManageableBuilding.selectedBuilding.UpgradeBuilding();
        }    
    }
}
