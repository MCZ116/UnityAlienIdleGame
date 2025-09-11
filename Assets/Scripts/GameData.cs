

using System.Collections.Generic;

[System.Serializable]
public class GameData
{
    public double researchPointsData;
    public double crystals;
    public double[] stageLevelData;
    public double upgradeLevelData;
    public double mainResetLevelData;
    public bool[] upgradeActivated;
    public bool[] astronautsbuy;
    public double rebirthCostData;
    public double[] suitsLevel;
    public double upgradesLoopLenght;
    public int[] astronautsLevel;
    public int[] astronautIDStartPosition;
    public List<int> researchIds = new();
    public List<int> planetIds = new();


    public GameData (GameManager gameManager, ResearchManager researchManager, PlanetManager planetManager)
    {
        researchPointsData = gameManager.mainCurrency;

        stageLevelData = gameManager.StageLevel;

        upgradeLevelData = gameManager.upgradeLevel1;

        mainResetLevelData = gameManager.mainResetLevel;

        upgradeActivated = gameManager.upgradesActivated;

        rebirthCostData = gameManager.rebirthCost;

        suitsLevel = gameManager.SuitsLevel;

        crystals = gameManager.crystalCurrency;

        astronautsbuy = gameManager.confirmAstronautBuy;

        astronautsLevel = gameManager.astronautsLevel;

        astronautIDStartPosition = gameManager.astronautBuyStartID;

        foreach (var researchData in researchManager.unlockedResearches)
        {
            researchIds.Add(researchData.researchId);
        }

        foreach (var planetData in planetManager.unlockedPlanets)
        {
            planetIds.Add(planetData.planetId);
        }
    }
}
