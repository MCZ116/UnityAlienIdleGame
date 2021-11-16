using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Research : MonoBehaviour
{
    public GameManager gameManager;
    public UnlockingSystem unlockingSystem;

    public Text researchTextField;
    public Text[] researchLevels;

    double[] researchCosts;
    public double[] upgradeResearchValues;
    string[] researchText;
    

    void Start()
    {
        researchText = new string[2];
        researchText[0] = "Improve oxgen tanks capacity for about 2%. Better oxygen tanks allow astronauts to stay longer on the surface of the planet";
        researchText[1] = "Durability of drills improve digging for about 10%";
        researchCosts = new double[2];
        researchCosts[0] = 10000;
        researchCosts[1] = 18000;
      
    }

    void Update()
    {
        for (int id = 0; id < gameManager.Research1Level.Length; id++)
        {
            if (gameManager.Research1Level[id] >= 1)
            {
                researchLevels[id].text = "Lvl: " + gameManager.Research1Level[id].ToString("F0");
            }
            else
            {
                researchLevels[id].text = "Not Activated";
            }
            
        }

    }

    public void ResearchInfoWindowOnClick(int id)
    {
                researchTextField.text = $"{researchText[id]}";
    }

    public void ResearchUpgradeButton(int id)
    {
        ResearchInfoWindowOnClick(id);
        var h = researchCosts[id];
        var c = gameManager.mainCurrency;
        var r = 1.07;
        var u = gameManager.Research1Level[id];
        double n = 1;
        var costResearchUpgrade = h * (System.Math.Pow(r, u) * (System.Math.Pow(r, n) - 1) / (r - 1));
        
        if (gameManager.mainCurrency >= costResearchUpgrade && gameManager.researchCanBeDone[id] == true)
        {
            gameManager.mainCurrency -= costResearchUpgrade;
            gameManager.Research1Level[id] += (int)n;
            ResearchBoost();
            gameManager.researchUnlocked[id] = true;
        }

    }

    public double ResearchBoost()
    {
        double resBoost = 0;

        for (int id = 0; id < gameManager.Research1Level.Length; id++) {
            resBoost += gameManager.Research1Level[id] * upgradeResearchValues[id];
        }

        return resBoost;
    }


}

