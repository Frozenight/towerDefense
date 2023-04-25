using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DigitalRubyShared;
using TMPro;
public class CameraMovement : MonoBehaviour
{
    public float swipeXMin = -10f;
    public float swipexMax = 50f;
    public float swipeZMin = -50f;
    public float swipeZMax = 10f;
    public float zoomYMin = 6f;
    public float zoomYMax = 30f;

    public bool disabled = false;
    private ScaleGestureRecognizer scaleGesture;
    private Vector3 lastPos;
    private float zoomSpeed = 15f;
    private void Start() {
        CreateScaleGesture();
    }

    void Update()
    {
        if (!disabled)
        {
            MoveByDrag();
        }
    }

    private void MoveByDrag()
    {
        if (CustomInput.GetOneTouchDown())
        {
            lastPos = Input.mousePosition;
            return;
        }

        if (Input.touchCount > 1 || CustomInput.GetNoTouchOrTouchUp()) return;

        Vector3 pos = Camera.main.ScreenToViewportPoint(Input.mousePosition - lastPos);
        float xS = pos.x * 0.707f;
        float yS = pos.y * 0.707f;
        Vector3 move = new Vector3(50f, 0, 50f);
        move.x *= yS - xS;
        move.z *= -xS - yS;
        transform.position = new Vector3(
            Mathf.Clamp(transform.position.x + move.x, swipeXMin, swipexMax),
            transform.position.y,
            Mathf.Clamp(transform.position.z + move.z, swipeZMin, swipeZMax)
        );
        lastPos = Input.mousePosition;
    }

    private void ScaleGestureCallback(GestureRecognizer gesture)
        {
            if (gesture.State == GestureRecognizerState.Executing)
            {
                float newY = transform.position.y + zoomSpeed*(1-scaleGesture.ScaleMultiplier)*1.414f;
                float clampedY = Mathf.Clamp(newY, zoomYMin, zoomYMax);
                // zoomed too much in or out
                if (clampedY != newY)
                    return;
                transform.position = new Vector3(
                    transform.position.x + zoomSpeed*(1-scaleGesture.ScaleMultiplier),
                    newY,
                    transform.position.z - zoomSpeed*(1-scaleGesture.ScaleMultiplier)
                );
                // Debug.Log($"Scaled: {scaleGesture.ScaleMultiplier}, Focus: {scaleGesture.FocusX}, {scaleGesture.FocusY}");
            }
        }

    private void CreateScaleGesture()
    {
        scaleGesture = new ScaleGestureRecognizer();
        scaleGesture.StateUpdated += ScaleGestureCallback;
        FingersScript.Instance.AddGesture(scaleGesture);
    }
}
