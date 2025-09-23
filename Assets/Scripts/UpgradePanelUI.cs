using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradePanelUI : MonoBehaviour
{
    [Header("UI Elements")]
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI priceText;
    public TextMeshProUGUI priceAstronautText;
    public TextMeshProUGUI profitPerSecondText;
    public Button upgradeButton;
    public Button astronautButton;

    private BuildingState targetState;
    private BuildingManager targetManager;

    public void SetBuilding(BuildingUI building)
    {
        targetState = building.state;
        targetManager = building.buildingManager;

        nameText.text = building.state.data.name; // example
        UpdateButtons();

        // Remove previous listeners
        upgradeButton.onClick.RemoveAllListeners();
        astronautButton.onClick.RemoveAllListeners();

        // Assign new listeners for this building
        upgradeButton.onClick.AddListener(() =>
        {
            targetManager.BuyLevel(targetState);
            UpdateButtons();
        });

        astronautButton.onClick.AddListener(() =>
        {
            targetManager.BuyAstronaut(targetState);
            UpdateButtons();
        });
    }

    private void Update()
    {
        if (targetState != null)
            UpdateButtons();
    }

    private void UpdateButtons()
    {
        priceText.text = GameManager.ExponentLetterSystem(GameManager.instance.GetCostPreview(targetState), "F2");
        priceAstronautText.text = GameManager.ExponentLetterSystem(targetState.GetCurrentAstronautsPrice(), "F0");

        upgradeButton.interactable = targetManager.HasEnoughCurrency(targetState,GameManager.instance.GetBuyAmount());
        astronautButton.interactable = targetManager.HasEnoughCrystals(targetState)
                                      && targetState.astronautsHired < targetState.data.maxAstronauts;
        profitPerSecondText.text = GameManager.ExponentLetterSystem(targetState.profitPerSecond, "F2") + "/sec";
    }
}
