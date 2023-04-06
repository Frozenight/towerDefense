using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour
{
    private enemySpawner enemyController;

    void Start()
    {
        enemyController = enemySpawner.instance;
    }

    private void OnDestroy()
    {
        enemyController.OnBuildingDestroyed();
    }
}
