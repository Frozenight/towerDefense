using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Rounds : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI rounds_text;
    [SerializeField] TextMeshProUGUI rounds;
    EventManager eventController;
    [SerializeField] GameMode gameMode;
    public int current_round = 0;
    [SerializeField] Button hide_button;
    [SerializeField] Show_BuildingSelection show_BuildingSelection;

    private void Start()
    {
        EventManager.onRoundChange += NextRound;
        eventController = GetComponent<EventManager>();
    }

    public void NextRound()
    {
        if (eventController.currentState == EventManager.Event.defending)
        {
            Debug.Log("TEST AR VEIKIA");
            gameMode.changeGameMode(4);
            current_round++;
            ChangeRoundText();
        }
        else
        {
            gameMode.changeGameMode(5);
            if (current_round == enemySpawner.instance.bossWave-1) { GameController.instance.BossWarning(); }
            ChangeDefendingText();
        }
    }

    public void NewGame()
    {
        eventController.currentState = EventManager.Event.preparation;
    }

    private void ChangeRoundText()
    {
        rounds.text = current_round.ToString();
        rounds_text.text = "Round: ";
    }

    private void ChangeDefendingText()
    {
        rounds.text = "";
        rounds_text.text = "Prepare!";
    }
}
