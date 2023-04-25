using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using TreeEditor;
using UnityEngine;

public class PickUpAnimated : MonoBehaviour
{
    public GameObject FloatingTextPrefab;
    public bool HasTrash;
    public bool Idle;
    private float time;
    private float timeDelay;
    private float timeDelay2;
    private Collider other;
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
        if (HasTrash && other != null)
        {
            time += Time.deltaTime;
            if (time > timeDelay)
            {
                GameController.instance.trashObjects.Remove(other.gameObject.GetComponent<TrashObject>());
                Destroy(other.gameObject);
                time = 0f;
                other = null;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Trash"))
        {
            this.other = other;
            HasTrash = true;
            animator.SetTrigger("PickUp");
            GameController.instance.AddCountRecource("trash");
            if(FloatingTextPrefab)
            {
                ShowFloatingText("+5");
            }
        }
        else if (other.gameObject.CompareTag("EnemyTrash"))
        {
            this.other = other;
            HasTrash = true;
            animator.SetTrigger("PickUp");
            GameController.instance.AddCountRecource("enemyTrash");
            if (FloatingTextPrefab)
            {
                ShowFloatingText("+3");
            }
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
