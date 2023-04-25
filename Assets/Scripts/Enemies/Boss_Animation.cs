using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_Animation : MonoBehaviour
{
    private Animator anim;
    // Start is called before the first frame update
    IEnumerator Start()
    {
        anim = GetComponent<Animator>();


        while (true)
        {
            yield return new WaitForSeconds(1f);
            anim.SetInteger("AttackIndex", Random.Range(0, 3));
        }
    }

}
