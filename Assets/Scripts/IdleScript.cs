using UnityEngine;
using UnityEngine.UI;

public class IdleScript : MonoBehaviour
{

    public int delayTime = 1;
    public int buyModeID;
    public Text AlienLevelText;
    public Text CurrencyText;
    public Text RPointsText;
    public Text ButtonUpgradeOneText;
    public Text ButtonUpgradeMaxText;
    public Text ChangeBuyModeText;
    public double mainCurrency;
    public double alienUpgradeCosts;
    public double researchPointsPerSecond;
    public double upgradeLevel1;

    public GameObject alienScreen;

    public CanvasGroup canvasShop;

    public CanvasGroup canvasMainGame;

    private double alienLevel;

    public double AlienLevel { get => alienLevel; set => alienLevel = value; }

    void Start()
    {
        Application.targetFrameRate = 30;
        mainCurrency = 100;
        alienUpgradeCosts = 25;
        alienLevel = 0;
        upgradeLevel1 = 0;
        ChangeBuyModeText.text = "Upgrade: 5";
    }

    void Update()
    {
        var costIncrease = 25 * System.Math.Pow(1.07, upgradeLevel1);
        researchPointsPerSecond = upgradeLevel1;

        AlienLevelText.text = "Level: " + alienLevel.ToString("F0");
        CurrencyText.text = "Research Points: " + mainCurrency.ToString("F0");
        RPointsText.text = researchPointsPerSecond.ToString("F0") + "RP/s ";
        ButtonUpgradeOneText.text = "1 Upgrade \n Price: " + costIncrease.ToString("F0");
        ButtonUpgradeMaxText.text = "Buy: " + BuyMaxCount();
        
        mainCurrency += researchPointsPerSecond * Time.deltaTime;
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
                break;

            case "shopMenu":
                CanvasGroupMenuSwitch(false, canvasMainGame);
                CanvasGroupMenuSwitch(true, canvasShop);
                alienScreen.SetActive(false);
                break;
        } 
    }

    //public void Save()
    //{

    //    mainCurrency = double.Parse(PlayerPrefs.GetString("mainCurrency", "100"));
    //    lightPower = double.Parse(PlayerPrefs.GetString("lightPower", "0"));
    //    lightUpgradeCosts = double.Parse(PlayerPrefs.GetString("lightUpgradeCosts", "25"));
    //    upgradeLevel1 = double.Parse(PlayerPrefs.GetString("upgradeLevel1", "0"));

    //}

    //public void Load()
    //{
    //    PlayerPrefs.SetString("mainCurrency", mainCurrency.ToString());
    //    PlayerPrefs.SetString("lightPower", lightPower.ToString());
    //    PlayerPrefs.SetString("lightUpgradeCosts", lightUpgradeCosts.ToString());
    //    PlayerPrefs.SetString("upgradeLevel1", mainCurrency.ToString());
    //}

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
}
