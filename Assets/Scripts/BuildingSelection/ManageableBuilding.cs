using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManageableBuilding : MonoBehaviour {
    public static string NAME_UNCATEGORISED = "Uncategorised building";
    public static string NAME_TURRET = "Turret";
    public static string NAME_BASE = "Recycling Centre";
    public static string NAME_WALL = "Wall";
    public static string NAME_FIRE_TURRET = "Fire Turret";
    public static string NAME_FROST_TURRET = "Frost Turret";
    public static string NAME_EARTH_TURRET = "Earth Turret";

    public static ManageableBuilding selectedBuilding = null;
    private GameMode gameMode;


    public virtual int buildingPrice {
        get { return 5; }
    }

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
    protected int worker_upgrade_price = 25;

    [SerializeField] private GameObject[] UpgradeModels = new GameObject[] {};  
    private int nextModel = 0;
    protected GameController gameController;
    public string currModelName { get {
        if (nextModel == 0) {
            return "default";
        }
        return UpgradeModels[nextModel-1].name;
    } }

    private void Start() {
      
    }

    public virtual void DestroyBuilding() {
        int sell_price =  buildingPrice;
        int one_level_price = 5;
        for (int i = 1; i < m_level; i++)
        {
            sell_price += one_level_price;
            one_level_price += 5;
        }
        GameController.instance.resources += (int)Mathf.Ceil(sell_price / 2f);
        Destroy(gameObject);
    }

    public void SelectBuilding() {
        if (selectedBuilding == this) {
            selectedBuilding = null;
            // Debug.Log("This building is unselected: " + this.GetHashCode());
        } else {
            selectedBuilding = this;
            gameMode = GameController.instance.GetComponent<GameMode>();
            gameMode.changeGameMode(4);
            // Debug.Log("This building is selected: " + this.GetHashCode());
        }
    }

    public virtual void UpgradeBuilding() {
        if (GameController.instance.resources < m_upgrade_price)
            return;
        GameController.instance.resources -= m_upgrade_price;
        m_level += 1;
        m_upgrade_price += 5;
        if (m_level % 5 == 0)
            UpdateObjectModel(out GameObject newModel); 
        Debug.LogWarning("UpgradeBuilding() Base class method invoked. ");
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
            foreach(Transform child in transform) {
                if(child.tag == "Model")
                    Destroy(child.gameObject);
            }
            newModel = Instantiate(UpgradeModels[nextModel++], transform);
            newModel.name = currModelName;
            return true;
        }
        newModel = null;
        return false;        
    }

    public virtual void HighlightBuilding() {

    }

    public virtual void UnhighlightBuilding() {

    }

}