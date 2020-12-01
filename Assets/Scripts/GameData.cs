//using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{

    public double researchPointsData;
    public double upgradeCostsData;
    public double alienLevelData;
    public double upgradeLevelData;
    public double mainResetLevelData;


    public GameData (IdleScript idleScript)
    {
        researchPointsData = idleScript.mainCurrency;
        upgradeCostsData = idleScript.alienUpgradeCosts;
        alienLevelData = idleScript.AlienLevel;
        upgradeLevelData = idleScript.upgradeLevel1;
        mainResetLevelData = idleScript.mainResetLevel;

    }


}
