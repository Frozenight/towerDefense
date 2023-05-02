using UnityEngine;

public class BuildingManager : MonoBehaviour
{


    public static BuildingManager instance;

    [SerializeField] private GameObject turret1;
    [SerializeField] private GameObject turret2;
    [SerializeField] private GameObject wall1;
    [SerializeField] private GameObject wall2;
    [SerializeField] private GameObject wall3;
    [SerializeField] private GameObject wall4;
    [SerializeField] private GameObject wall5;

    private GameObject selectedBuilding;

    void Awake (){
        if(instance!=null){
            Debug.LogError("Multiple building managers in scene.");
            return;
        }
        instance=this;
    }

    public GameObject GetTurret(){
        return selectedBuilding;
    }

    void Start()
    {
        selectedBuilding = turret1;
    }


    public void SelectTurret1()
    {
        selectedBuilding = turret1;
    }

    public void SelectTurret2()
    {
        selectedBuilding = turret2;
    }

    public void SelectWall1()
    {
        selectedBuilding = wall1;
    }

    public void SelectWall2()
    {
        selectedBuilding = wall2;
    }

    public void SelectWall3()
    {
        selectedBuilding = wall3;
    }

    public void SelectWall4()
    {
        selectedBuilding = wall4;
    }

    public void SelectWall5()
    {
        selectedBuilding = wall5;
    }
}
