

using System.Collections.Generic;

[System.Serializable]
public class GameData
{
    public double researchPointsData;
    public double crystals;
    public int resetLevel;
    public double rebirthCostData;
    public double[] suitsLevel;
    public double incomeMultiplier;
    public double totalCurrencyEarned;

    public List<int> researchIds = new();
    public List<int> planetIds = new();
    public List<int> buildingLevels = new();
    public List<int> astronautsHired = new();

    public GameData (GameManager gameManager, ResearchManager researchManager, PlanetManager planetManager, BuildingManager buildingManager)
    {
        researchPointsData = gameManager.mainCurrency;

        resetLevel = gameManager.resetLevel;

        rebirthCostData = gameManager.rebirthCost;

        suitsLevel = gameManager.SuitsLevel;

        crystals = gameManager.crystalCurrency;

        incomeMultiplier = gameManager.incomeMultiplier;

        totalCurrencyEarned = gameManager.totalCurrencyEarned;

        foreach (var researchData in researchManager.unlockedResearches)
        {
            researchIds.Add(researchData.researchId);
        }

        foreach (var planetData in planetManager.unlockedPlanets)
        {
            planetIds.Add(planetData.planetId);
        }

        foreach (var buildingState in buildingManager.buildings)
        {
            buildingLevels.Add(buildingState.level);
        }

        foreach (var buildingState in buildingManager.buildings)
        {
            astronautsHired.Add(buildingState.astronautsHired);
        }
    }
}
