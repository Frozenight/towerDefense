using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    public delegate void OnRoundChange();
    public static event OnRoundChange onRoundChange;

    public Event currentState;
    public enum Event
    {
        preparation,
        building,
        defending
    }

    public void NewGame()
    {
        UpdateCurrentState(Event.preparation);
    }

    public void ChangeGameState()
    {
        if (currentState == Event.preparation)
            UpdateCurrentState(Event.defending);
        else if (currentState == Event.building)
            UpdateCurrentState(Event.defending);
        else
            UpdateCurrentState(Event.building);
    }

    private void UpdateCurrentState(Event newState)
    {
        currentState = newState;
        onRoundChange?.Invoke();
    }
}
