using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
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
    private bool slowed = false;
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
            _Attack();
        if (_currentState == State.Final)
            _FinalMove();
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
        if (Vector3.Distance(transform.position, Objective.transform.position) > 3)
        {
            transform.position = Vector3.MoveTowards(transform.position, Objective.transform.position, speed * Time.deltaTime);
        }
        else
            _currentState = State.Attack;
    }


    private void _Idle()
    {
        _animator.Play("idle");
    }
    private void _Attack()
    {
        if (!Objective.isActiveAndEnabled)
            _currentState = State.Idle;

        if (Objective.tag == "Base")
        {
            if (Vector3.Distance(transform.position, Objective.transform.position) > 8)
                return;
            GetComponent<EnemyNavmesh>().Stop();
        }
        else if (Vector3.Distance(transform.position, Objective.transform.position) > 3)
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
        yield return new WaitForSeconds(4);
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        _roundController.CheckForEndOfRound();
    }
    
    public void ReduceSpeed(float reduce, float reduce_time)
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
