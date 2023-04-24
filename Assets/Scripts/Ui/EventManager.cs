using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventManager : MonoBehaviour
{
    public delegate void OnRoundChange();
    public static event OnRoundChange onRoundChange;
    public static UnityAction changeWorkerState;

    [SerializeField] GameController gameController;
    [SerializeField] ResourceSpawner resourceSpawner;

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
        if (newState == Event.building)
        {
            Debug.Log("Calling Spawning");
            resourceSpawner.bagsPerRound = 0;
            resourceSpawner.StartNewSpawn();
        }
        if (ChangingToOrFromDefending(newState)) {
            changeWorkerState?.Invoke();
        }
        currentState = newState;
        onRoundChange?.Invoke();
    }

    private bool ChangingToOrFromDefending(Event newState) {
        return currentState == Event.defending || newState == Event.defending;
    }
}
