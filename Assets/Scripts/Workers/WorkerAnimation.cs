using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorkerAnimation : MonoBehaviour
{
    private Animator animator;
    private MovementAnimated worker;
    private PickUpAnimated pickup;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        worker = GetComponent<MovementAnimated>();
        pickup = GetComponent<PickUpAnimated>();
    }

    // Update is called once per frame
    void Update()
    {
        if(animator != null)
        {
            ////to go towards trash
            //if(worker.goingBackward == false && pickup.HasTrash == false)
            //{
            //    animator.SetBool("GoingBackward", false);
            //    animator.SetBool("HasTrash", false);
            //}
            ////to go back with trash
            //if (worker.goingBackward == true && pickup.HasTrash == true)
            //{
            //    animator.SetBool("GoingBackward", true);
            //}
            ////to pick up trash
            //if (pickup.HasTrash == true)
            //{
            //    animator.SetBool("HasTrash", true);
            //}

        }
    }
}
