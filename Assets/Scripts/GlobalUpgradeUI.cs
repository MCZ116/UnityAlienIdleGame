using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GlobalUpgradeUI : MonoBehaviour
{
    [Header("UI References")]
    public TextMeshProUGUI upgradeNameText;
    public TextMeshProUGUI levelText;
    public TextMeshProUGUI effectText;
    public TextMeshProUGUI costText;
    public Button purchaseButton;

    public GlobalUpgradeState upgradeState;

    [SerializeField]private GlobalUpgradeManager manager;

    public void Awake()
    {
        purchaseButton.onClick.AddListener(OnPurchaseClick);
    }

    private void Update()
    {
        RefreshUI();
    }

    private void OnPurchaseClick()
    {
        manager.BuyUpgrade(upgradeState);
    }

    private void RefreshUI()
    {
        var data = upgradeState.upgradeData;

        upgradeNameText.text = data.upgradeName;
        levelText.text = $"Level {upgradeState.level}";

        double nextCost = upgradeState.GetUpgradeCost(data);
        costText.text = GameManager.ExponentLetterSystem(nextCost);

        double previewBoost = manager.GetGlobalIncomeBoostWithPreview(data);
        double currentBoost = manager.GetGlobalIncomeBoost();

        UpdateUpgradeUI(data, upgradeState, (float)currentBoost, (float)previewBoost, (float)nextCost);

    }

    private void UpdateUpgradeUI(GlobalUpgradeData data, GlobalUpgradeState upgradeState, float currentBoost, float previewBoost, float nextCost)
    {
        string effectString = GetEffectString(data.type, upgradeState.level, currentBoost, previewBoost);

        if (effectText != null)
            effectText.text = effectString;

        if (purchaseButton != null && GameManager.instance != null)
            purchaseButton.interactable = GameManager.instance.antiMatter >= nextCost;
    }

    private string GetEffectString(UpgradeType type, int level, float currentBoost, float previewBoost)
    {
        string preview = $"<color=green>x{previewBoost:F2}</color>";

        if (level == 0)
        {
            return type == UpgradeType.ResearchCost
                ? $"Reduce price by {preview}"
                : $"Increase by {preview}";
        }

        string current = $"x{currentBoost:F2}";
        return type == UpgradeType.ResearchCost
            ? $"Reduced price by {current} next upgrade {preview}"
            : $"Increased by {current} next upgrade {preview}";
    }

}
