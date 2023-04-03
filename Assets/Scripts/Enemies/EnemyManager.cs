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
        Die
    }
    private State _currentState;
    [SerializeField] private Animator _animator;

    public int damage;
    public float TimeBetweenAttacks;
    public float speed;
    private float temp;

    private float Slow_Amount;
    private float Slow_Time;
    private bool slowed = false;
    float timer = 0;

    private EnemyHealth health;
    private Building_Base MainBase;
    private Transform MainBaseTransform;
    private float waitTime = 0;

    private void Start()
    {
        MainBase = GameObject.FindGameObjectWithTag("Base").GetComponent<Building_Base>();
        health = gameObject.GetComponent<EnemyHealth>();
        MainBaseTransform = MainBase.transform;
        _currentState = State.Moveto;
    }

    private void Update()
    {
        if (health.getHealth() <= 0)
            _Die();
        if (_currentState == State.Moveto)
            _MoveTo();
        if (_currentState == State.Idle)
            _Idle();
        if (_currentState == State.Attack)
            _Attack();

        if (slowed == true)
        {
            Delay();
        }
    }

    private void _MoveTo()
    {
        transform.LookAt(MainBaseTransform);
        if (Vector3.Distance(transform.position, MainBaseTransform.position) > 8)
        {
            transform.position = Vector3.MoveTowards(transform.position, MainBaseTransform.position, speed * Time.deltaTime);
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
        _animator.Play("attack");
        waitTime -= Time.deltaTime;
        if (waitTime <= 0)
        {
            MainBase.ModifyHealth(damage);
            waitTime = TimeBetweenAttacks;
        }

        if (MainBase.maxHealth <= 0)
        {
            _currentState = State.Idle;
        }

    }

    private void _Die()
    {
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
