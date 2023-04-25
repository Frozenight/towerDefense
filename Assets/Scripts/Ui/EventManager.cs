using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventManager : MonoBehaviour
{
    public delegate void OnRoundChange();
    public static event OnRoundChange onRoundChange;
    public static UnityAction changeWorkerState;
    GameMode gameMode;

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
        gameMode = GameObject.FindObjectOfType<GameController>().GetComponent<GameMode>();
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
        if (ChangingToOrFromDefending(newState)) {
            changeWorkerState?.Invoke();
        }
        currentState = newState;
        onRoundChange?.Invoke();
        if (currentState == Event.defending)
        {
            gameMode.changeGameMode(5);
        }
    }

    private bool ChangingToOrFromDefending(Event newState) {
        return currentState == Event.defending || newState == Event.defending;
    }
}
