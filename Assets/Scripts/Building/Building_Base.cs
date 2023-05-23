using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Building_Base : ManageableBuilding, IGameController
{
    public new const int m_typeIndex =0;
    
    public int maxHealth;
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

    public int currentHealth;

    public event Action<float> OnHealthChanged = delegate { };

    private int m_workerLevel = 1;

    private void Start()
    {
        if (currentHealth == 0) {
            currentHealth = maxHealth;
        }
        gameController = GameController.instance;
        SetArmorMultiplier();
    }

    public float healthRatio {
        get {
            return (float) currentHealth / maxHealth;
        }
    }

    /// <summary>
    /// GetExportedData method override. Call this method only from the main base object.
    /// </summary>
    /// <returns>Exported tower data.</returns>
    public override BuildingData GetExportedData() {
        return new BuildingMainBaseData(
            -1,
            m_level,
            buildingPrice,
            m_workerLevel,
            m_typeIndex,
            currentHealth,
            maxHealth
        );
    }

    public virtual void ImportSessionData(BuildingData data, bool thisIsMainBase) {
        currentHealth = data.HealthCurrent;
        maxHealth = data.HealthMax;
        ModifyHealth(0);
        if (!thisIsMainBase)
            return;
        m_level = data.Level;
        m_upgrade_price = 5 * m_level;
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
        ModifyHealth(maxHealth - currentHealth);
    }

    public void UpgradeWorkers()
    {
        if (gameController.resources < worker_upgrade_price)
            return;
        gameController.resources -= worker_upgrade_price;
        UpgradeEachWorker();
        m_workerLevel++;
    }

    private static void UpgradeEachWorker()
    {
        var workers = GameObject.FindGameObjectsWithTag("Worker");
        foreach (var w in workers)
            w.GetComponent<MovementAnimated>().Upgrade();
    }

    public void UpdateWorkerSpeed(float speed) {
        var workers = GameObject.FindGameObjectsWithTag("Worker");
        foreach (var w in workers)
            w.GetComponent<MovementAnimated>().SetSpeed(speed);
    }

    public float GetWorkerSpeed {
        get {
            return GameObject.FindGameObjectWithTag("Worker")
                .GetComponent<MovementAnimated>().GetSavedSpeed;
        }
    }

    public void SetArmorMultiplier() {
        if (gameObject.tag != "Base") 
            return;
        List<float> values = GameController.instance.GetBonusValues(BonusType.BaseToughness);
        reverseArmorMult = 0f;
        foreach(var value in values) {
            reverseArmorMult += value;
        }
        Debug.Log("Reverse armor mult: " + reverseArmorMult);
    }

    public void ModifyHealth(int amount)
    {
        if (amount < 0 && gameObject.tag == "Base") {
            amount = (int)(amount * (1f - reverseArmorMult));
        }
        currentHealth += amount;

        float currentHealthPct = (float)currentHealth / (float)maxHealth;
        OnHealthChanged(currentHealthPct);
        CheckForDeath();
    }

    public void CheckForDeath()
    {
        if (currentHealth <= 0)
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
        if (gameObject.tag == "Base")
        {
            maxHealth = data.maxBaseHealth;
            currentHealth = maxHealth;
            UpdateWorkerSpeed(data.workerSpeed);
        }
    }

    public void SaveData(ref GameData data)
    {
        if (gameObject.tag == "Base")
        {
            data.maxBaseHealth = maxHealth;
            data.workerSpeed = GetWorkerSpeed;
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

    public void IncreaseHp()
    {
        maxHealth += 10;
    }
}
