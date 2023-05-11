using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EnemyManager : MonoBehaviour
{
    public AudioClip spawnSound;
    private AudioSource audioSource;
    public GameObject FloatingTextPrefab;
    private enum State
    {
        Idle,
        Moveto,
        Attack,
        Die,
        Final
    }
    private State _currentState;
    [SerializeField] private Animator _animator;
    private Timer _roundController;

    public int damage;
    public float TimeBetweenAttacks;
    public float speed;
    private float temp;

    private float Slow_Amount;
    private float Slow_Time;
    public bool slowed = false;
    float timer = 0;

    private EnemyHealth health;
    private Building_Base Objective;
    private float waitTime = 0;

    [SerializeField] private LayerMask layerMask;
    private float searchDistance = 4f;
    private bool resourceBonusGiven = false;

    bool stuck = false;
    bool tryingToUnStuck = false;
    float stuckTime = 0f;
    EnemyNavmesh navmesh;


    private void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        PlaySpawnSound();
        Objective = GameObject.FindGameObjectWithTag("Base").GetComponent<Building_Base>();
        health = gameObject.GetComponent<EnemyHealth>();
        navmesh = GetComponent<EnemyNavmesh>();
        _roundController = Timer.instance;
    }

    private void PlaySpawnSound()
    {
        if (spawnSound != null)
        {
            audioSource.clip = spawnSound;
            audioSource.Play();
        }
    }

    public void ChangeEnemeyStateMoveTo()
    {
        _animator.Play("run");
        _currentState = State.Moveto;
    }

    public void ChangeEnemyStateToFinal()
    {
        _animator.Play("run");
        _currentState = State.Final;
    }
    public void ResetStuck()
    {
        stuck = false;
    }
    private void Update()
    {
        if (health.getHealth() <= 0)
            _Die();
        if (_currentState == State.Moveto)
        {
            _MoveTo();
            SearchForBuild();
        }
        if (_currentState == State.Idle)
            _Idle();
        if (_currentState == State.Attack)
        {
            _Attack();
            checkForStuckStatus();
            if (stuck)
                StartCoroutine(MoveBack());
        }
        if (_currentState == State.Final)
            _FinalMove();
        if (slowed == true)
        {
            Delay();
        }
    }

    void checkForStuckStatus()
    {
        AnimatorStateInfo stateInfo = _animator.GetCurrentAnimatorStateInfo(0);
        if (navmesh.spawnPosition.velocity == Vector3.zero && stateInfo.IsName("run"))
        {
            stuckTime += Time.deltaTime;
            if (stuckTime > 1)
            {
                stuck = true;
                stuckTime = 0;
            }
        }
    }

    private void SearchForBuild()
    {
        Vector3 centerOffset = new Vector3(0, 1, 0);
        Vector3 forwardDirection = transform.TransformDirection(Vector3.forward);
        Vector3 rightDirection = transform.TransformDirection(Vector3.right);
        RaycastHit hit;

        // Raycast in the center
        if (Physics.Raycast(transform.position + centerOffset, forwardDirection, out hit, searchDistance, layerMask))
        {
            Objective = hit.transform.GetComponent<Building_Base>();
        }

        // Raycast slightly to the right
        if (!Objective && Physics.Raycast(transform.position + centerOffset, forwardDirection + (rightDirection / 4), out hit, searchDistance, layerMask))
        {
            Objective = hit.transform.GetComponent<Building_Base>();
        }

        // Raycast slightly to the left
        if (!Objective && Physics.Raycast(transform.position + centerOffset, forwardDirection - (rightDirection / 4), out hit, searchDistance, layerMask))
        {
            Objective = hit.transform.GetComponent<Building_Base>();
        }
    }

    IEnumerator MoveBack()
    {
        stuck = false;
        tryingToUnStuck = true;
        navmesh.spawnPosition.obstacleAvoidanceType = UnityEngine.AI.ObstacleAvoidanceType.NoObstacleAvoidance;

        transform.rotation = Quaternion.LookRotation(-transform.forward);
        // Walk for 2 seconds
        float moveTimer = 0f;

        while (moveTimer < 1f)
        {
            transform.position += transform.forward * speed * Time.deltaTime;
            moveTimer += Time.deltaTime;
            yield return null;
        }
        GetNewWall();
        navmesh.spawnPosition.obstacleAvoidanceType = UnityEngine.AI.ObstacleAvoidanceType.MedQualityObstacleAvoidance;
    }

    private void GetNewWall()
    {
        GameObject closestWall = null;
        float closestDistance = Mathf.Infinity;

        GameObject[] walls = GameObject.FindGameObjectsWithTag("Wall");
        foreach (GameObject wall in walls)
        {
            float distance = Vector3.Distance(transform.position, wall.transform.position);
            if (distance < closestDistance)
            {
                closestWall = wall;
                closestDistance = distance;
            }
        }

        Objective = closestWall.transform.GetComponent<Building_Base>();
        tryingToUnStuck = false;
        _currentState = State.Moveto;
    }
    public void ResetObjective()
    {
        try
        {
            Objective = GameObject.FindGameObjectWithTag("Base").GetComponent<Building_Base>();
        }
        catch { }

        _currentState = State.Moveto;
    }
    private void _FinalMove()
    {
        Objective = GameObject.FindGameObjectWithTag("Base").GetComponent<Building_Base>();
        if (Vector3.Distance(transform.position, Objective.transform.position) < 8)
        {
            _currentState = State.Attack;
        }
    }

    private void _MoveTo()
    {
        transform.LookAt(Objective.transform);
        tryingToUnStuck = false;
        if (Vector3.Distance(transform.position, Objective.transform.position) > 3)
        {
            transform.position = Vector3.MoveTowards(transform.position, Objective.transform.position, speed * Time.deltaTime);
        }
        else
        {
            _currentState = State.Attack;
        }

    }


    private void _Idle()
    {
        _animator.Play("idle");
    }
    private void _Attack()
    {
        if (tryingToUnStuck)
            return;
        if (!Objective.isActiveAndEnabled)
            _currentState = State.Idle;

        if (Objective.tag == "Base")
        {
            if (Vector3.Distance(transform.position, Objective.transform.position) > 8)
                return;
            GetComponent<EnemyNavmesh>().Stop();
        }
        else if (Vector3.Distance(transform.position, Objective.transform.position) > 2.5f)
        {
            return;
        }
        _animator.Play("attack");
        waitTime -= Time.deltaTime;
        if (waitTime <= 0)
        {
            Objective.ModifyHealth(damage);
            waitTime = TimeBetweenAttacks;
        }
    }

    private void _Die()
    {
        GetComponent<EnemyNavmesh>().Stop();
        transform.gameObject.tag = "Untagged";
        _currentState = State.Die;
        _animator.Play("death");
        StartCoroutine(die());
    }

    IEnumerator die()
    {
        if (!resourceBonusGiven) {
            GameController.instance.AddCountResource("enemyTrash");
            ShowFloatingText("+6");
            resourceBonusGiven = true;
        }
        yield return new WaitForSeconds(4);
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        _roundController.CheckForEndOfRound();
        gameObject.GetComponent<EnemyTrashSpawn>().SpawnTrash();
    }
    
    public void ReduceSpeed(float reduce, float reduce_time)
    {
        if (slowed == false)
        {
            temp = speed;
            slowed = true;
            speed = speed * reduce;
            Slow_Time = reduce_time;
        }
    }
    private void Delay()
    {
        if (timer < Slow_Time)
        {
            timer += Time.deltaTime;
        }
        else
        {
            timer = 0;
            slowed = false;
            speed = temp;
        }

    }

    private void ShowFloatingText(string text)
    {
        Vector3 position = new Vector3(transform.position.x, transform.position.y + 5, transform.position.z) ;
        var textObject = Instantiate(FloatingTextPrefab, position, Quaternion.identity, transform);
        textObject.transform.LookAt(Camera.main.transform);
        textObject.transform.Rotate(0, 180, 0);
        textObject.GetComponent<TextMeshPro>().text = text;
    }
}
