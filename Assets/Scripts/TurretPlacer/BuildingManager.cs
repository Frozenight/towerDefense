using UnityEngine;

public class BuildingManager : MonoBehaviour
{


    public static BuildingManager instance;

    [SerializeField] private GameObject turret1;
    [SerializeField] private GameObject turret2;
    [SerializeField] private GameObject wall;
    private TileOnWhichToPlace selectedTile;

    private GameObject selectedTurret;

    [SerializeField] GridManager gridManager;


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

    private void GetTile()
    {
        // Create a ray from the mouse cursor on screen in the direction of the camera.
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            // If we hit an object, get its position
            Vector3 hitObjectPosition = hit.transform.position;

            // Find the tile that occupies the same position as the hit object
            TileOnWhichToPlace hitTile = FindTileByPosition(hitObjectPosition);

            if (hitTile != null)
            {
                selectedTile = hitTile;
            }
        }
    }

    TileOnWhichToPlace FindTileByPosition(Vector3 position)
    {
        foreach (GameObject tile in gridManager.tiles)
        {
            Vector2Int tilePosition = new Vector2Int(Mathf.RoundToInt(tile.transform.position.x / 4), Mathf.RoundToInt(tile.transform.position.z / 4));
            Vector2Int hitObjectPosition = new Vector2Int(Mathf.RoundToInt(position.x / 4), Mathf.RoundToInt(position.z / 4));

            if (tilePosition == hitObjectPosition)
            {
                return tile.GetComponent<TileOnWhichToPlace>();
            }
        }

        return null;
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

    public void SelectSelectedWall()
    {
        selectedTurret = wall;
        GetTile();
        BuildTower();
    }
}
