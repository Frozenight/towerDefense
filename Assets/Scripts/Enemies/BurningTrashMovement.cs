using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BurningTrashMovement : MonoBehaviour
{
    public float initialSpeed = 5f;
    public float acceleration = 0.5f;
    private Transform mainBuilding;
    public float enemyDistance;
    public float avoidTowerDistance = 10f;
    private NavMeshAgent navMeshAgent;
    public float jumpForce = 5f;
    public float jumpInterval = 5f;
    private bool isJumping = false;

    private List<Vector3> directions = new List<Vector3>
    {
        Vector3.forward,
        Vector3.back,
        Vector3.left,
        Vector3.right,
        Vector3.up,
        Vector3.down
    };
    // Start is called before the first frame update
    void Start()
    {
        mainBuilding = GameObject.FindWithTag("Base").transform;
        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.speed = initialSpeed;
        navMeshAgent.destination = mainBuilding.position;
        navMeshAgent.avoidancePriority = 50;
        navMeshAgent.angularSpeed = 360;
        StartCoroutine(JumpingRoutine());
    }

    // Update is called once per frame
    void Update()
    {

        if (mainBuilding == null)
        {
            gameObject.GetComponent<Animator>().Play("idle");
        }
        else
        {
            Collider[] towersInRange = Physics.OverlapSphere(transform.position, avoidTowerDistance);
            Vector3 avoidanceDirection = Vector3.zero;
            int avoidCount = 0;

            foreach (Collider tower in towersInRange)
            {
                if (tower.CompareTag("Tower"))
                {
                    avoidCount++;
                    avoidanceDirection += (transform.position - tower.transform.position);
                }
            }

            if (avoidCount > 0)
            {
                avoidanceDirection /= avoidCount;
                avoidanceDirection = avoidanceDirection.normalized;
                Vector3 targetPosition = mainBuilding.position + avoidanceDirection * avoidTowerDistance;
                NavMeshPath path = new NavMeshPath();
                navMeshAgent.CalculatePath(targetPosition, path);
                if (path.status == NavMeshPathStatus.PathComplete)
                {
                    navMeshAgent.SetPath(path);
                }
            }
            else
            {
                navMeshAgent.SetDestination(mainBuilding.position);
            }

            navMeshAgent.speed += acceleration * Time.deltaTime;

            if (navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)
            {
                GetComponent<Rigidbody>().velocity = Vector3.zero;
                gameObject.GetComponent<Animator>().Play("attack");
            }
        }
    }

    private IEnumerator JumpingRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(jumpInterval);

            if (!isJumping)
            {
                isJumping = true;
                navMeshAgent.isStopped = true;
                GetComponent<Rigidbody>().AddForce(GetDirectionToMainBuilding() * jumpForce, ForceMode.Impulse);
            }

            if (isJumping)
                {
                    isJumping = false;
                    navMeshAgent.isStopped = false;
                    navMeshAgent.speed = navMeshAgent.speed/2;
                }
        }
    }

    private Vector3 GetDirectionToMainBuilding()
    {
        Vector3 direction = mainBuilding.position - transform.position;
        return direction.normalized;
    }


}