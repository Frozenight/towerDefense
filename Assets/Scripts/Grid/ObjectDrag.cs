using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectDrag : MonoBehaviour
{
    private Vector3 offset;
    private bool dragIsOngoing = false;


    private void Update() {
        if (CustomInput.ClickedOnObject(this)) {
            StartMoving();
        } else if (CustomInput.GetNoTouchOrTouchUp()) {
            dragIsOngoing = false;
            return;
        }
        if (dragIsOngoing)
            MovePos();
    }

    private void StartMoving()
    {
        dragIsOngoing = true;
        offset = transform.position - BuildingSystem.GetMouseWorldPosition();
    }

    private void MovePos()
    {
        Vector3 pos = BuildingSystem.GetMouseWorldPosition() + offset;
        transform.position = BuildingSystem.current.SnapCoordinateToGrid(pos);
    }
}
