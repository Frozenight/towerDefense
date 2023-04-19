using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Image foregroundImage;
    [SerializeField] private float updateSpeedSeconds = 0.5f;

    private Image backgroundImage;

    private float fadeOutTime = 3f;
    private float fadeInTime = 1f;

    private void Awake()
    {
        if (transform.parent != null && transform.parent.tag == "Enemy")
        {
            GetComponentInParent<EnemyHealth>().OnHealthChanged += HandleHealthChanged;
        }
        else
        {
            GetComponentInParent<Building_Base>().OnHealthChanged += HandleHealthChanged;
        }
        backgroundImage = transform.GetChild(0).GetComponent<Image>();
    }

    private void Start()
    {
        StartCoroutine(FadeOut());
    }

    private void HandleHealthChanged(float pct)
    {
        if (this.isActiveAndEnabled)
        {
            StartCoroutine(ChangeToPct(pct));
            StartCoroutine(FadeIn());
        }
    }

    private IEnumerator ChangeToPct(float pct)
    {
        float preChangePct = foregroundImage.fillAmount;
        float elapsed = 0f;

        while (elapsed < updateSpeedSeconds)
        {
            elapsed += Time.deltaTime;
            foregroundImage.fillAmount = Mathf.Lerp(preChangePct, pct, elapsed / updateSpeedSeconds);
            yield return null;
        }
        foregroundImage.fillAmount = pct;
    }

    private void LateUpdate()
    {
        transform.LookAt(Camera.main.transform);
        transform.Rotate(0, 180, 0);
    }

    IEnumerator FadeOut()
    {
        float startTime = Time.time;
        Color originalColor1 = foregroundImage.color;
        Color originalColor2 = backgroundImage.color;
        while (Time.time < startTime + fadeOutTime)
        {
            float t = (Time.time - startTime) / fadeOutTime;
            foregroundImage.color = new Color(originalColor1.r, originalColor1.g, originalColor1.b, Mathf.Lerp(1, 0, t));
            backgroundImage.color = new Color(originalColor2.r, originalColor2.g, originalColor2.b, Mathf.Lerp(1, 0, t));
            yield return null;
        }
    }

    IEnumerator FadeIn()
    {
        foregroundImage.color = new Color(foregroundImage.color.r, foregroundImage.color.g, foregroundImage.color.b, 0f);
        backgroundImage.color = new Color(backgroundImage.color.r, backgroundImage.color.g, backgroundImage.color.b, 0f);
        float startTime = Time.time;
        Color originalColor = foregroundImage.color;
        Color originalColor1 = backgroundImage.color;
        while (Time.time < startTime + fadeInTime)
        {
            float t = (Time.time - startTime) / fadeInTime;
            foregroundImage.color = new Color(originalColor.r, originalColor.g, originalColor.b, Mathf.Lerp(0, 1, t));
            backgroundImage.color = new Color(originalColor1.r, originalColor1.g, originalColor1.b, Mathf.Lerp(0, 1, t));
            yield return null;
        }
    }
}
