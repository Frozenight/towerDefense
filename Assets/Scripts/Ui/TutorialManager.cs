using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;

public class TutorialManager : MonoBehaviour
{
    public GameObject[] tutorials;
    public Canvas MainUi;
    [SerializeField] private Canvas tip;
    private Timer timer;
    [SerializeField] private Building_Base Base;
    private float delayBeforeAttack;
    private float time;
    private int index;
    private float time2;

    private EventManager eventManager;

    // Start is called before the first frame update
    void Start()
    {
        timer = MainUi.GetComponent<Timer>();
        eventManager = MainUi.GetComponent<EventManager>();
        
        index = 0;
        if(!PlayerPrefs.HasKey("firstTime"))
        {
            PlayerPrefs.SetInt("firstTime", 0);
        }
        if (PlayerPrefs.HasKey("firstTime") && PlayerPrefs.GetInt("firstTime") < 1)
        {
            PlayerPrefs.SetString("twr2Unlocked", "false");
        }
        if (PlayerPrefs.HasKey("firstTime") && PlayerPrefs.GetInt("firstTime") > 1)
            this.gameObject.SetActive(false);
        else if (PlayerPrefs.HasKey("firstTime") && PlayerPrefs.GetInt("firstTime") != 1)
        {
            timer.HideButton();
            PlayerPrefs.SetInt("firstTime", PlayerPrefs.GetInt("firstTime")+1);
            PlayerPrefs.Save();
            delayBeforeAttack = 3f;
            time = 0;
            NextTutorial();
        }
        
        
    }

    // Update is called once per frame
    void Update()
    {
        if(MainUi.GetComponent<Rounds>().current_round == 1 && eventManager.currentState == EventManager.Event.building && index == 3) { NextTutorial(); }
        switch (index)
        {
            case 1:
                timer.HideButton();
                Base.GetComponent<Outline>().enabled = true;
                if (ManageableBuilding.selectedBuilding != null && ManageableBuilding.selectedBuilding.tag == "Base")
                {
                    NextTutorial();
                }
                break;
            case 2:
                timer.HideButton();
                time += Time.deltaTime;
                if(time > delayBeforeAttack) { NextTutorial(); time = 0; time2 = 25; }
                break;
            case 3:
                timer.HideButton();
                //show tip, hide after 15sec
                tip.gameObject.SetActive(true);
                time += Time.deltaTime;
                if (time > 20) { tip.gameObject.SetActive(false);}
                //update prep time, start wave after 25sec
                if(eventManager.currentState == EventManager.Event.preparation)
                    PreparationTime();
                break;
            case 4:
                this.gameObject.SetActive(false);
                break;
        }
    }

    public void NextTutorial()
    {
        Time.timeScale = 0;
        tutorials[index].SetActive(true);
    }
    public void EndTutorial()
    {
        tutorials[index].SetActive(false);
        Time.timeScale = 1;
        index++;
        if (index == tutorials.Length)
        {
            Debug.Log("Show Ready");
            timer.ShowButton();
        }

    }

    private void PreparationTime()
    {
        time2 -= Time.deltaTime;
        timer.TutorialTimer(Mathf.Round(time2 * 1f));
        if (time2 <= 0)
        {
            eventManager.ChangeGameState();
        }
         
    }
}
