using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMode : MonoBehaviour
{
    public bool isDefendMode;
    int modeNumber;
    CameraMovement cameraMovement;

    
    void Start()
    {
        cameraMovement = Camera.main.GetComponent<CameraMovement>();
        playMode();
    }

    //Play mode and Build mode depend on the numder given,
    // if more modes are added another number and transition should be added as well
    public void changeGameMode(int a)
    {
        modeNumber = a;
        switch (a)
        {
            case 1:
                playMode();
                break;
            case 2:
                pauseMode();
                break;
            case 3:
                tabMode();
                break;
            case 4:
                Debug.Log("Defend");
                defendMode();
                break;
            case 5:
                notDefendMode();
                break;
            default:
                playMode();
                break;
        }
    }
   
    private void playMode()
    {
        if(cameraMovement != null)
            cameraMovement.enabled = true;
    }
    private void pauseMode()
    {
        cameraMovement.enabled = false;
    }
    private void tabMode()
    {
        cameraMovement.enabled = false;
    }
    private void defendMode()
    {
        isDefendMode = true;
    }
    private void notDefendMode()
    {
        isDefendMode = false;
    }
}
