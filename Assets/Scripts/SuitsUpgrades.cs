using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SuitsUpgrades : MonoBehaviour
{
    public GameManager gameManager;

    public Text[] suitsTextField;
    public Text[] suitsTextLevels;
    public double[] suitsUpgradesCosts;
    double[] suitsUpgradesValues = { 0.4, 0.5 };
    string[] suitsUpgradeText = { "Space is full of micro meteors, durability for suits is important. Increasing income for about 10%", "Better gloves are helping detail work. Increasing income for about 2%" };

    void Start()
    {
        suitsUpgradesCosts = new double[2];



    }

    void Update()
    {
        for (int id = 0; id < gameManager.SuitsLevel.Length; id++)
        {
            if (gameManager.SuitsLevel[id] >= 1)
            {
                suitsTextLevels[id].text = "Level: " + gameManager.SuitsLevel[id].ToString("F0");
            }
            else
            {
                suitsTextLevels[id].text = "Not Activated";
            }
            suitsTextField[id].text = $"{suitsUpgradeText[id]}";
        }
    }

    public void SuitsUpgradeButtons(int id)
    {
        var h = suitsUpgradesCosts[id];
        var c = gameManager.crystalCurrency;
        var r = 1.07;
        var u = gameManager.SuitsLevel[id];
        double n = 1;
        var costSuitsUpgrade = h * (System.Math.Pow(r, u) * (System.Math.Pow(r, n) - 1) / (r - 1));

        if (gameManager.crystalCurrency >= costSuitsUpgrade)
        {
            gameManager.crystalCurrency -= costSuitsUpgrade;
            gameManager.SuitsLevel[id] += (int)n;
            SuitsBoost();

        }

    }

    public double SuitsBoost()
    {
        double suitBoost = 0;

        for (int id = 0; id < gameManager.SuitsLevel.Length; id++)
        {
            suitBoost += 0.05 * gameManager.SuitsLevel[id] * suitsUpgradesValues[id];
        }

        return suitBoost;
    }
}
