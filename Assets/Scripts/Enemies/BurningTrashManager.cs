using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BurningTrashManager : MonoBehaviour
{
[SerializeField] private float moveSpeed;
[SerializeField] private float jumpForce;
[SerializeField] private float detectionRange;
[SerializeField] private int damage;
public AudioClip spawnSound;
private AudioSource audioSource;
private Rigidbody _rigidbody;
private Building_Base _base;
private bool _inRange;

private void Start()
{
    audioSource = gameObject.AddComponent<AudioSource>();
    PlaySpawnSound();
    _rigidbody = GetComponent<Rigidbody>();
    _base = GameObject.FindGameObjectWithTag("Base").GetComponent<Building_Base>();
}

private void Update()
{
    MoveTowardsBase();
    JumpAttack();
}

private void MoveTowardsBase()
{
    if (_base == null) return;

    float distance = Vector3.Distance(transform.position, _base.transform.position);

    if (distance <= detectionRange)
    {
        _inRange = true;
        Vector3 direction = (_base.transform.position - transform.position).normalized;
        direction.y = 0;
        transform.position += direction * moveSpeed * Time.deltaTime;
        transform.LookAt(_base.transform);
    }
    else
    {
        _inRange = false;
    }
}

private void PlaySpawnSound()
    {
        if (spawnSound != null)
        {
            audioSource.clip = spawnSound;
            audioSource.Play();
        }
    }

private void JumpAttack()
{
    if (_inRange && Mathf.Abs(_rigidbody.velocity.y) < 0.001f)
    {
        _rigidbody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }
}

private void OnCollisionEnter(Collision collision)
{
    if (collision.gameObject.CompareTag("Base"))
    {
        _base.ModifyHealth(-damage);
    }
}
}