using System;
using UnityEngine;
using System.Collections;

public class Building_Base : ManageableBuilding
{
    private const int HEALTH_INCREASE = 25;

    public override string buildingName { 
        get { return "Recycling Centre"; }
    }

    public override bool canDestroyManually { 
        get { return false; }
    }

    [SerializeField] private int _maxHealth = 100;

    private int _currMaxHealth;
    private int _currentHealth;

    public event Action<float> OnHealthChanged = delegate { };

    public override void UpgradeBuilding()
    {
        m_level += 1;
        _currMaxHealth += HEALTH_INCREASE;
        // set health to new max value
        ModifyHealth(_currMaxHealth - _currentHealth);
    }

    private void OnEnable()
    {
        _currMaxHealth = _currentHealth = _maxHealth;
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
