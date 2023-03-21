using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public float xMin = -10f;
    public float xMax = 50f;
    public float zMin = -50f;
    public float zMax = 10f;
    private Vector3 lastPos;

    void Update()
    {
        MoveByDrag();
    }

    private void MoveByDrag()
    {
        if (Input.GetMouseButtonDown(0))
        {
            lastPos = Input.mousePosition;
            return;
        }

        if (!Input.GetMouseButton(0)) return;

        Vector3 pos = Camera.main.ScreenToViewportPoint(Input.mousePosition - lastPos);
        float xS = pos.x * 0.707f;
        float yS = pos.y * 0.707f;
        Vector3 move = new Vector3(50f, 0, 50f);
        move.x *= yS - xS;
        move.z *= -xS - yS;
        transform.position = new Vector3(
            Mathf.Clamp(transform.position.x + move.x, xMin, xMax),
            transform.position.y,
            Mathf.Clamp(transform.position.z + move.z, zMin, zMax)
        );
        lastPos = Input.mousePosition;
    }
}
