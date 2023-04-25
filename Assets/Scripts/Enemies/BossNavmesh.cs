using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BossNavmesh : MonoBehaviour
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
            BossManager[] bosses1 = FindObjectsOfType<BossManager>();
            foreach (var e in bosses1)
            {
                e.ChangeEnemyStateToFinal();
            }
            Debug.Log("Path available");
        }
        else
        {
            pathAvailable = false;
            DestroyBuildings();
            Debug.Log("Path not available");
        }
    }

    void DestroyBuildings()
    {
        GetComponent<BossManager>().ChangeEnemeyStateMoveTo();
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
    public void UpdateSpeed()
    {
        spawnPosition.speed = GetComponent<BossManager>().speed;
    }
}
