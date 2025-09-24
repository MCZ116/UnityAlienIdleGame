using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SuitsUpgrades : MonoBehaviour
{
    public GameManager gameManager;

    public Text suitsTextField;
    public Text[] suitsTextLevels;
    public Text[] suitsTextCost;
    public GameObject suitsObjectInfoWindow;
    public double[] suitsUpgradesCosts;
    public double[] suitsUpgradesValues;
    string[] suitsUpgradeText;

    private void Awake()
    {
        suitsUpgradesCosts = new double[6];
        suitsUpgradesValues = new double[suitsUpgradesCosts.Length];

        for (int id = 0; id < suitsUpgradesCosts.Length; id++)
        {
            suitsUpgradesCosts[id] = 10000000 * (id + 1) * 1000;
            suitsUpgradesValues[id] = 1 + 0.4 * id;
        }   
    }

    void Start()
    {
        suitsUpgradeText = new string[6];
        suitsUpgradeText[0] = "Oxygen tank efficiency";
        suitsUpgradeText[1] = "Helmet durability";
        suitsUpgradeText[2] = "Torso durability";
        suitsUpgradeText[3] = "Gloves durability";
        suitsUpgradeText[4] = "Shoes durability";
        suitsUpgradeText[5] = "Improvement of tools use by astronauts";
    }

    void Update()
    {
        for (int id = 0; id < gameManager.SuitsLevel.Length; id++)
        {
            if (gameManager.SuitsLevel[id] >= 1)
            {
                suitsTextLevels[id].enabled = true;
                suitsTextLevels[id].text = gameManager.SuitsLevel[id].ToString("F0");
            }
            else
            {
                suitsTextLevels[id].enabled = false;
            }
            suitsTextCost[id].text = GameManager.ExponentLetterSystem(SuitsUpgradeCalc(id), "F2");

        }
        SuitshButtonStatus();
    }

    public void SuitsInfoWindowOnClick(int id)
    {
        suitsObjectInfoWindow.SetActive(true);
        suitsTextField.text = $"{suitsUpgradeText[id]}";
    }

    public void SuitsUpgradeButtons(int id)
    {
        var h = suitsUpgradesCosts[id];
        var c = gameManager.mainCurrency;
        var r = 1.07;
        var u = gameManager.SuitsLevel[id];
        double n = 1;
        var costSuitsUpgrade = h * (System.Math.Pow(r, u) * (System.Math.Pow(r, n) - 1) / (r - 1));

        if (gameManager.mainCurrency >= costSuitsUpgrade)
        {
            gameManager.mainCurrency -= costSuitsUpgrade;
            gameManager.SuitsLevel[id] += (int)n;

        }
        SuitsInfoWindowOnClick(id);
    }

    public double SuitsUpgradeCalc(int id)
    {
        var h = suitsUpgradesCosts[id];
        var c = gameManager.mainCurrency;
        var r = 1.07;
        var u = gameManager.SuitsLevel[id];
        double n = 1;
        var costSuitsUpgrade = h * (System.Math.Pow(r, u) * (System.Math.Pow(r, n) - 1) / (r - 1));

        return costSuitsUpgrade;
    }

    public double SuitsBoost()
    {
        double suitBoost = 1;

        for (int id = 0; id < gameManager.SuitsLevel.Length; id++)
        {
            suitBoost += 0.05 * gameManager.SuitsLevel[id] * suitsUpgradesValues[id];
        }

        return suitBoost;
    }

    public void SuitshButtonStatus()
    {

        for (int id = 0; id < gameManager.SuitsLevel.Length; id++)
        {

            if (gameManager.mainCurrency >= SuitsUpgradeCalc(id))
            {
                suitsTextCost[id].color = Color.green;
            }
            else
                suitsTextCost[id].color = Color.red;

        }
    }
}
