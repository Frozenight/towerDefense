using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DigitalRubyShared;
using TMPro;
public class CameraMovement : MonoBehaviour
{
    public float defaultDragXMin = -10f;
    public float defaultDragXMax = 50f;
    public float defaultDragZMin = -50f;
    public float defaultDragZMax = 10f;
    public float zoomYMin = 6f;
    public float zoomYMax = 30f;
    [HideInInspector] public bool disabled = false;
    [SerializeField] float maxY = 30f;
    [SerializeField] float testRotation = 10f;
    [SerializeField] float testZoomMult = 1.1f;
    [SerializeField] float testZoomMult2 = 0.9f;
    public TextMeshProUGUI testDebug;
    private ScaleGestureRecognizer scaleGesture;
    // private RotateGestureRecognizer rotateGesture;
    private Vector3 lastPos;
    private float zoomSpeed = 15f;
    private float rotY = 0f;
    private float sinY = 0f;
    private float cosY = 0f;
    private float currentDragXMin = 0f;
    private float currentDragXMax = 0f;
    private float currentDragZMin = 0f;
    private float currentDragZMax = 0f;
    private bool cameraDragOngoing = false;

    public static bool canMove = false;
    private float distanceThreshold = 100;
    private void Start()
    {
        transform.position = new Vector3(
            transform.position.x,
            maxY,
            transform.position.z
        );
        currentDragXMin = defaultDragXMin;
        currentDragXMax = defaultDragXMax;
        currentDragZMin = defaultDragZMin;
        currentDragZMax = defaultDragZMax;
        CreateScaleGesture();
        // uncomment for camera rotation
        // CreateRotateGesture();
        SetTrigonometricData();
        SetXZLimits();
    }

    void Update()
    {
        if (!disabled)
        {
            MoveByDrag();

            // for testing purposes
            if (Input.GetKeyDown(KeyCode.I))
            {
                ZoomCamera(1.2f);
            }
            else if (Input.GetKeyDown(KeyCode.O))
            {
                ZoomCamera(0.8f);
            }
        }
    }

    public void RotateCameraCaller()
    {
        RotateCamera(testRotation);
    }

    public void RotateCameraCaller2()
    {
        RotateCamera(-testRotation);
    }

    public void ZoomCameraCaller()
    {
        ZoomCamera(testZoomMult);
    }

    public void ZoomCameraCalle2()
    {
        ZoomCamera(testZoomMult2);
    }

    private void SetTrigonometricData()
    {
        rotY = transform.rotation.eulerAngles.y * Mathf.PI / 180f;
        sinY = Mathf.Sin(rotY);
        cosY = Mathf.Cos(rotY);
    }

    private void SetXZLimits()
    {
        float deltaY = maxY - transform.position.y;
        float deltaX = deltaY * sinY;
        float deltaZ = deltaY * cosY;
        currentDragXMax = defaultDragXMax + deltaX;
        currentDragXMin = defaultDragXMin + deltaX;
        currentDragZMax = defaultDragZMax + deltaZ;
        currentDragZMin = defaultDragZMin + deltaZ;
    }

    private void MoveByDrag()
    {
        if (CustomInput.TouchDown())
        {
            canMove = false;
            if (!cameraDragOngoing)
            {
                cameraDragOngoing = true;
                lastPos = Input.mousePosition;
            }
            return;
        }
        if (cameraDragOngoing)
        {
            if (!CustomInput.GetOneTouchDrag())
            {
                cameraDragOngoing = false;
                return;
            }
        }
        else return;

        Touch touch = Input.touches[0];
        Vector2 currentPosition = touch.position;
        //Debug.Log(CustomInput.startPosition + " " + currentPosition);
        Vector3 pos = Vector3.zero;
        if (Vector2.Distance(CustomInput.startPosition, currentPosition) > distanceThreshold || canMove)
        {
            canMove = true;
            pos = Camera.main.ScreenToViewportPoint(Input.mousePosition - lastPos);
        }
        float xS = pos.x * sinY;
        float xC = pos.x * cosY;
        float zS = pos.y * sinY;
        float zC = pos.y * cosY;
        Vector3 move = new Vector3(50f, 0, 50f);
        move.x *= xC + zS;
        move.z *= -xS + zC;
        transform.position = new Vector3(
            Mathf.Clamp(transform.position.x - move.x, currentDragXMin, currentDragXMax),
            transform.position.y,
            Mathf.Clamp(transform.position.z - move.z, currentDragZMin, currentDragZMax)
        );
        lastPos = Input.mousePosition;
    }

    private void ZoomCamera(float mult)
    {
        float newY = transform.position.y + zoomSpeed * (1 - mult);
        float clampedY = Mathf.Clamp(newY, zoomYMin, zoomYMax);
        // zooming too much in or out
        if (clampedY != newY)
            return;
        float newX = transform.position.x - zoomSpeed * (1 - mult) * sinY;
        float newZ = transform.position.z - zoomSpeed * (1 - mult) * cosY;
        transform.position = new Vector3(
            transform.position.x,
            clampedY,
            transform.position.z
        );
        SetXZLimits();
        newX = Mathf.Clamp(newX, currentDragXMin, currentDragXMax);
        newZ = Mathf.Clamp(newZ, currentDragZMin, currentDragZMax);
        transform.position = new Vector3(
            newX,
            transform.position.y,
            newZ
        );
    }

    private void RotateCamera(float angleEuler, bool debugging = false)
    {
        if (testRotation == 0)
            return;
        float currY = transform.position.y;
        Vector3 vectorB = new Vector3(
            currY * sinY,
            0f,
            currY * cosY
        );
        Vector3 centerPoint = vectorB + transform.position;
        float angleRadians = angleEuler * Mathf.PI / 180f;
        float magnitudeB = Mathf.Sqrt(currY * currY + 1);

        float sinNewAngle = Mathf.Sin(angleRadians);
        float tanNewAngle = Mathf.Sqrt(1 / (1 - sinNewAngle * sinNewAngle) - 1);
        if (angleEuler > -90)
        {
            if (angleEuler < 0)
                tanNewAngle *= -1;
        }
        else if (((angleEuler % 180) + 180) % 180 >= 90)
            tanNewAngle *= -1;

        float magnitudeA = tanNewAngle * magnitudeB;
        float magnitudeC = magnitudeA / sinNewAngle;
        Vector3 vectorA = new Vector3(magnitudeA * cosY, 0f, magnitudeA * -sinY);
        Vector3 newPosXZ = centerPoint - (vectorB - vectorA) * magnitudeB / magnitudeC;

        if (debugging)
        {
            Debug.Log($"Real: {Mathf.Tan(angleRadians)} | Optimised: {tanNewAngle} | angle: {angleEuler} | mod: {((angleEuler % 180) + 180) % 180}");
            Debug.DrawRay(transform.position, vectorB, Color.cyan);
            Debug.DrawRay(transform.position, vectorA, Color.red);
            Debug.DrawRay(transform.position + vectorA, vectorB - vectorA, Color.green);
            Debug.DrawRay(centerPoint, -(vectorB - vectorA) * magnitudeB / magnitudeC, Color.blue);
        }
        transform.Rotate(0f, -angleEuler, 0f, Space.World);
        SetTrigonometricData();
        SetXZLimits();
        newPosXZ.x = Mathf.Clamp(newPosXZ.x, currentDragXMin, currentDragXMax);
        newPosXZ.z = Mathf.Clamp(newPosXZ.z, currentDragZMin, currentDragZMax);
        transform.position = new Vector3(
            newPosXZ.x, currY, newPosXZ.z
        );
    }

    private void CreateScaleGesture()
    {
        scaleGesture = new ScaleGestureRecognizer();
        scaleGesture.StateUpdated += ScaleGestureCallback;
        FingersScript.Instance.AddGesture(scaleGesture);
    }

    // uncomment for camera rotation
    // private void CreateRotateGesture()
    // {
    //     rotateGesture = new RotateGestureRecognizer();
    //     rotateGesture.StateUpdated += RotateGestureCallback;
    //     FingersScript.Instance.AddGesture(rotateGesture);
    // }

    private void ScaleGestureCallback(GestureRecognizer gesture)
    {
        if (gesture.State == GestureRecognizerState.Executing)
        {
            ZoomCamera(scaleGesture.ScaleMultiplier);
        }
    }

    // uncomment for camera rotation 
    // private void RotateGestureCallback(GestureRecognizer gesture)
    // {
    //     if (gesture.State == GestureRecognizerState.Executing)
    //     {
    //         RotateCamera(-rotateGesture.RotationDegreesDelta);
    //     }
    // }
}
