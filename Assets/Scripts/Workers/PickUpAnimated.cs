using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PickUpAnimated : MonoBehaviour
{
    public bool HasTrash;
    public bool Idle;
    private float time;
    private float timeDelay;
    private float timeDelay2;
    private Collider trashToPickUp;
    private Animator animator;


    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        HasTrash = false;
        time = 0f;
        timeDelay = 0.1f;
    }

    // Update is called once per frame
    void Update()
    {
        if (HasTrash && trashToPickUp != null)
        {
            time += Time.deltaTime;
            if (time > timeDelay)
            {
                GameController.instance.trashObjects.Remove(trashToPickUp.gameObject.GetComponent<TrashObject>());
                Destroy(trashToPickUp.gameObject);
                time = 0f;
                trashToPickUp = null;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Trash") && trashToPickUp == null && !HasTrash)
        {
            trashToPickUp = other;
            HasTrash = true;
            animator.SetTrigger("PickUp");
        }
        else if (other.gameObject.CompareTag("EnemyTrash") && trashToPickUp == null)
        {
            trashToPickUp = other;
            HasTrash = true;
            animator.SetTrigger("PickUp");
        }
    }


}
