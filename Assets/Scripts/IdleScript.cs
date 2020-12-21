using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class IdleScript : MonoBehaviour
{
    public OfflineProgress offline;
    public int buyModeID;
    public Text[] AlienLevelText;
    public Text CurrencyText;
    public Text RPointsText;
    public Text[] ButtonUpgradeMaxText;
    public Text ChangeBuyModeText;
    public Text RebirthPrice;
    public Text RebirthLevel;
    public double mainCurrency;
    public double rebirthCost;
    double[] alienUpgradeCosts = { 25, 225 };
    public double upgradeLevel1;
    public double mainResetLevel;
    private GameObject[] progressBarObject;
    public bool[] upgradesActivated = { false };
    bool activateRB = false;
    public Image[] progressBar;

    public Button[] upgradeButtons;

    public Research research;

    public CanvasGroup canvasShop;

    public CanvasGroup canvasMainGame;

    public CanvasGroup canvasRebirthTab;

    public CanvasGroup canvasResearchTab;

    private double[] alienLevel;

    public double[] AlienLevel { get => alienLevel; set => alienLevel = value; }

    private double[] research1Level;

    public double[] Research1Level { get => research1Level; set => research1Level = value; }

    public double[] upgradesCounts = { 0.3, 0.4 };
    public float[] upgradeMaxTime = { 5f, 10f };
    public float[] upgradeTimer = { 5f, 10f };
    public float[] progressTimer = { 0f, 0f };

    void Start()
    {
        Application.targetFrameRate = 30;
        mainCurrency = 100;
        rebirthCost = 10000;
        Research1Level = new double[2];
        Research1Level[0] = 0;
        Research1Level[1] = 0;
        AlienLevel = new double[2];
        alienLevel[0] = 0;
        alienLevel[1] = 0;
        upgradeLevel1 = 0;
        ChangeBuyModeText.text = "Upgrade: 1";
        Load();
        offline.OfflineProgressLoad();
        mainResetLevel = 1;
        
    }

    IEnumerator MySave()
    {
        yield return new WaitForSeconds(5f);
        Save();
    }

    void Update()
    {
        BarAssigning();
        CurrencyText.text = "Research Points: " + ExponentLetterSystem(mainCurrency, "F2");
        RPointsText.text = ResearchPointsCalculator().ToString("F2") + "RP/s ";
        RebirthPrice.text = "Level \n" + ExponentLetterSystem(rebirthCost, "F2");
        RebirthLevel.text = "Rebirth " + mainResetLevel ;

        for (int id = 0; id < AlienLevel.Length; id++) {
            AlienLevelText[id].text = "Level: " + alienLevel[id].ToString("F0");
            ButtonUpgradeMaxText[id].text = "Buy: " + BuyMaxCount(id) + "\n Price: " + ExponentLetterSystem(BuyCount(id), "F2");

            if (progressBarObject.Length == progressBar.Length)
            {
                progressTimer[id] += Time.deltaTime;
                progressBar[id].fillAmount = (progressTimer[id] / upgradeMaxTime[id]);
            }

            if (progressTimer[id] >= upgradeMaxTime[id])
            {
                progressTimer[id] = 0f;
            }

            if (mainCurrency >= BuyCount(id))
            {
                upgradeButtons[id].interactable = true;
            } else {
                upgradeButtons[id].interactable = false;
            }

            if (upgradeTimer[id] <= 0)
            {
                mainCurrency += ResearchPointsCalculator();
                upgradeTimer[id] = upgradeMaxTime[id];
            }
            else
                upgradeTimer[id] -= Time.deltaTime;

        }


        
        StartCoroutine("MySave");
        SaveDate();
    }

    public void BarAssigning()
    {
        progressBarObject = GameObject.FindGameObjectsWithTag("progressBars");
        progressBar = new Image[progressBarObject.Length];
        for (int id = 0; id < progressBarObject.Length; id++)
        {
            progressBar[id] = progressBarObject[id].GetComponent<Image>();
        }
    }

    //public double ResearchPointsPerSecond()
    //{
    //    double temp = 0;
    //    temp += upgradeLevel1;
    //    temp += AlienLevel[0] * 0.3;
    //    temp += AlienLevel[1] * 0.3;
    //    temp += research.ResearchBoost();
    //    temp += RebirthBoost();
    //    return temp;
    //}

    public double ResearchPointsCalculator()
    {
        double tempB = 0;
        double temp = 0;
        tempB += research.ResearchBoost();
        tempB += RebirthBoost();
        tempB += upgradeLevel1;
        for (int id = 0; id < AlienLevel.Length; id++)
        {
            if (progressTimer[id] >= upgradeMaxTime[id])
            {
                    temp += AlienLevel[id] * upgradesCounts[id];
                    return temp;
            }
        }
        return tempB;
    }

    public static String ExponentLetterSystem(double value, string numberToString)
    {

        if (value <= 1000) return value.ToString(numberToString);

        var exponentSci = Math.Floor(Math.Log10(value));
        var exponentEng = 3 * Math.Floor(exponentSci / 3);
        var letterOne = ((char)Math.Floor((((double)exponentEng - 3) / 3) % 26 + 97)).ToString();

        if ((double)exponentEng / 3 >= 27)
        {
            var letterTwo = ((char)(Math.Floor(((double)exponentEng - 3 * 26) / (3 * 26)) % 26 + 97)).ToString();
            return (value / Math.Pow(10, exponentEng)).ToString("F2") + letterTwo + letterOne;
        }
        if (value > 1000)
        {
            return (value / Math.Pow(10, exponentEng)).ToString("F2") + letterOne;

        }
        return value.ToString("F2");

    }

    public void CanvasGroupMenuSwitch(bool status, CanvasGroup choosenGroup)
    {
        if (status)
        {
            choosenGroup.alpha = 1;
            choosenGroup.interactable = true;
            choosenGroup.blocksRaycasts = true;
        }
        else
        {
            choosenGroup.alpha = 0;
            choosenGroup.interactable = false;
            choosenGroup.blocksRaycasts = false;
        }
    }

    public void ChangeTab(string tabName)
    {
        switch (tabName)
        {
            case "gameMenu":
                CanvasGroupMenuSwitch(true, canvasMainGame);
                CanvasGroupMenuSwitch(false, canvasShop);
                CanvasGroupMenuSwitch(false, canvasRebirthTab);
                CanvasGroupMenuSwitch(false, canvasResearchTab);
                break;

            case "shopMenu":
                CanvasGroupMenuSwitch(false, canvasMainGame);
                CanvasGroupMenuSwitch(true, canvasShop);
                CanvasGroupMenuSwitch(false, canvasRebirthTab);
                CanvasGroupMenuSwitch(false, canvasResearchTab);
                break;

            case "rebirth":
                CanvasGroupMenuSwitch(false, canvasMainGame);
                CanvasGroupMenuSwitch(false, canvasShop);
                CanvasGroupMenuSwitch(true, canvasRebirthTab);
                CanvasGroupMenuSwitch(false, canvasResearchTab);
                break;

            case "researchMenu":
                CanvasGroupMenuSwitch(false, canvasMainGame);
                CanvasGroupMenuSwitch(false, canvasShop);
                CanvasGroupMenuSwitch(false, canvasRebirthTab);
                CanvasGroupMenuSwitch(true, canvasResearchTab);
                break;
        } 
    }

    public void Save()
    {

        SaveSystem.SaveGameData(this);

    }

    public void Load()
    {
        GameData gameData = SaveSystem.LoadData();

        mainCurrency = gameData.researchPointsData;
        AlienLevel[0] = gameData.alienLevelData;
        AlienLevel[1] = gameData.alienLevelData2;
        upgradeLevel1 = gameData.upgradeLevelData;
        mainResetLevel = gameData.mainResetLevelData;
        Research1Level[0] = gameData.researchLevel1;
        Research1Level[1] = gameData.researchLevel2;
        upgradesActivated[0] = gameData.upgradeActivated;
        rebirthCost = gameData.rebirthCostData;
    }

    public void SaveDate()
    {
        PlayerPrefs.SetString("OfflineTime", System.DateTime.Now.ToBinary().ToString());
    }

    public void ChangeBuyButtonMode()
    {
        switch (buyModeID)
        {
            case 0:
                ChangeBuyModeText.text = "Upgrade: 1";
                buyModeID = 1;
                break;
            case 1:
                ChangeBuyModeText.text = "Upgrade: 10";
                buyModeID = 2;
                break;
            case 2:
                ChangeBuyModeText.text = "Upgrade: 100";
                buyModeID = 3;
                break;
            case 3:
                ChangeBuyModeText.text = "Upgrade: MAX";
                buyModeID = 0;
                break;
        }
    }

    public double BuyCount(int id)
    {
        var h = alienUpgradeCosts[id];
        var c = mainCurrency;
        var r = 1.07;
        var u = AlienLevel[id];
        double n = 0;

        switch (buyModeID)
        {
            case 0:
                n = System.Math.Floor(System.Math.Log(c * (r - 1) / (h * System.Math.Pow(r, u)) + 1, r));
                break;
            case 1:
                n = 1;
                break;
            case 2:
                n = 10;
                break;
            case 3:
                n = 100;
                break;
        }
        var costUpgrade = h * (System.Math.Pow(r, u) * (System.Math.Pow(r, n) - 1) / (r - 1));
        return costUpgrade;
    }

    public double BuyMaxCount(int id)
    {
        var h = alienUpgradeCosts[id];
        var c = mainCurrency;
        var r = 1.07;
        var u = AlienLevel[id];
        double n = 0;

        switch (buyModeID)
        {
            case 0:
                return System.Math.Floor(System.Math.Log(c * (r - 1) / (h * System.Math.Pow(r, u)) + 1, r));
            case 1:
                return 1;
            case 2:
                return 10;
            case 3:
                return 100;
        }
        return n;
    }

    public void BuyMaxUpgradeClicked(int id)
    {
            var h = alienUpgradeCosts[id];
            var c = mainCurrency;
            var r = 1.07;
            var u = AlienLevel[id];

        double n = 0;

        switch (buyModeID)
        {
            case 0:
                n = System.Math.Floor(System.Math.Log(c * (r - 1) / (h * System.Math.Pow(r, u)) + 1, r));
                break;
            case 1:
                n = 1;
                break;
            case 2:
                n = 10;
                break;
            case 3:
                n = 100;
                break;
        }

        var costUpgrade = h * (System.Math.Pow(r, u) * (System.Math.Pow(r, n) - 1) / (r - 1));

            if(mainCurrency >= costUpgrade)
            {
              alienLevel[id] += (int)n;
              mainCurrency -= costUpgrade;
              upgradeLevel1 += (int)n;
            }

    }

    public void FullReset()
    {
        if (mainCurrency >= rebirthCost)
        {
            mainCurrency = 100;
            alienLevel[1] = 1;
            alienLevel[0] = 1;
            upgradeLevel1 = 0;
            mainResetLevel++;
            Research1Level[0] = 0;
            Research1Level[1] = 0;
            upgradesActivated[0] = false;
            rebirthCost *= (System.Math.Pow(2, mainResetLevel) * (System.Math.Pow(2, 1) - 1) / (2 - 1));
        }
        
    }
    // Deleted +1 on return
    public double RebirthBoost()
    {
      
            double rBoost = 0;
            for (int id = 0; id < AlienLevel.Length; id++)
            {
                rBoost += AlienLevel[id] * 0.1;
            }
            rBoost += 0.05 * upgradeLevel1 * 0.1;
            rBoost += 0.05 * mainResetLevel * 1.7;
            return rBoost;

    }
}
