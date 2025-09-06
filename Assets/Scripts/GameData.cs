

using System.Collections.Generic;

[System.Serializable]
public class GameData
{
    public double researchPointsData;
    public double crystals;
    public double[] stageLevelData;
    public double upgradeLevelData;
    public double mainResetLevelData;
    public double[] researchLevel;
    public bool[] upgradeActivated;
    public bool[] astronautsbuy;
    public double rebirthCostData;
    public double[] suitsLevel;
    public double upgradesLoopLenght;
    public int[] astronautsLevel;
    public int[] astronautIDStartPosition;
    public bool[] planetUnlocked;
    public bool[] researchCanBeDone;
    public bool[] researchUnlocked;
    public List<int> researchIds = new();

    public GameData (GameManager gameManager, ResearchManager researchManager)
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

        planetUnlocked = gameManager.planetUnlocked;

        foreach (var researchData in researchManager.unlockedResearches)
        {
            researchIds.Add(researchData.researchId);
        }

    }


}
