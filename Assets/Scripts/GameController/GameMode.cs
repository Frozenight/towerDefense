using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMode : MonoBehaviour
{
    public bool isBiuldMode;
    public bool isDefendMode;
    int modeNumber;
    Show_BuildingSelection buildingSelection;
    CameraMovement cameraMovement;

    
    void Start()
    {
        buildingSelection = GameObject.Find("Hide/Show_Selection_button").GetComponent<Show_BuildingSelection>();
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
                if (buildingSelection.isHidden == false)
                    buildingSelection.Hide_Panel();
                playMode();
                break;
            case 2:
                buildMode();
                break;
            case 3:
                pauseMode();
                break;
            case 4:
                tabMode();
                break;
            //case 5:
            //    Debug.Log("Defend");
            //    defendMode();
            //    break;
            default:
                playMode();
                break;
        }
    }

    public void changeTabMode()
    {
        if (modeNumber == 4)
        {
            changeGameMode(1);
        }
    }
   
    private void playMode()
    {
        isDefendMode = false;
        isBiuldMode = false;
        if(cameraMovement != null)
            cameraMovement.enabled = true;
    }
    private void buildMode()
    {
        isDefendMode = false;
        isBiuldMode = true;
        cameraMovement.enabled = false;
        
    }
    private void pauseMode()
    {
        isDefendMode = false;
        isBiuldMode = false;
        cameraMovement.enabled = false;
    }
    private void tabMode()
    {
        isDefendMode = false;
        isBiuldMode = false;
        if (buildingSelection.isHidden == false)
        {
            buildingSelection.Hide_Panel();
        }
        cameraMovement.enabled = false;
    }
    //private void defendMode()
    //{
    //    isDefendMode = true;
    //    isBiuldMode = false;
    //    //if (buildingSelection.isHidden == false)
    //    //{
    //    //    buildingSelection.Hide_Panel();
    //    //}
    //    cameraMovement.enabled = true;
    //}
}
