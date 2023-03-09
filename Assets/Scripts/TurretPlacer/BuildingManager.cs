using UnityEngine;

public class BuildingManager : MonoBehaviour
{


    public static BuildingManager instance;


    void Awake (){
        if(instance!=null){
            Debug.LogError("Multiple building managers in scene.");
            return;
        }
        instance=this;
    }

    public GameObject standardTurretPrefab;
    public GameObject turretTwoPrefab;

    private GameObject selectedTurret;
    // Start is called before the first frame update
    public GameObject GetTurret(){
        return selectedTurret;
    }

    void Start()
    {
        selectedTurret=standardTurretPrefab;
    }

    public void SelectTurret(GameObject turret)
    {
        selectedTurret=turret;
    }
}
