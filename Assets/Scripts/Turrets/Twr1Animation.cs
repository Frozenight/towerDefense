using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Twr1Animation : MonoBehaviour
{

    //state indexes
    //Idle - 1
    //Shooting - 2

    private Turret turretScript;
    private Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        turretScript = GetComponent<Turret>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(turretScript.IsShooting);
        if (turretScript.IsShooting)
        {
            anim.SetBool("IsShooting", true);
            anim.SetLayerWeight(2, 1);
            anim.SetLayerWeight(1, 0);
        }
        else
        {
            anim.SetLayerWeight(1, 1);
            anim.SetLayerWeight(2, 0);
        }
    }
}
