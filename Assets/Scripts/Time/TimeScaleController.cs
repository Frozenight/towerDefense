using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeScaleController : MonoBehaviour
{
    private void SetTimeScale(float scale){
        Time.timeScale = scale;
        Time.fixedDeltaTime = scale*.02f;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Equals)){
            SetTimeScale(.5f);
        }
        if(Input.GetKeyDown(KeyCode.Minus)){
            SetTimeScale(.5f);
        }
        if(Input.GetKeyDown(KeyCode.Space)){
            SetTimeScale(0f);
        }
    }
}