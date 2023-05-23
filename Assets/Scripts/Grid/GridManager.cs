using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GridManager : MonoBehaviour
{
    [SerializeField] private int _width, _height;

    [SerializeField] private GameObject block;
    [SerializeField] private GameObject block2;
    public List<GameObject> tiles;


    void Start()
    {
        GenerateGrid();
        GameController.instance.SpawnWalls();
    }

    void GenerateGrid()
    {
        for(int x = -40; x < _width; x = x + 4 )
        {
            for(int y = -3; y < _height; y = y + 4)
            {
                if(!((x == -8 && y == 37) ||( x == -8 && y == 41) || (x == -8 && y == 45)))
                {
                    var spawnedTile = Instantiate(block, new Vector3(x, 0.2f, y), Quaternion.identity);

                    spawnedTile.name = $"Tile {x} {y}";
                    //Debug.Log($"{x},{y}");
                    spawnedTile.transform.parent = this.transform;
                    tiles.Add(spawnedTile);
                }
            }
        }
    }
}
