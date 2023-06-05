using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashUI : MonoBehaviour
{
    public RectTransform targetElement; // UI element to slide
    public float slideSpeed = 2.0f; // speed of sliding
    private Vector2 initialPosition; // initial position of the UI element

    [SerializeField] private ResourceSpawner resourceSpawner;

    void Start()
    {
        if (targetElement == null)
        {
            Debug.LogError("Target element is not set");
            return;
        }
        initialPosition = targetElement.anchoredPosition;
    }

    public void SlideOut()
    {
        if (!resourceSpawner.AllTrashPickedUP())
            return;
        StartCoroutine(Slide(targetElement, new Vector2(-targetElement.rect.width, targetElement.anchoredPosition.y), slideSpeed));
    }

    public void SlideIn()
    {
        StartCoroutine(Slide(targetElement, initialPosition, slideSpeed));
    }

    IEnumerator Slide(RectTransform target, Vector2 endPosition, float speed)
    {
        while (Vector2.Distance(target.anchoredPosition, endPosition) > 0.1f)
        {
            target.anchoredPosition = Vector2.Lerp(target.anchoredPosition, endPosition, Time.deltaTime * speed);
            yield return null;
        }
        target.anchoredPosition = endPosition; // ensure exact position
    }
}
