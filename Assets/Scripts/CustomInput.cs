using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class CustomInput : MonoBehaviour
{
    public static bool GetOneTouchDown() {
        return Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Began;
    }

    public static bool GetOneTouchDrag() {
        return Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Moved;
    }

    public static bool GetNoTouchOrTouchUp() {
        return Input.touchCount == 0 
            || (Input.touchCount == 1 
            && Input.GetTouch(0).phase 
                == UnityEngine.TouchPhase.Ended);
    }

    public static bool ClickedOnObject<T>(T script) {
        if (GetOneTouchDown())
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