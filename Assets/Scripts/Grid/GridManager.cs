using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GridManager : MonoBehaviour
{
    [SerializeField] private int _width, _height;

    [SerializeField] private GameObject block;
    [SerializeField] private GameObject block2;
    // Start is called before the first frame update
    void Start()
    {
        GenerateGrid();
        GenerateFakeGrid();
    }

    void GenerateGrid()
    {
        for(int x = -23; x < _width; x = x + 2 )
        {
            for(int y = -13; y < _height; y = y + 2)
            {
                var spawnedTile = Instantiate(block, new Vector3(x, 0.2f, y), Quaternion.identity);
                
                spawnedTile.name = $"Tile {x} {y}";
                //Debug.Log($"{x},{y}");
                spawnedTile.transform.parent = this.transform;
            }
        }
    }

    void GenerateFakeGrid()
    {
        for (int x = -23; x < _width; x = x + 2)
        {
            for (int y = -23; y < -13; y = y + 2)
            {
                var spawnedTile = Instantiate(block2, new Vector3(x, 0.2f, y), Quaternion.identity);

                spawnedTile.name = $"FAKE t" +
                    $"ile {x} {y}";
                //Debug.Log($"{x},{y}");
                spawnedTile.transform.parent = this.transform;
            }
        }
    }
}
