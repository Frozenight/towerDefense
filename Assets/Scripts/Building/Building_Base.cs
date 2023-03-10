using System;
using UnityEngine;
using System.Collections;

public class Building_Base : MonoBehaviour
{
    [SerializeField] private int _maxHealth = 100;

    private int _currentHealth;

    public event Action<float> OnHealthChanged = delegate { };

    private void OnEnable()
    {
        _currentHealth = _maxHealth;
    }

    public void ModifyHealth(int amount)
    {
        _currentHealth += amount;

        float currentHealthPct = (float)_currentHealth / (float)_maxHealth;
        OnHealthChanged(currentHealthPct);
        CheckForDeath();
    }

    public void CheckForDeath()
    {
        if (_currentHealth <= 0)
            StartCoroutine(destroyItself());
    }

    private IEnumerator destroyItself()
    {
        yield return new WaitForSeconds(0.5f);
        Destroy(gameObject);
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy")
            ModifyHealth(-10);
    }
}
