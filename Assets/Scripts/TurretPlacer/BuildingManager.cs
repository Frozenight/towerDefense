using UnityEngine;

public class BuildingManager : MonoBehaviour
{


    public static BuildingManager instance;

    [SerializeField] private GameObject turret1;
    [SerializeField] private GameObject turret2;
    [SerializeField] private GameObject wall;

    private GameObject selectedTurret;

    void Awake (){
        if(instance!=null){
            Debug.LogError("Multiple building managers in scene.");
            return;
        }
        instance=this;
    }

    // Start is called before the first frame update
    public GameObject GetTurret(){
        return selectedTurret;
    }

    void Start()
    {
        selectedTurret = turret1;
    }


    public void SelectTurret1()
    {
        selectedTurret = turret1;
    }

    public void SelectTurret2()
    {
        selectedTurret = turret2;
    }

    public void SelectWall()
    {
        selectedTurret = wall;
    }
}
