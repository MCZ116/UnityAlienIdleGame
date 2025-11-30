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
        icon.sprite = state.data.icon;
    }

    private void Update()
    {
        RefreshUI();
    }

    private void RefreshUI()
    {
        levelText.text = state.level.ToString();
        progressBar.fillAmount = Mathf.Clamp01(state.currentProgress);
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
