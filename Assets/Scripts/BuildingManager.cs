using System.Collections.Generic;
using UnityEngine;

public class BuildingManager : MonoBehaviour
{
    public GameObject buildingPrefab;    // Assign prefab
    public Transform container;          // UI parent (e.g., ScrollView content)
    public GameManager gameManager;
    public List<BuildingState> buildings = new();

    private void Awake()
    {
        // Assign index based on the list order
        for (int i = 0; i < buildings.Count; i++)
        {
            buildings[i].data.buildingIndex = i;

            // Optional: calculate initial profit per second
            buildings[i].profitPerSecond = buildings[i].GetCurrentProfit() / buildings[i].data.incomeInterval;
        }
    }

    public bool HasEnoughCurrency(BuildingState building)
    {
        return gameManager.mainCurrency >= building.GetCurrentPrice();
    }

    public bool HasEnoughCrystals(BuildingState building)
    {
        return gameManager.crystalCurrency >= building.GetCurrentAstronautsPrice();
    }

    public void BuyLevel(BuildingState state)
    {
        if (!HasEnoughCurrency(state)) return;
        gameManager.mainCurrency -= state.GetCurrentPrice();
        state.level++;
        state.profitPerSecond = state.GetCurrentProfit() / state.data.incomeInterval;
    }

    public void BuyAstronaut(BuildingState state)
    {
        if (!HasEnoughCurrency(state)) return;
        gameManager.crystalCurrency -= state.GetCurrentAstronautsPrice();
        state.astronautsHired++;
    }

}
