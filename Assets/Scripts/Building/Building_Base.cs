using System;
using UnityEngine;
using System.Collections;

public class Building_Base : ManageableBuilding, IGameController
{
    public int maxHealth { get; set; }
    public GameOverScreen gameOverScreen;

    private const int HEALTH_INCREASE = 50;

    public override string buildingName { 
        get { return NAME_BASE; }
    }

    public override bool canDestroyManually { 
        get { return false; }
    }

    private int _currentHealth;

    public event Action<float> OnHealthChanged = delegate { };

    private void Start()
    {
        gameController = GameController.instance;
        _currentHealth = maxHealth;
    }

    public override void UpgradeBuilding()
    {
        // not enough recourses
        if (gameController.resources < m_upgrade_price)
            return;
        gameController.resources -= m_upgrade_price;
        m_level += 1;
        m_upgrade_price += 5;
        maxHealth += HEALTH_INCREASE;
        // set health to new max value
        ModifyHealth(maxHealth - _currentHealth);
    }

    public void UpgradeWorkers() {
        var workers = GameObject.FindGameObjectsWithTag("Worker");
        foreach (var w in workers)
            w.GetComponent<MovementAnimated>().Upgrade();
    }

    private void OnEnable()
    {
        _currentHealth = maxHealth;
    }

    public void ModifyHealth(int amount)
    {
        _currentHealth += amount;

        float currentHealthPct = (float)_currentHealth / (float)maxHealth;
        OnHealthChanged(currentHealthPct);
        CheckForDeath();
    }

    public void CheckForDeath()
    {
        if (_currentHealth <= 0)
        {
            if (GetComponent<Turret>() != null || GetComponent<Wall>() != null)
            {
                Destroy(gameObject);
            }
            else
                GameController.instance.GameOver();
                gameObject.SetActive(false);
        }
    }



    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy")
            ModifyHealth(-10);
    }

    public void LoadData(GameData data)
    {
        this.maxHealth = data.maxHealth;
        _currentHealth = maxHealth;
    }

    public void SaveData(ref GameData data)
    {
        data.maxHealth = this.maxHealth;
    }

    public void TestIncreaseHp()
    {
        maxHealth += 100;
    }
}
