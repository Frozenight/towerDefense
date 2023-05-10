using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class CustomInput : MonoBehaviour
{
    private static float TouchTime;
    public static Vector2 startPosition = Vector2.zero;

    public static bool GetOneTouchDown() {
        return Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Began;
    }

    public static bool TouchDown()
    {
        if (Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            Touch touch = Input.touches[0];
            startPosition = touch.position;
        }
        return Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Began;
    }

    public static bool GetHoldTouchDown()
    {        
        if (Input.touchCount == 1)
        {
            Touch touch = Input.GetTouch(0);

            
            if (touch.phase == TouchPhase.Began)
            {
                TouchTime = Time.time;
            }
            
            if (!CameraMovement.canMove)
            {
                if (Time.time - TouchTime >= 0.5 && Time.time - TouchTime <= 0.55)
                {
                    //Debug.Log("Press hold time: " + (Time.time - TouchTime));
                    return true;
                }
            }
        }
        return false;
    }

    public static bool GetOneTouchDrag()
    {
        return Input.touchCount == 1
        && (Input.GetTouch(0).phase & (TouchPhase.Moved | TouchPhase.Stationary)) != 0;
    }

    public static bool GetNoTouchOrTouchUp() {
        return Input.touchCount == 0 
            || (Input.touchCount == 1 
            && Input.GetTouch(0).phase 
                == UnityEngine.TouchPhase.Ended);
    }

    public static bool ClickedOnObject<T>(T script) {
        if (GetHoldTouchDown())
        {
            Vector3 mousePos = Input.mousePosition;
            RaycastHit _hit;
            Ray _ray = Camera.main.ScreenPointToRay(mousePos);
            if (Physics.Raycast(_ray, out _hit))
            {
                T tile = _hit.collider.GetComponent<T>();
                if (tile != null && tile.GetHashCode() == script.GetHashCode())
                    return true;                    
            }
        }
        return false;
    }
}