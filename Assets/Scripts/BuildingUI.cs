using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BuildingUI : MonoBehaviour
{
    public TextMeshProUGUI levelText;
    public Button mainButton;
    public Image icon;
    public Image progressBar;
    public BuildingManager buildingManager;

    public BuildingState state;

    private void Awake()
    {
        mainButton.onClick.AddListener(ToggleUpgradePanel);
    }

    private void Update()
    {
        progressBar.fillAmount = Mathf.Clamp01(state.currentProgress);
        RefreshUI();
    }

    private void RefreshUI()
    {
        levelText.text = state.level.ToString();
    }

    public void ToggleUpgradePanel()
    {
        UpgradePanelController.Instance.TogglePanel(this);
    }

    public void OnUpgradeClicked()
    {
        buildingManager.BuyLevel(state);
    }

    public void OnAstronautClicked()
    {
        buildingManager.BuyAstronaut(state);
    }
}
