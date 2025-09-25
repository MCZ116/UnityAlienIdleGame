using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class BuildingManager : MonoBehaviour
{
    public GameObject buildingPrefab;
    public Transform container;          // UI parent (e.g., ScrollView content)
    public GameManager gameManager;
    public List<BuildingState> buildings = new();

    private void Awake()
    {
        // Assign index based on the list order
        for (int i = 0; i < buildings.Count; i++)
        {
            buildings[i].data.buildingIndex = i + 1; // start from 1

            // Optional: calculate initial profit per second
            buildings[i].profitPerSecond = buildings[i].GetCurrentProfit() / buildings[i].data.incomeInterval;
        }
    }

    public bool HasEnoughCurrency(BuildingState state, int levelsToBuy)
    {
        if (levelsToBuy == int.MaxValue)
        {
            // Calculate actual max affordable levels
            levelsToBuy = gameManager.CalculateMaxAffordableLevels(state);
        }

        double totalCost = gameManager.CalculateTotalCost(state, levelsToBuy);
        return gameManager.mainCurrency >= totalCost && levelsToBuy > 0;
    }



    public bool HasEnoughCrystals(BuildingState building)
    {
        return gameManager.crystalCurrency >= building.GetAstronautCost();
    }

    public void BuyLevel(BuildingState state)
    {
        int buyAmount = gameManager.GetBuyAmount();
        // Handle MAX: calculate the maximum affordable levels
        if (buyAmount == int.MaxValue)
            buyAmount = gameManager.CalculateMaxAffordableLevels(state);

        if (buyAmount <= 0) return;

        double totalCost = gameManager.CalculateTotalCost(state, buyAmount);

        if (gameManager.mainCurrency >= totalCost)
        {
            gameManager.mainCurrency -= totalCost;
            state.level += buyAmount;
            state.profitPerSecond = state.GetCurrentProfit() / state.data.incomeInterval;

            CheckCrystalRewards(state);
            state.UpdateVisuals();
        }
    }

    private void CheckCrystalRewards(BuildingState state)
    {
        int newCheckpoint = state.level / 10;
        if (newCheckpoint > state.lastCrystalCheckpoint)
        {
            int crystalsEarned = newCheckpoint - state.lastCrystalCheckpoint;
            state.lastCrystalCheckpoint = newCheckpoint;
            gameManager.crystalCurrency += crystalsEarned;
        }
    }


    public void BuyAstronaut(BuildingState state)
    {
        if (!HasEnoughCrystals(state)) return;
        gameManager.crystalCurrency -= state.GetAstronautCost();
        state.UnlockNextAstronaut();
    }

    public void ResetAllBuildings()
    {
        foreach (var building in buildings)
        {
            building.level = 0;
            building.astronautsHired = 0;
            building.profitPerSecond = 0;
            building.UpdateVisuals();
        }
    }

    public void ApplyLoadedData(GameData data)
    {
        for (int i = 0; i < buildings.Count; i++)
        {
            buildings[i].level = data.buildingLevels[i];
            buildings[i].astronautsHired = data.astronautsHired[i];

            // Restore astronauts visuals
            buildings[i].RestoreAstronauts();
            buildings[i].UpdateVisuals();
            // Recalculate profit per second
            buildings[i].profitPerSecond = buildings[i].GetCurrentProfit() / buildings[i].data.incomeInterval;
        }
    }

}
