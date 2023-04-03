using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManageableBuilding : MonoBehaviour {
    public static string NAME_UNCATEGORISED = "Uncategorised building";
    public static string NAME_TURRET = "Turret";
    public static string NAME_BASE = "Recycling Centre";
    public static string NAME_FIRE_TURRET = "Fire Turret";
    public static string NAME_FROST_TURRET = "Frost Turret";
    public static string NAME_EARTH_TURRET = "Earth Turret";

    public static ManageableBuilding selectedBuilding = null;

    public virtual string buildingName { 
        get { return NAME_UNCATEGORISED; } 
    }
    
    public virtual bool canDestroyManually  {
        get { return true; }
    }

    public int level { 
        get { return m_level; }
    }

    public int upgrade_Price {
        get { return m_upgrade_price; }
    }

    protected int m_level = 1;
    protected int m_upgrade_price = 5;
    
    [SerializeField] private GameObject[] UpgradeModels = new GameObject[] {};  
    private int nextModel = 0;

    public string currModelName { get {
        if (nextModel == 0) {
            return "default";
        }
        return UpgradeModels[nextModel-1].name;
    } }

    public virtual void DestroyBuilding() {
        Destroy(gameObject);
    }

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

    //Methods for changing turret types
    public virtual void ChangeTypeFire()
    { }
    public virtual void ChangeTypeFrost()
    { }
    public virtual void ChangeTypeEarth()
    { }

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