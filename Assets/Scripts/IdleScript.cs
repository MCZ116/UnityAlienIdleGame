using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class IdleScript : MonoBehaviour
{
    public OfflineProgress offline;
    public int buyModeID;
    public Text AlienLevelText;
    public Text CurrencyText;
    public Text RPointsText;
    public Text ButtonUpgradeOneText;
    public Text ButtonUpgradeMaxText;
    public Text ChangeBuyModeText;
    public double mainCurrency;
    public double alienUpgradeCosts;
    public double upgradeLevel1;
    public double mainResetLevel;

    public Research research;

    public GameObject alienScreen;

    public CanvasGroup canvasShop;

    public CanvasGroup canvasMainGame;

    public CanvasGroup canvasRebirthTab;

    public CanvasGroup canvasResearchTab;

    private double alienLevel;

    public double AlienLevel { get => alienLevel; set => alienLevel = value; }

    private double research1Level;
    [SerializeField]
    public double Research1Level { get => research1Level; set => research1Level = value; }

    void Start()
    {
        Application.targetFrameRate = 30;
        mainCurrency = 100;
        alienUpgradeCosts = 25;
        research1Level = 0;
        alienLevel = 0;
        upgradeLevel1 = 0;
        ChangeBuyModeText.text = "Upgrade: 1";
        Load();
        offline.OfflineProgressLoad();
        
    }

    IEnumerator MySave()
    {
        yield return new WaitForSeconds(5f);
        Save();
    }

    void Update()
    {
        var costIncrease = 25 * System.Math.Pow(1.07, upgradeLevel1);
        AlienLevelText.text = "Level: " + alienLevel.ToString("F0");
        CurrencyText.text = "Research Points: " + ExponentLetterSystem(mainCurrency, "F2");
        RPointsText.text = ResearchPointsPerSecond().ToString("F2") + "RP/s ";
        ButtonUpgradeOneText.text = "1 Upgrade \n Price: " + ExponentLetterSystem(costIncrease, "F2");
        ButtonUpgradeMaxText.text = "Buy: " + BuyMaxCount();
        
        mainCurrency += ResearchPointsPerSecond() * Time.deltaTime;
        StartCoroutine("MySave");
        SaveDate();
    }

    public double ResearchPointsPerSecond()
    {
        double temp = 0;
        temp += upgradeLevel1;
        temp += research.ResearchBoost();
        temp += RebirthBoost();
        return temp;
    }


    //public String ExponentSystem(double value, string numberAfterPoint)
    //{

    //    if (value > 1000)
    //    {
    //        var exponent = (System.Math.Floor(System.Math.Log10(System.Math.Abs(value))));
    //        var mantissa = (value / System.Math.Pow(10, exponent));
    //        return mantissa.ToString("F2") + "e" + exponent;
    //    }
    //    return value.ToString(numberAfterPoint);

    //}

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
                alienScreen.SetActive(true);
                CanvasGroupMenuSwitch(false, canvasRebirthTab);
                CanvasGroupMenuSwitch(false, canvasResearchTab);
                break;

            case "shopMenu":
                CanvasGroupMenuSwitch(false, canvasMainGame);
                CanvasGroupMenuSwitch(true, canvasShop);
                alienScreen.SetActive(false);
                CanvasGroupMenuSwitch(false, canvasRebirthTab);
                CanvasGroupMenuSwitch(false, canvasResearchTab);
                break;

            case "rebirth":
                CanvasGroupMenuSwitch(false, canvasMainGame);
                CanvasGroupMenuSwitch(false, canvasShop);
                alienScreen.SetActive(false);
                CanvasGroupMenuSwitch(true, canvasRebirthTab);
                CanvasGroupMenuSwitch(false, canvasResearchTab);
                break;

            case "researchMenu":
                CanvasGroupMenuSwitch(false, canvasMainGame);
                CanvasGroupMenuSwitch(false, canvasShop);
                alienScreen.SetActive(false);
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
        alienUpgradeCosts = gameData.upgradeCostsData;
        AlienLevel = gameData.alienLevelData;
        upgradeLevel1 = gameData.upgradeLevelData;
        mainResetLevel = gameData.mainResetLevelData;
        Research1Level = gameData.researchLevel1;
    }

    public void SaveDate()
    {
        PlayerPrefs.SetString("OfflineTime", System.DateTime.Now.ToBinary().ToString());
    }

    public void UpgradeButtonClicked()
    {
        var costIncrease = 25 * System.Math.Pow(1.07, upgradeLevel1);
        if (mainCurrency >= costIncrease)
        {
            mainCurrency -= costIncrease;
            upgradeLevel1++;
            //upgradeLevel1 *= 1.07;
            alienLevel++;

        } 

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

    public double BuyMaxCount()
    {
        var h = 25;
        var c = mainCurrency;
        var r = 1.07;
        var u = upgradeLevel1;
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

    public void BuyMaxUpgradeClicked()
    {
            var h = 25;
            var c = mainCurrency;
            var r = 1.07;
            var u = upgradeLevel1;

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
              upgradeLevel1 += (int)n;
              mainCurrency -= costUpgrade;
              alienLevel += n;
            }

    }

    public void FullReset()
    {
        if (mainCurrency >= 1000)
        {
            mainCurrency = 100;
            alienUpgradeCosts = 25;
            alienLevel = 0;
            upgradeLevel1 = 0;
            mainResetLevel++;
            research1Level = 0;
        }
    }

    public double RebirthBoost()
    {
        double rBoost = 0;
        rBoost += 0.05 * upgradeLevel1 * 0.1;
        rBoost += 0.05 * mainResetLevel * 1.7;
        return rBoost + 1;
    }
}
