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
    [SerializeField] enemySpawner enemySpawner;

    public int pickUpWave = 2;
    public PickUpScreen pickUpScreen;

    private void Start()
    {
        current_round = PlayerPrefs.GetInt("CurrentRound", 0);  // Load the round

        EventManager.onRoundChange += NextRound;
        eventController = GetComponent<EventManager>();
    }

    public void NextRound()
    {
        if (eventController.currentState == EventManager.Event.defending)
        {
            gameMode.changeGameMode(4);
            current_round++;

            ChangeRoundText();
        }
        else
        {
            PlayerPrefs.SetInt("CurrentRound", current_round);  // Save the round
            gameMode.changeGameMode(5);
            if (current_round == pickUpWave) { pickUpScreen.Setup(); pickUpWave += 5; }
            if (((enemySpawner.completedWaves - 4) % 10 == 0))
            { GameController.instance.BossWarning(); }
            ChangeDefendingText();
        }
    }

    public void NewGame()
    {
        current_round = 0;
        PlayerPrefs.SetInt("CurrentRound", current_round); // Reset the round when new game
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
