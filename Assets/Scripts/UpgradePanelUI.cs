using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradePanelUI : MonoBehaviour
{
    [Header("UI Elements")]
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI priceText;
    public TextMeshProUGUI maxText;
    public TextMeshProUGUI priceAstronautText;
    public TextMeshProUGUI profitPerSecondText;
    public TextMeshProUGUI possibleProfitPerSecond;
    public TextMeshProUGUI totalProfit;
    public TextMeshProUGUI incomeTimer;
    public TextMeshProUGUI level;
    public Button upgradeButton;
    public Button astronautButton;
    public Image buildingIcon;
    public Image progressBar;
    [SerializeField] private GameObject priceContainer;

    private BuildingState buildingState;
    private BuildingManager buildingManager;

    public void SetBuilding(BuildingUI building)
    {
        buildingState = building.state;
        buildingManager = building.buildingManager;

        nameText.text = building.state.data.name;
        buildingIcon.sprite = building.state.data.icon;
        level.text = "Level " + building.state.level.ToString();
        nameText.text = building.state.data.buildingName;
        RefreshUI();

        // Remove previous listeners
        upgradeButton.onClick.RemoveAllListeners();
        astronautButton.onClick.RemoveAllListeners();

        // Assign new listeners for this building
        upgradeButton.onClick.AddListener(() =>
        {
            buildingManager.BuyLevel(buildingState);
            RefreshUI();
            AudioControler.instance.ButtonClickSound();
        });

        astronautButton.onClick.AddListener(() =>
        {
            buildingManager.BuyAstronaut(buildingState);
            RefreshUI();
            AudioControler.instance.ButtonClickSound();
        });
    }

    private void Update()
    {
        progressBar.fillAmount = Mathf.Clamp01(buildingState.currentProgress);

        if (buildingState != null)
            RefreshUI();
    }

    private void RefreshUI()
    {
        UpdateUpgradeButton(buildingState);
        UpdateAstronautButton(buildingState);

        double currentProfit = GameManager.instance.GetIncomePerSecondOfBuilding(buildingState);
        profitPerSecondText.text = GameManager.ExponentLetterSystem(currentProfit) + "/s";
        double nextProfit = GameManager.instance.GetIncomePerSecondOfBuilding(buildingState, GameManager.instance.GetBuyAmount());
        possibleProfitPerSecond.text = GameManager.ExponentLetterSystem(nextProfit) + "/s";
        incomeTimer.text = buildingState.TimeRemaining.ToString("F1")+"s";
        totalProfit.text = GameManager.ExponentLetterSystem(buildingState.GetCurrentProfit());
    }

    private void UpdateUpgradeButton(BuildingState buildingState)
    {
        int buyAmount = GameManager.instance.GetBuyAmount();
        double previewCost = GameManager.instance.GetCostPreview(buildingState);

        upgradeButton.interactable = buildingManager.HasEnoughCurrency(buildingState, buyAmount);

        priceText.text = GameManager.ExponentLetterSystem(previewCost);
    }

    private void UpdateAstronautButton(BuildingState buildingState)
    {
        int maxAstronauts = buildingState.data.maxAstronauts;
        bool atMax = buildingState.astronautsHired >= maxAstronauts;

        astronautButton.gameObject.SetActive(true);
        priceAstronautText.gameObject.SetActive(true);

        if (atMax)
        {
            astronautButton.interactable = false;
            priceContainer.SetActive(false);
            maxText.gameObject.SetActive(true);
            maxText.text = "MAX";
            return;
        }

        maxText.gameObject.SetActive(false);
        astronautButton.interactable = buildingManager.HasEnoughCrystals(buildingState);
        priceContainer.SetActive(true);
        priceAstronautText.text = buildingState.GetAstronautCost().ToString();
    }



}
