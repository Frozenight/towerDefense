using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossManager : EnemyManager
{
    private static ILogger logger = Debug.unityLogger;
    private static string kTAG = "MyGameTag";
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

    public new int damage;
    public new float TimeBetweenAttacks;
    public new float speed;
    private float temp;

    private float Slow_Amount;
    private float Slow_Time;
    private new bool slowed = false;
    float timer = 0;

    private EnemyHealth health;
    private Building_Base Objective;
    private float waitTime = 0;

    [SerializeField] private LayerMask layerMask;
    private float searchDistance = 4f;

    private void Start()
    {
        Objective = GameObject.FindGameObjectWithTag("Base").GetComponent<Building_Base>();
        health = gameObject.GetComponent<EnemyHealth>();

        _roundController = Timer.instance;
    }

    public new void ChangeEnemeyStateMoveTo()
    {
        //_animator.Play("run");
        _currentState = State.Moveto;
    }

    public new void ChangeEnemyStateToFinal()
    {
        //_animator.Play("run");
        _currentState = State.Final;
    }

    private void Update()
    {
        logger.Log("", Objective.buildingName);
        if (health.getHealth() <= 0)
        {
            _animator.SetTrigger("Death");
            _Die();
        }
        if (_currentState == State.Moveto)
        {
            _animator.SetBool("isAttacking", false);
            _MoveTo();
            SearchForBuild();
        }
        if (_currentState == State.Idle)
        {
            _animator.SetBool("isAttacking", false);
            _Idle();
        }
        if (_currentState == State.Attack)
        {
            _animator.SetBool("isAttacking", true);
            _Attack();
        }
        if (_currentState == State.Final)
        {
            _animator.SetBool("isAttacking", false);
            _FinalMove();
        }
        if (slowed == true)
        {
            Delay();
        }
    }

    private void SearchForBuild()
    {
        RaycastHit hit;
        // For debuging to check if raycast is correct
        if (Physics.Raycast(transform.position + new Vector3(0, 1), transform.TransformDirection(Vector3.forward), out hit, searchDistance, layerMask))
        {
            Objective = hit.transform.GetComponent<Building_Base>();
        }
        else if (Physics.Raycast(transform.position + new Vector3(0, 1), transform.TransformDirection(Vector3.forward + (transform.right / 4)), out hit, searchDistance, layerMask))
        {
            Objective = hit.transform.GetComponent<Building_Base>();
        }
        else if (Physics.Raycast(transform.position + new Vector3(0, 1), transform.TransformDirection(Vector3.forward - (transform.right / 4)), out hit, searchDistance, layerMask))
        {
            Objective = hit.transform.GetComponent<Building_Base>();
        }
    }
    public new void ResetObjective()
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
        if (Vector3.Distance(transform.position, Objective.transform.position) < 12)
        {
            _currentState = State.Attack;
        }
    }

    private void _MoveTo()
    {
        Debug.Log(Vector3.Distance(transform.position, Objective.transform.position));
        transform.LookAt(Objective.transform);
        if (Vector3.Distance(transform.position, Objective.transform.position) > 3)
        {
            transform.position = Vector3.MoveTowards(transform.position, Objective.transform.position, speed * Time.deltaTime);
        }
        else
            _currentState = State.Attack;
    }


    private void _Idle()
    {
        //_animator.Play("idle");
    }
    private void _Attack()
    {
        if (!Objective.isActiveAndEnabled)
            _currentState = State.Idle;

        if (Objective.tag == "Base")
        {
            if (Vector3.Distance(transform.position, Objective.transform.position) > 12)
                return;
            GetComponent<BossNavmesh>().Stop();
        }
        else if (Vector3.Distance(transform.position, Objective.transform.position) > 12)
        {
            return;
        }
        if (waitTime <= 1 && waitTime > 0.9f)
        {
            _animator.SetTrigger("StopWaiting");
        }
        waitTime -= Time.deltaTime;
        if (waitTime <= 0)
        {
            if(Objective.tag == "Wall")
                Objective.ModifyHealth(damage * 2);
            else
                Objective.ModifyHealth(damage);
            waitTime = TimeBetweenAttacks;
        }
    }

    private void _Die()
    {
        GetComponent<BossNavmesh>().Stop();
        transform.gameObject.tag = "Untagged";
        _currentState = State.Die;
        //_animator.Play("death");
        StartCoroutine(die());
    }

    IEnumerator die()
    {
        yield return new WaitForSeconds(4);
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        _roundController.CheckForEndOfRound();
    }

    public new void ReduceSpeed(float reduce, float reduce_time)
    {
        if (slowed == false)
        {
            Debug.Log("SlowHit");
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
}
