using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Show_BuildingSelection : MonoBehaviour
{
    private float xOffset = 420f;
    private float animationTime = 1f;
    public bool isHidden;
    private GameMode gameMode;
    [SerializeField] private RectTransform panel;
    [SerializeField] private Sprite hide_arrow;
    [SerializeField] private Sprite show_arrow;

    private void Start()
    {
        isHidden = true;
        gameMode = FindObjectOfType<GameController>().GetComponent<GameMode>();
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
        Vector3 newPosition = panel.anchoredPosition + new Vector2(xOffset, 0);
        StartCoroutine(SlideOverTime(panel.anchoredPosition, newPosition));
        panel.GetChild(1).GetComponent<Image>().sprite = hide_arrow;
        isHidden = false;
    }

    public void Hide_Panel()
    {
        Vector3 newPosition = panel.anchoredPosition - new Vector2(xOffset, 0);
        StartCoroutine(SlideOverTime(panel.anchoredPosition, newPosition));
        panel.GetChild(1).GetComponent<Image>().sprite = show_arrow;
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
    }
}
