using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Building_Base : ManageableBuilding, IGameController
{
    public int maxHealth { get; set; }
    public GameOverScreen gameOverScreen;
    public GameObject tile;

    private const int HEALTH_INCREASE = 50;
    private float reverseArmorMult = 0f;

    public override string buildingName { 
        get { return NAME_BASE; }
    }

    public override bool canDestroyManually { 
        get { return false; }
    }

    public int _currentHealth { get; set; }

    public event Action<float> OnHealthChanged = delegate { };

    private void Start()
    {
        gameController = GameController.instance;
        SetArmorMultiplier();
    }

    public float healthRatio {
        get {
            return (float) _currentHealth / maxHealth;
        }
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
        if (gameController.resources < worker_upgrade_price)
            return;
        gameController.resources -= worker_upgrade_price;
        var workers = GameObject.FindGameObjectsWithTag("Worker");
        foreach (var w in workers)
            w.GetComponent<MovementAnimated>().Upgrade();
    }

    public void SetArmorMultiplier() {
        if (gameObject.tag != "Base") 
            return;
        List<float> values = GameController.instance.GetBonusValues(BonusType.BaseToughness);
        reverseArmorMult = 0f;
        foreach(var value in values) {
            reverseArmorMult += value;
        }
    }

    private void OnEnable()
    {
        _currentHealth = maxHealth;
    }

    public void ModifyHealth(int amount)
    {
        if (amount < 0 && gameObject.tag == "Base") {
            amount = (int)(amount * (1f - reverseArmorMult));
        }
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
                tile.GetComponent<TileOnWhichToPlace>().ChangePlacedState();
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
        if(gameObject.tag == "Base")
        {
            this.maxHealth = data.maxBaseHealth;
            _currentHealth = maxHealth;
        }
        if(gameObject.tag == "Tower")
        {
            this.maxHealth = data.towerHealth;
            _currentHealth = maxHealth;
        }
        if (gameObject.tag == "Wall")
        {
            this.maxHealth = data.wallHealth;
            _currentHealth = maxHealth;
        }

    }

    public void SaveData(ref GameData data)
    {
        if (gameObject.tag == "Base")
        {
            data.maxBaseHealth = this.maxHealth;
        }
        if (gameObject.tag == "Tower")
        {
            data.towerHealth = this.maxHealth;
        }
        if (gameObject.tag == "Wall")
        {
            data.wallHealth = this.maxHealth;
        }
        
    }

    public void TestIncreaseHp()
    {
        maxHealth += 100;
    }
}
