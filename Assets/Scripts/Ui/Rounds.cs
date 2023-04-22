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

    private int current_round = 0;

    private void Start()
    {
        EventManager.onRoundChange += NextRound;
        eventController = GetComponent<EventManager>();
    }

    public void NextRound()
    {
        if (eventController.currentState == EventManager.Event.defending)
        {
            current_round++;
            ChangeRoundText();
        }
        else
        {
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
