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
    public Button upgradeButton;
    public Button astronautButton;
    [SerializeField] private GameObject priceContainer;

    private BuildingState buildingState;
    private BuildingManager buildingManager;

    public void SetBuilding(BuildingUI building)
    {
        buildingState = building.state;
        buildingManager = building.buildingManager;

        nameText.text = building.state.data.name; // example
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
        if (buildingState != null)
            RefreshUI();
    }

    private void RefreshUI()
    {
        UpdateUpgradeButton(buildingState);
        UpdateAstronautButton(buildingState);

        profitPerSecondText.text = GameManager.ExponentLetterSystem(buildingState.profitPerSecond) + "/s";
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
