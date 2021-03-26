﻿

[System.Serializable]
public class GameData
{

    public double researchPointsData;
    public double crystals;
    public double alienLevelData;
    public double alienLevelData2;
    public double alienLevelData3;
    public double alienLevelData4;
    public double alienLevelData5;
    public double upgradeLevelData;
    public double mainResetLevelData;
    public double researchLevel1;
    public double researchLevel2;
    public bool upgradeActivated;
    public bool upgradeActivated2;
    public bool upgradeActivated3;
    public bool upgradeActivated4;
    public bool upgradeActivated5;
    public double rebirthCostData;
    public double suitsLevel1;
    public double suitsLevel2;
    public double upgradesLoopLenght;

    public GameData (GameManager idleScript)
    {
        researchPointsData = idleScript.mainCurrency;
        alienLevelData = idleScript.AlienLevel[0];
        alienLevelData2 = idleScript.AlienLevel[1];
        alienLevelData3 = idleScript.AlienLevel[2];
        alienLevelData4 = idleScript.AlienLevel[3];
        alienLevelData5 = idleScript.AlienLevel[4];
        upgradeLevelData = idleScript.upgradeLevel1;
        mainResetLevelData = idleScript.mainResetLevel;
        researchLevel1 = idleScript.Research1Level[0];
        researchLevel2 = idleScript.Research1Level[1];
        upgradeActivated = idleScript.upgradesActivated[0];
        upgradeActivated2 = idleScript.upgradesActivated[1];
        upgradeActivated3 = idleScript.upgradesActivated[2];
        upgradeActivated4 = idleScript.upgradesActivated[3];
        rebirthCostData = idleScript.rebirthCost;
        suitsLevel1 = idleScript.SuitsLevel[0];
        suitsLevel2 = idleScript.SuitsLevel[1];
        crystals = idleScript.crystalCurrency;

    }


}
