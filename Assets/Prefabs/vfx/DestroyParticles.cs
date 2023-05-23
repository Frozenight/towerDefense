using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyParticles : MonoBehaviour
{
    private float t = 2;
    private void Update()
    {
        t -= Time.deltaTime;
        if (t < 0)
            Destroy(gameObject);
    }
}
