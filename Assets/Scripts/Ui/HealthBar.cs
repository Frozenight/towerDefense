using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Image foregroundImage;
    [SerializeField] private Image foregroundImageDelayed;
    [SerializeField] private float updateSpeedSeconds = 0.5f;

    private Image backgroundImage;

    private float fadeOutTime = 3f;
    private float fadeInTime = 1f;

    private Coroutine fadeOutCoroutine; // declare a variable to store the running fadeOut coroutine

    bool hit = false;

    private float healthPerFrame = 200f;

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
        fadeOutCoroutine = StartCoroutine(FadeOut());
    }

    private void Update()
    {
        ShowDelayedDamage();
    }

    private void HandleHealthChanged(float pct)
    {
        if (this.isActiveAndEnabled)
        {
            StartCoroutine(ChangeToPct(pct));
            if (!hit)
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


    private void ShowDelayedDamage()
    {
        float difference = foregroundImageDelayed.fillAmount - foregroundImage.fillAmount;
        if (difference > 0)
        {
            foregroundImageDelayed.fillAmount -= healthPerFrame * Time.deltaTime * (difference/100);
        }
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
        Color originalColor3 = foregroundImageDelayed.color;
        while (Time.time < startTime + fadeOutTime)
        {
            float t = (Time.time - startTime) / fadeOutTime;
            float alpha = Mathf.Lerp(1, 0f, t);
            foregroundImage.color = new Color(originalColor1.r, originalColor1.g, originalColor1.b, alpha);
            backgroundImage.color = new Color(originalColor2.r, originalColor2.g, originalColor2.b, alpha);
            foregroundImageDelayed.color = new Color(originalColor3.r, originalColor3.g, originalColor3.b, alpha);
            yield return null;
        }
    }

    IEnumerator FadeIn()
    {
        hit = true;
        // if there's a running fadeOut coroutine, stop it
        if (fadeOutCoroutine != null)
        {
            StopCoroutine(fadeOutCoroutine);
        }
        foregroundImage.color = new Color(foregroundImage.color.r, foregroundImage.color.g, foregroundImage.color.b, 0f);
        backgroundImage.color = new Color(backgroundImage.color.r, backgroundImage.color.g, backgroundImage.color.b, 0f);
        foregroundImageDelayed.color = new Color(foregroundImageDelayed.color.r, foregroundImageDelayed.color.g, foregroundImageDelayed.color.b, 0f);
        float startTime = Time.time;
        Color originalColor = foregroundImage.color;
        Color originalColor1 = backgroundImage.color;
        Color originalColor2 = foregroundImageDelayed.color;
        while (Time.time < startTime + fadeInTime)
        {
            float t = (Time.time - startTime) / fadeInTime;
            foregroundImage.color = new Color(originalColor.r, originalColor.g, originalColor.b, Mathf.Lerp(0, 1, t));
            backgroundImage.color = new Color(originalColor1.r, originalColor1.g, originalColor1.b, Mathf.Lerp(0, 1, t));
            foregroundImageDelayed.color = new Color(originalColor2.r, originalColor2.g, originalColor2.b, Mathf.Lerp(0, 1, t));
            yield return null;
        }
    }
}
