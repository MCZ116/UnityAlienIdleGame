using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Research : MonoBehaviour
{
    public IdleScript idleScript;

    public Text research1Text;
    public Text research1Level;
    double conditionResearch1;



    void Start()
    {
        conditionResearch1 = 10000;
    }

    void Update()
    {
        if (idleScript.Research1Level >= 1)
        {
            research1Level.text = "Level: " + idleScript.Research1Level.ToString("F0");
        }
        else
        {
            research1Level.text = "Not Activated";
        }
        research1Text.text = "Better oxygen tanks allow astronauts to stay " + ResearchBoost().ToString("F2") + "% longer on the surface of the planet";
    }

    public void ResearchUpgradeButton()
    {
        var h = 10000;
        var c = idleScript.mainCurrency;
        var r = 1.07;
        var u = idleScript.Research1Level;
        double n = 1;
        var costResearchUpgrade = h * (System.Math.Pow(r, u) * (System.Math.Pow(r, n) - 1) / (r - 1));

        if (idleScript.mainCurrency >= costResearchUpgrade)
        {
            idleScript.mainCurrency -= costResearchUpgrade;
            idleScript.Research1Level++;
            ResearchBoost();

        }

    }

    public double ResearchBoost()
    {
        double resBoost = 0;
        resBoost += 0.05 * idleScript.Research1Level * 0.2;
        return resBoost + 1;
    }

}
