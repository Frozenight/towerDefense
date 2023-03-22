using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class CustomInput : MonoBehaviour
{
    public static bool GetOneTouchDown() {
        return Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Began;
    }

    public static bool GetNoTouchOrTouchUp() {
        return Input.touchCount == 0 
            || (Input.touchCount == 1 
            && Input.GetTouch(0).phase 
                == UnityEngine.TouchPhase.Ended);
    }

}