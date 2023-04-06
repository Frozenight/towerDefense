using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyNavmesh : MonoBehaviour
{
    public NavMeshAgent spawnPosition;
    public Transform goal;

    [HideInInspector]
    public bool pathAvailable;
    public NavMeshPath navMeshPath;

    void Start()
    {
        navMeshPath = new NavMeshPath();
        goal = GameObject.FindGameObjectWithTag("Base").GetComponent<Building_Base>().transform;

        CheckIfPathAvailable();
    }

    void CheckIfPathAvailable()
    {
        if (CalculateNewPath() == true)
        {
            pathAvailable = true;
            spawnPosition.destination = goal.position;
            EnemyManager[] enemies1 = FindObjectsOfType<EnemyManager>();
            foreach (var e in enemies1)
            {
                e.ChangeEnemyStateToFinal();
            }
            print("Path available");
        }
        else
        {
            pathAvailable = false;
            DestroyBuildings();
            print("Path not available");
        }
    }

    void DestroyBuildings()
    {
        GetComponent<EnemyManager>().ChangeEnemeyStateMoveTo();
    }

    bool CalculateNewPath()
    {
        spawnPosition.CalculatePath(goal.position, navMeshPath);
        if (navMeshPath.status != NavMeshPathStatus.PathComplete)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    public void Stop()
    {
        gameObject.GetComponent<NavMeshAgent>().isStopped = true;
    }

    public void OnBuildingDestroyed()
    {
        StartCoroutine(waitForCheck());
    }

    IEnumerator waitForCheck()
    {
        yield return new WaitForSeconds(0.2f);
        CheckIfPathAvailable();
    }
}
