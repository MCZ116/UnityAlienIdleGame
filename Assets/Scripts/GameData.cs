

using UnityEngine;

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
    public int researchID;

    public GameData (GameManager gameManager)
    {
        researchPointsData = gameManager.mainCurrency;

        stageLevelData = gameManager.StageLevel;

        upgradeLevelData = gameManager.upgradeLevel1;

        mainResetLevelData = gameManager.mainResetLevel;

        researchLevel = gameManager.Research1Level;

        upgradeActivated = gameManager.upgradesActivated;

        rebirthCostData = gameManager.rebirthCost;

        suitsLevel = gameManager.SuitsLevel;

        crystals = gameManager.crystalCurrency;

        astronautsbuy = gameManager.confirmAstronautBuy;

        astronautsLevel = gameManager.astronautsLevel;

        astronautIDStartPosition = gameManager.astronautBuyStartID;

        planetUnlocked = gameManager.planetUnlocked;

        researchCanBeDone = gameManager.researchCanBeDone;

        researchUnlocked = gameManager.researchUnlocked;

    }


}
