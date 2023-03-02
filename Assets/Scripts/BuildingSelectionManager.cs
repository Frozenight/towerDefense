using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingSelectionManager : MonoBehaviour
{
    private string[] ClickableBuildingTags = new string[] 
    { 
        "Tower"
    }; 
    
    void Update() {
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
                    Turret turret = _hit.collider.GetComponent<Turret>();
                    turret.SelectBuilding();
                }
                Debug.Log(objTr.name + " " + objTag + " " + validClick);
            }
        }    
    }
}
