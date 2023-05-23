using System.Numerics;
using UnityEngine;

public class BuildingManager : MonoBehaviour
{


    public static BuildingManager instance;

    [SerializeField] private GameObject turret1;
    [SerializeField] private GameObject turret2;
    [SerializeField] private GameObject wall;
    private TileOnWhichToPlace selectedTile;

    private GameObject selectedTurret;

    void Awake (){
        if(instance!=null){
            Debug.LogError("Multiple building managers in scene.");
            return;
        }
        instance=this;
    }

    public void SetSelectedTile(TileOnWhichToPlace tile)
    {
        selectedTile = tile;
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
        BuildTower();
    }

    public void SelectTurret2()
    {
        selectedTurret = turret2;
        BuildTower();
    }

    public void SelectWall()
    {
        selectedTurret = wall;
        BuildTower();
    }

    public void BuildTower()
    {
        GameObject selectedTurret = GetTurret();
        ManageableBuilding manageableBuilding = selectedTurret.GetComponent<ManageableBuilding>();

        if (manageableBuilding != null && !selectedTile.placed
            && GameController.instance.resources >= manageableBuilding.buildingPrice)
        {
            selectedTile.animationPrefab1 = (GameObject)Instantiate(selectedTile.animationPrefab1, selectedTile.transform.position + selectedTile.offsetFromPlacer, selectedTile.transform.rotation);
            selectedTile.animationPrefab2 = (GameObject)Instantiate(selectedTile.animationPrefab2, selectedTile.transform.position + selectedTile.offsetFromPlacer, selectedTile.transform.rotation);
            selectedTile.animationPrefab1.transform.parent = GameController.instance.vfx.transform;
            selectedTile.animationPrefab2.transform.parent = GameController.instance.vfx.transform;
            selectedTile.animationPrefab1.SetActive(true);
            selectedTile.animationPrefab2.SetActive(true);
            StartCoroutine(selectedTile.AnimationTimer(selectedTile.timeDelay * 2));
            StartCoroutine(selectedTile.SpawnTurretAfterTime(selectedTile.timeDelay, selectedTurret));

            GameController.instance.resources -= manageableBuilding.buildingPrice;
            selectedTile.placed = true;
        }
    }
}
