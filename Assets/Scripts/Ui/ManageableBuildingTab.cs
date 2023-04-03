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
    [SerializeField] private RectTransform NameAndLevel;
    [SerializeField] private TextMeshProUGUI NameText;
    [SerializeField] private TextMeshProUGUI LevelText;
    [SerializeField] private TextMeshProUGUI Description;
    [SerializeField] public Button Fire_Turret;
    [SerializeField] public Button Frost_Turret;
    [SerializeField] public Button Earth_Turret;
    private ManageableBuilding assignedBuilding;
    private UnityAction m_closeTab;


    public UnityAction closeTab {
        set {
            m_closeTab = value;
        }
    }

    public void FillBuildingData(ManageableBuilding mBuilding) {
        RemoveB.gameObject.SetActive(mBuilding.canDestroyManually);
        WorkerB.gameObject.SetActive(
            mBuilding.buildingName == ManageableBuilding.NAME_BASE);
        Fire_Turret.gameObject.SetActive(mBuilding.buildingName == ManageableBuilding.NAME_TURRET);
        Frost_Turret.gameObject.SetActive(mBuilding.buildingName == ManageableBuilding.NAME_TURRET);
        Earth_Turret.gameObject.SetActive(mBuilding.buildingName == ManageableBuilding.NAME_TURRET);
        assignedBuilding = mBuilding;
        NameText.text = mBuilding.buildingName;
        LevelText.text = "Level: " + mBuilding.level;
        if (GameController.instance.aiAPI.gameObjects.Length != 0)
        {
            Description.text = GameController.instance.aiAPI.gameObjects[0].text;
        }
        if (assignedBuilding.level < 5)
            SetInteractiveType(false);
    }

    private void SetInteractiveType(bool set)
    {
            Fire_Turret.interactable = set;
            Frost_Turret.interactable = set;
            Earth_Turret.interactable = set;
    }

    public void Upgrade() {
        assignedBuilding?.UpgradeBuilding();
        UpdateBuildingStats();
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

    public void Destroy() {
        assignedBuilding?.DestroyBuilding();
        m_closeTab?.Invoke();
    }

    private void UpdateBuildingStats() {
        LevelText.text = "Level: " + assignedBuilding.level;
        if (assignedBuilding.level >= 5)
            SetInteractiveType(true);
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
}
