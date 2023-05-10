using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;

public class ManageableBuildingTab : MonoBehaviour
{
    private IEnumerator labelScaleAnim;
    private int animTime = 15; // 100 == 1 sec
    private float targetScale = 1.2f;

    [SerializeField] private Button WorkerB;
    [SerializeField] private Button UpgradeB;
    [SerializeField] private Button RemoveB;
    [SerializeField] private Button RotationL_B;
    [SerializeField] private Button RotationR_B;
    [SerializeField] private RectTransform NameAndLevel;
    [SerializeField] private TextMeshProUGUI NameText;
    [SerializeField] private TextMeshProUGUI LevelText;
    [SerializeField] private TextMeshProUGUI PriceText;
    [SerializeField] private TextMeshProUGUI Description;
    [SerializeField] public TextMeshProUGUI WorkerPrice;
    [SerializeField] public Button Fire_Turret;
    [SerializeField] public Button Frost_Turret;
    [SerializeField] public Button Earth_Turret;
    [SerializeField] public GameObject Turret_type;

    [SerializeField] public RectTransform Stats;
    [SerializeField] public GameObject SatsWindow;
    [SerializeField] private TextMeshProUGUI Damage;
    [SerializeField] private TextMeshProUGUI Range;
    [SerializeField] private TextMeshProUGUI AttackSpeed;
    [SerializeField] private TextMeshProUGUI DamageUpgrade;
    [SerializeField] private TextMeshProUGUI RangeUpgrade;
    [SerializeField] private TextMeshProUGUI AttackSpeedUpgrade;

    private ManageableBuilding assignedBuilding;
    private UnityAction m_closeTab;
    [SerializeField] private GameMode gameMode;

    public UnityAction closeTab {
        set {
            m_closeTab = value;
        }
    }

    public void FillBuildingData(ManageableBuilding mBuilding) {
        RemoveB.gameObject.SetActive(mBuilding.canDestroyManually);
        WorkerB.gameObject.SetActive(mBuilding.buildingName == ManageableBuilding.NAME_BASE);
        RotationL_B.gameObject.SetActive(mBuilding.tag == "Wall");
        RotationR_B.gameObject.SetActive(mBuilding.tag == "Wall");
        Turret_type.SetActive(mBuilding.buildingName == ManageableBuilding.NAME_TURRET);
        WorkerPrice.gameObject.SetActive(mBuilding.buildingName == ManageableBuilding.NAME_BASE);
        UpgradeB.gameObject.SetActive(mBuilding.buildingName != ManageableBuilding.NAME_WALL);
        PriceText.gameObject.SetActive(mBuilding.buildingName != ManageableBuilding.NAME_WALL);
        LevelText.gameObject.SetActive(mBuilding.buildingName != ManageableBuilding.NAME_WALL);

        if (mBuilding.level == 5 && mBuilding.buildingName == ManageableBuilding.NAME_TURRET)
        {
            UpgradeB.gameObject.SetActive(false);
            PriceText.gameObject.SetActive(false);
            SetInteractiveType(true);
        }

        Fire_Turret.gameObject.SetActive(mBuilding.buildingName == ManageableBuilding.NAME_TURRET);
        Frost_Turret.gameObject.SetActive(mBuilding.buildingName == ManageableBuilding.NAME_TURRET);
        Earth_Turret.gameObject.SetActive(mBuilding.buildingName == ManageableBuilding.NAME_TURRET);
        Stats.gameObject.SetActive(mBuilding.buildingName == ManageableBuilding.NAME_TURRET ||
                                   mBuilding.buildingName == ManageableBuilding.NAME_FIRE_TURRET ||
                                   mBuilding.buildingName == ManageableBuilding.NAME_FROST_TURRET ||
                                   mBuilding.buildingName == ManageableBuilding.NAME_EARTH_TURRET);
        if(mBuilding.buildingName == ManageableBuilding.NAME_TURRET ||
                                   mBuilding.buildingName == ManageableBuilding.NAME_FIRE_TURRET ||
                                   mBuilding.buildingName == ManageableBuilding.NAME_FROST_TURRET ||
                                   mBuilding.buildingName == ManageableBuilding.NAME_EARTH_TURRET)
        {
            UpdateStats(mBuilding.GetComponent<Turret>());
        }
        
        assignedBuilding = mBuilding;
        NameText.text = mBuilding.buildingName;
        LevelText.text = "Level: " + mBuilding.level;
        PriceText.text = "Upgrade price: " + mBuilding.upgrade_Price;
        if (assignedBuilding.level < 5)
            SetInteractiveType(false);
        if (GameController.instance.aiAPI.gameObjects.Length != 0)
        {
            Description.text = GameController.instance.aiAPI.gameObjects[0].text;
        }
        
    }

    private void SetInteractiveType(bool set)
    {
            Fire_Turret.interactable = set;
            Frost_Turret.interactable = set;
            Earth_Turret.interactable = set;
    }

    public void Upgrade() {
        if (assignedBuilding?.level != 5 && assignedBuilding?.buildingName == ManageableBuilding.NAME_TURRET)
        {
            assignedBuilding?.UpgradeBuilding();
            UpdateStats(assignedBuilding?.GetComponent<Turret>());
            UpdateBuildingStats();
        }
        else if(assignedBuilding?.buildingName != ManageableBuilding.NAME_TURRET)
        {
            assignedBuilding?.UpgradeBuilding();
            if (assignedBuilding?.buildingName == ManageableBuilding.NAME_FIRE_TURRET ||
                assignedBuilding?.buildingName == ManageableBuilding.NAME_FROST_TURRET ||
                assignedBuilding?.buildingName == ManageableBuilding.NAME_EARTH_TURRET)
            {
                UpdateStats(assignedBuilding?.GetComponent<Turret>());
            }
            UpdateBuildingStats();
        }
    }

    public void ChangeBiulding_EarthTurret()
    {
        assignedBuilding?.ChangeTypeEarth();
        m_closeTab?.Invoke();

    }

    public void ChangeBiulding_FireTurret()
    {
        assignedBuilding?.ChangeTypeFire();
        m_closeTab?.Invoke();
    }

    public void ChangeBiulding_FrostTurret()
    {
        assignedBuilding?.ChangeTypeFrost();
        m_closeTab?.Invoke();
    }

    public void UpgradeWorkers() {
        if (assignedBuilding is Building_Base) {
            (assignedBuilding as Building_Base).UpgradeWorkers();
            UpdateBuildingStats();
        }
    }

    public void RotateLeft()
    { 
        Vector3 left = new Vector3 (0, -1, 0);
        assignedBuilding.transform.Rotate(left * 1 * Time.deltaTime, 90);
    }

    public void RotateRight()
    {
        Vector3 right = new Vector3(0, 1, 0);
        assignedBuilding.transform.Rotate(right * 1 * Time.deltaTime, 90);
    }

    public void Destroy() {
        assignedBuilding?.gameObject.GetComponent<Building_Base>().tile.GetComponent<TileOnWhichToPlace>().ChangePlacedState();
        assignedBuilding?.DestroyBuilding();
        m_closeTab?.Invoke();
    }

    private void UpdateBuildingStats() {
        if(assignedBuilding.level == 5 && assignedBuilding.buildingName == ManageableBuilding.NAME_TURRET)
        {
            LevelText.text = "Level: " + assignedBuilding.level;
            PriceText.gameObject.SetActive(false);
            SetInteractiveType(true);
            return;
        }
        LevelText.text = "Level: " + assignedBuilding.level;
        PriceText.text = "Upgrade price: " + assignedBuilding.upgrade_Price;
            
    }

    IEnumerator LabelScaleAnim() {   
        int halfAnimTime = animTime / 2;
        for (int i = 1; i <= animTime; i++) {
            float delta = ((targetScale - 1f) / (halfAnimTime));
            if (i <= halfAnimTime) {
                delta *= i;
                NameAndLevel.localScale = new Vector3(1f + delta, 1f + delta, 1f);
            } else {
                delta *= (i-halfAnimTime);
                NameAndLevel.localScale = new Vector3(targetScale - delta, targetScale - delta, 1f);
            }
            yield return new WaitForSeconds(0.01f);
        }
    }

    public void ResetNamePulse() {
        StartCoroutine(labelScaleAnim);
        StartNamePulse();
    }

    private void StopNamePulse() {
        NameAndLevel.localScale = new Vector3(1f, 1f, 1f);
        StopCoroutine(labelScaleAnim);        
    }

    private void StartNamePulse() {
        NameAndLevel.localScale = new Vector3(1f, 1f, 1f);
        labelScaleAnim = LabelScaleAnim();
        StartCoroutine(labelScaleAnim);
    }

    private void OnEnable() {
        StartNamePulse();
    }

    private void OnDisable() {
        NameText.text = "";
        LevelText.text = "";
        assignedBuilding = null;
        StopNamePulse();
    }

    public void SetStatsActive()
    {
        SatsWindow.SetActive(true);
        Time.timeScale = 0;
        gameMode.changeGameMode(2);
    }

    public void CloseStats()
    {
        SatsWindow.SetActive(false);
        Time.timeScale = 1;
        gameMode.changeGameMode(1);
    }

    public void UpdateStats(Turret mBuilding)
    {
        Damage.text = "Damage: " + mBuilding.damage.ToString();
        Range.text = "Range: " + mBuilding.range.ToString();
        AttackSpeed.text = "Attack speed: " + mBuilding.fireRate.ToString("F2");
        DamageUpgrade.text = "Damage +" + mBuilding.GetUpgradeDamage().ToString();
        RangeUpgrade.text = "Range +" + mBuilding.GetUpgradeRange().ToString();
        AttackSpeedUpgrade.text = "Attack speed +" + (mBuilding.GetUpgradeAttackSpeed() * 100 - 100).ToString() + "%";
    }
}
