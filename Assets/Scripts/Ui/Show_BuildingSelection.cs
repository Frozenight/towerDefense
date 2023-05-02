using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Show_BuildingSelection : MonoBehaviour
{
    private float xOffset = 420f;
    private float animationTime = .3f;
    public bool isHidden;
    private GameMode gameMode;
    [SerializeField] private RectTransform panel;
    [SerializeField] private RectTransform buttonRect;
    [SerializeField] private Image buttonImage;
    [SerializeField] private Sprite hide_arrow;
    [SerializeField] private Sprite show_arrow;
    private Vector3 targetPos;
    

    private void Start()
    {
        Invoke("DelayedStart", .01f);
    }

    private void DelayedStart() 
    {
        targetPos = panel.anchoredPosition;
        xOffset = panel.rect.width - buttonRect.rect.width;
        HidePanelInstant();
        gameMode = FindObjectOfType<GameController>().GetComponent<GameMode>();
        TileOnWhichToPlace.SetBoundaries(panel.GetComponent<RectTransform>());
    }

    public void Change_Panel()
    {
        if (isHidden)
        {
            Show_Panel();
            gameMode.changeGameMode(2);
        }
        else
        {
            Hide_Panel();
            gameMode.changeGameMode(1);
        }
    }

    private void Show_Panel()
    {
        Debug.Log("Show");
        targetPos = targetPos + new Vector3(xOffset, 0, 0);
        StartCoroutine(SlideOverTime(panel.anchoredPosition, targetPos));
        buttonImage.sprite = show_arrow;
        isHidden = false;
    }

    public void Hide_Panel()
    {
        Debug.Log("Hide");
        if (isHidden)
            return;
        targetPos = targetPos - new Vector3(xOffset, 0, 0);
        StartCoroutine(SlideOverTime(panel.anchoredPosition, targetPos));
        buttonImage.sprite = hide_arrow;
        isHidden = true;
    }

    private void HidePanelInstant() {
        if (isHidden)
            return;
        targetPos = targetPos - new Vector3(xOffset, 0, 0);
        panel.anchoredPosition = targetPos;
        buttonImage.sprite = hide_arrow;
        isHidden = true;
    }

    IEnumerator SlideOverTime(Vector3 initialPosition, Vector3 newPosition)
    {
        float elapsedTime = 0;

        while (elapsedTime < animationTime)
        {
            panel.anchoredPosition = Vector2.Lerp(initialPosition, newPosition, elapsedTime / animationTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        panel.anchoredPosition = newPosition;
        TileOnWhichToPlace.SetBoundaries(panel.GetComponent<RectTransform>());
    }
}
