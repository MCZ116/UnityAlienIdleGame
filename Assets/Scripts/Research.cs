using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Research : MonoBehaviour
{
    public IdleScript idleScript;

    public Text[] researchTextField;
    public Text[] researchLevels;

    double[] researchCosts;
    double[] upgradeResearchValues = { 0.2, 0.5 };
    string[] researchText = { "Improve oxgen tanks capacity for about 2%. Better oxygen tanks allow astronauts to stay longer on the surface of the planet", "Durability of drills improve digging for about 10%" };
   

    void Start()
    {
        researchCosts = new double[2];
        researchCosts[0] = 10000;
        researchCosts[1] = 18000;
      
    }

    void Update()
    {
        for (int id = 0; id < idleScript.Research1Level.Length; id++)
        {
            if (idleScript.Research1Level[id] >= 1)
            {
                researchLevels[id].text = "Level: " + idleScript.Research1Level[id].ToString("F0");
            }
            else
            {
                researchLevels[id].text = "Not Activated";
            }
            researchTextField[id].text = $"{researchText[id]}";
        }

    }

    public void ResearchUpgradeButton(int id)
    {
        var h = researchCosts[id];
        var c = idleScript.mainCurrency;
        var r = 1.07;
        var u = idleScript.Research1Level[id];
        double n = 1;
        var costResearchUpgrade = h * (System.Math.Pow(r, u) * (System.Math.Pow(r, n) - 1) / (r - 1));

        if (idleScript.mainCurrency >= costResearchUpgrade)
        {
            idleScript.mainCurrency -= costResearchUpgrade;
            idleScript.Research1Level[id] += (int)n;
            ResearchBoost();

        }

    }

    public double ResearchBoost()
    {
        double resBoost = 0;

        for (int id = 0; id < idleScript.Research1Level.Length; id++) {
            resBoost += 0.05 * idleScript.Research1Level[id] * upgradeResearchValues[id];
        }

        return resBoost;
    }


}

