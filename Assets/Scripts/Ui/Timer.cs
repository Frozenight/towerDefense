using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Timer : MonoBehaviour
{
    public static Timer instance { get; private set; }

    [SerializeField] TextMeshProUGUI timer_text;
    [SerializeField] TextMeshProUGUI timer;
    [SerializeField] enemySpawner spawner;
    [SerializeField] WallBuilder wallBuilder;

    float curreentTime = 0f;

    float buildingTime = 60f;
    float defenseTime = 20f;

    bool timerOn = false;

    [SerializeField] GameObject startButton;
    public GameObject GameBase;

    EventManager eventController;

    private void Start()
    {
        instance = this;
        EventManager.onRoundChange += ChangeSGameState;
        eventController = GetComponent<EventManager>();
        eventController.NewGame();
    }

    private void Update()
    {
        if (timerOn)
        {
            curreentTime -= Time.deltaTime;
            timer.text = curreentTime.ToString("0");
            if (curreentTime < 0)
            {
                timerOn = false;
                eventController.ChangeGameState();
            }
        }
    }
    private void ChangeSGameState()
    {
        if (eventController.currentState == EventManager.Event.preparation)
        {
            if(startButton.gameObject != null)
                startButton.gameObject.SetActive(true);
        }
        else if (eventController.currentState == EventManager.Event.building)
        {
            timer_text.text  = "Building Time: ";
            timer_text.fontSize = 26;
            curreentTime = buildingTime;
            timerOn = true;
            startButton.gameObject.SetActive(true);
        }
        else if (eventController.currentState == EventManager.Event.defending)
        {
            timer_text.text = "Defend!";
            timer_text.fontSize = 36;
            curreentTime = defenseTime;
            timerOn = false;
            timer.text = string.Empty;
            startButton.gameObject.SetActive(false);
            wallBuilder.HideWallSelection();
            if (GameBase.activeSelf)
            {
                spawner.spawnWave();
            }
        }
    }

    public void HideButton()
    {
        startButton.gameObject.SetActive(false);
    }
    public void ShowButton()
    {
        startButton.gameObject.SetActive(true);
    }

    public void CheckForEndOfRound()
    {
        if (GameObject.FindObjectsOfType<EnemyManager>().Length == 0)
        {
            eventController.ChangeGameState();
        }
    }

    private void OnDestroy()
    {
        EventManager.onRoundChange -= ChangeSGameState;
    }

    public void TutorialTimer(float time)
    {
        timer.text = time.ToString();
    }
}
