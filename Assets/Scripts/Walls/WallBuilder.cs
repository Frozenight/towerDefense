using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallBuilder : MonoBehaviour
{
    public static WallBuilder _instance;
    [SerializeField] GameObject selectableWall;
    [SerializeField] BuildingManager buildingManager;
    [SerializeField] EventManager eventManager;

    float raycastHeightOffset = 1f;
    float distance = 4.0f;

    bool _showing = false;
    GameObject selectedMainWall;
    List<GameObject> _instantiatedSelectableWalls = new List<GameObject>();

    Vector3 bottomLeftCorner = new Vector3(-41, -4, -5);
    Vector3 topRightCorner = new Vector3(25, 4, 45);

    private void Start()
    {
        if (_instance != null)
        {
            Debug.LogError("Multiple building managers in scene.");
            return;
        }
        _instance = this;
    }

    public void ShowWallSelection(GameObject mainWall)
    {
        if (_showing)
            HideWallSelection();
        if (eventManager.currentState == EventManager.Event.defending)
            return;
        _showing = true;
        selectedMainWall = mainWall;
        StartCoroutine(InstantiateSelectableWalls());
    }

    public void HideWallSelection()
    {
        if (!_showing)
            return;
        _showing = false;
        HideSelectableWalls();
    }

    public void SelectedSelectableWall()
    {
        buildingManager.SelectSelectedWall();
        HideWallSelection();
    }


    private IEnumerator InstantiateSelectableWalls()
    {
        yield return new WaitForSeconds(0.1f);
        Vector3 center = selectedMainWall.transform.position + new Vector3(0, raycastHeightOffset, 0);
        Vector3 North = selectedMainWall.transform.TransformDirection(Vector3.forward);
        Vector3 South = selectedMainWall.transform.TransformDirection(Vector3.back);
        Vector3 West = selectedMainWall.transform.TransformDirection(Vector3.left);
        Vector3 East = selectedMainWall.transform.TransformDirection(Vector3.right);
        InstantiateSelectableWall(center, North, Quaternion.identity);
        InstantiateSelectableWall(center, South, Quaternion.identity);
        InstantiateSelectableWall(center, West, Quaternion.identity);
        InstantiateSelectableWall(center, East, Quaternion.identity);
    }

    private void InstantiateSelectableWall(Vector3 center, Vector3 direction, Quaternion rotation)
    {
        Vector3 position = selectedMainWall.transform.position + (direction * distance);
        RaycastHit hit;
        if (!IsObjectInCube(position))
            return;
        if (Physics.Raycast(center, direction, out hit, distance))
        {
            return;
        }
        if (direction == Vector3.forward || direction == Vector3.back)
            rotation *= Quaternion.Euler(0, 90, 0);

        GameObject instantiatedWall = Instantiate(selectableWall, position, rotation);
        _instantiatedSelectableWalls.Add(instantiatedWall);
        instantiatedWall.transform.SetParent(selectedMainWall.transform);
    }


    private void HideSelectableWalls()
    {
        foreach (var wall in _instantiatedSelectableWalls)
        {
            Destroy(wall);
        }
        _instantiatedSelectableWalls.Clear();
    }

    bool IsObjectInCube(Vector3 objPos)
    {

        if (objPos.x >= bottomLeftCorner.x && objPos.x <= topRightCorner.x &&
            objPos.y >= bottomLeftCorner.y && objPos.y <= topRightCorner.y &&
            objPos.z >= bottomLeftCorner.z && objPos.z <= topRightCorner.z)
        {
            return true;
        }

        return false;
    }
}
