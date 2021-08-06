

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
    public bool astronautsbuy1;
    public bool astronautsbuy2;
    public bool astronautsbuy3;
    public bool astronautsbuy4;
    public bool astronautsbuy5;
    public bool astronautsbuy6;
    public bool astronautsbuy7;
    public bool astronautsbuy8;
    public bool astronautsbuy9;
    public bool astronautsbuy10;
    public bool astronautsbuy11;
    public bool astronautsbuy12;
    public bool astronautsbuy13;
    public bool astronautsbuy14;
    public bool astronautsbuy15;
    public bool astronautsbuy16;
    public bool astronautsbuy17;
    public bool astronautsbuy18;
    public bool astronautsbuy19;
    public bool astronautsbuy20;
    public double rebirthCostData;
    public double suitsLevel1;
    public double suitsLevel2;
    public double upgradesLoopLenght;
    public int astronautsLevel;
    public int astronautsLevel2;
    public int astronautsLevel3;
    public int astronautsLevel4;
    public int astronautsLevel5;
    public int astronautIDStart1;
    public int astronautIDStart2;
    public int astronautIDStart3;
    public int astronautIDStart4;
    public int astronautIDStart5;
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
        astronautsbuy1 = idleScript.confirmAstronautBuy[0];
        astronautsbuy2 = idleScript.confirmAstronautBuy[1];
        astronautsbuy3 = idleScript.confirmAstronautBuy[2];
        astronautsbuy4 = idleScript.confirmAstronautBuy[3];
        astronautsbuy5 = idleScript.confirmAstronautBuy[4];
        astronautsbuy6 = idleScript.confirmAstronautBuy[5];
        astronautsbuy7 = idleScript.confirmAstronautBuy[6];
        astronautsbuy8 = idleScript.confirmAstronautBuy[7];
        astronautsbuy9 = idleScript.confirmAstronautBuy[8];
        astronautsbuy10 = idleScript.confirmAstronautBuy[9];
        astronautsbuy11 = idleScript.confirmAstronautBuy[10];
        astronautsbuy12 = idleScript.confirmAstronautBuy[11];
        astronautsbuy13 = idleScript.confirmAstronautBuy[12];
        astronautsbuy14 = idleScript.confirmAstronautBuy[13];
        astronautsbuy15 = idleScript.confirmAstronautBuy[14];
        astronautsbuy16 = idleScript.confirmAstronautBuy[15];
        astronautsbuy17 = idleScript.confirmAstronautBuy[16];
        astronautsbuy18 = idleScript.confirmAstronautBuy[17];
        astronautsbuy19 = idleScript.confirmAstronautBuy[18];
        astronautsbuy20 = idleScript.confirmAstronautBuy[19];

        astronautsLevel = idleScript.astronautsLevel[0];
        astronautsLevel2 = idleScript.astronautsLevel[1];
        astronautsLevel3 = idleScript.astronautsLevel[2];
        astronautsLevel4 = idleScript.astronautsLevel[3];
        astronautsLevel5 = idleScript.astronautsLevel[4];
        astronautIDStart1 = idleScript.astronautBuyStartID[0];
        astronautIDStart2 = idleScript.astronautBuyStartID[1];
        astronautIDStart3 = idleScript.astronautBuyStartID[2];
        astronautIDStart4 = idleScript.astronautBuyStartID[3];
        astronautIDStart5 = idleScript.astronautBuyStartID[4];

    }


}
