using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManageableBuilding : MonoBehaviour {
    public static ManageableBuilding selectedBuilding = null;

    public virtual string buildingName { 
        get { return "Uncategorised building"; } 
    }

    public int level { 
        get { return m_level; }
    }

    protected int m_level = 1;
    
    [SerializeField] private GameObject[] UpgradeModels = new GameObject[] {};  
    private int nextModel = 0;

    public string currModelName { get {
        if (nextModel == 0) {
            return "default";
        }
        return UpgradeModels[nextModel-1].name;
    } }

    public void SelectBuilding() {
        if (selectedBuilding == this) {
            selectedBuilding = null;
            // Debug.Log("This building is unselected: " + this.GetHashCode());
        } else {
            selectedBuilding = this;
            // Debug.Log("This building is selected: " + this.GetHashCode());
        }
    }

    public virtual void UpgradeBuilding() { 
        Debug.LogWarning("UpgradeBuilding() Base class method invoked. " + 
            "This should only be visible if a building of this " + 
            "category has no overriding implementation.");
    }

    protected bool UpdateObjectModel(out GameObject newModel) {
        if (nextModel < UpgradeModels.Length) {
            Destroy(this.transform.GetChild(0).gameObject);
            newModel = Instantiate(UpgradeModels[nextModel++], transform);
            newModel.name = currModelName;
            return true;
        }
        newModel = null;
        return false;        
    }
}