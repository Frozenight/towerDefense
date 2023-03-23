using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfterDelay : MonoBehaviour
{
    public float Seconds = 2f;
    public bool done;
    
    void Start(){
                StartCoroutine(DestroyObj());

    }
    void Update(){
        StartCoroutine(DestroyObj());
    }
 
    IEnumerator DestroyObj(){
        yield return new WaitForSeconds(Seconds);
        Destroy(gameObject);
        done = true;
    }
}
