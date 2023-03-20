using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Show_BuildingSelection : MonoBehaviour
{
    private float xOffset = 220f;
    private float animationTime = 1f;
    private bool isHidden;

    [SerializeField] private RectTransform panel;
    [SerializeField] private Sprite hide_arrow;
    [SerializeField] private Sprite show_arrow;

    private void Start()
    {
        isHidden = false;
    }

    public void Change_Panel()
    {
        if (isHidden)
        {
            Show_Panel();
            isHidden = false;
        }
        else
        {
            Hide_Panel();
            isHidden = true;
        }
    }

    private void Show_Panel()
    {
        Vector3 newPosition = panel.anchoredPosition + new Vector2(xOffset, 0);
        StartCoroutine(SlideOverTime(panel.anchoredPosition, newPosition));
        panel.GetChild(1).GetComponent<Image>().sprite = hide_arrow;
    }

    private void Hide_Panel()
    {
        Vector3 newPosition = panel.anchoredPosition - new Vector2(xOffset, 0);
        StartCoroutine(SlideOverTime(panel.anchoredPosition, newPosition));
        panel.GetChild(1).GetComponent<Image>().sprite = show_arrow;
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
