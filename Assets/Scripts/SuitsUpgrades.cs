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
    double[] suitsUpgradesValues = { 0.4, 0.5, 0.6, 0.8, 0.85, 0.90 };
    string[] suitsUpgradeText;

    void Start()
    {
        suitsUpgradesCosts = new double[6];
        //suitsUpgradesCosts[0] = 10000;
        //suitsUpgradesCosts[1] = 20000;
        //suitsUpgradesCosts[2] = 30000;
        //suitsUpgradesCosts[3] = 40000;
        //suitsUpgradesCosts[4] = 50000;
        //suitsUpgradesCosts[5] = 60000;
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
            Debug.Log(" Suits Upgrade Cost: " + SuitsUpgradeCalc(id));
            
        }
        //HideIfClickedOutside(suitsObjectInfoWindow);
    }

    //private void HideIfClickedOutside(GameObject panel)
    //{
    //    if (Input.GetMouseButton(0) && panel.activeSelf && !RectTransformUtility.RectangleContainsScreenPoint(panel.GetComponent<RectTransform>(),
    //        Input.mousePosition,Camera.main))
    //    {
    //        panel.SetActive(false);
    //    }
    //}

    public void SuitsInfoWindowOnClick(int id)
    {
        suitsObjectInfoWindow.SetActive(true);
        suitsTextField.text = $"{suitsUpgradeText[id]}";

        //researchWindowIcon.sprite = researchImage[id].sprite;


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
            SuitsBoost();

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
        double suitBoost = 0;

        for (int id = 0; id < gameManager.SuitsLevel.Length; id++)
        {
            suitBoost += 0.05 * gameManager.SuitsLevel[id] * suitsUpgradesValues[id];
        }

        return suitBoost;
    }
}
