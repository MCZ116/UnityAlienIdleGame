using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class IdleScript : MonoBehaviour
{
    public OfflineProgress offline;
    public int buyModeID;
    public Text[] AlienLevelText;
    public Text[] EarningStage;
    public Text CurrencyText;
    public Text RPointsText;
    public Text[] ButtonUpgradeMaxText;
    public Text ChangeBuyModeText;
    public Text RebirthPrice;
    public Text RebirthLevel;
    public Text CrystalsAmount;
    public double mainCurrency;
    public double crystalCurrency;
    public double rebirthCost;
    double[] alienUpgradeCosts;
    public double upgradeLevel1;
    public double mainResetLevel;
    private GameObject[] progressBarObject;
    private GameObject[] levelStageTextObject;
    private GameObject[] upgradeButtonObject;
    private GameObject[] buyMaxTextObject;
    private GameObject[] earningStageObject;
    private GameObject[] Stages;
    private GameObject[] unlockUpgradeText;
    public GameObject[] unlockGameObjects;
    public bool[] upgradesActivated;
    public bool[] earnedCrystal;
    double[] copyArray;
    //bool activateRB = false;
    public Image[] progressBar;

    public Button[] upgradeButtons;

    public Research research;

    public SuitsUpgrades suitsUpgrades;

    public UnlockingSystem unlockingSystem;

    public CanvasGroup canvasShop;

    public CanvasGroup canvasMainGame;

    public CanvasGroup canvasRebirthTab;

    public CanvasGroup canvasResearchTab;

    public CanvasGroup canvasSuitsTab;

    private double[] alienLevel;

    public double[] AlienLevel { get => alienLevel; set => alienLevel = value; }

    private double[] research1Level;

    public double[] Research1Level { get => research1Level; set => research1Level = value; }

    private double[] suitsLevel;

    public double[] SuitsLevel { get => suitsLevel; set => suitsLevel = value; }

    public double[] upgradesCounts;
    public float[] upgradeMaxTime = { 5f, 10f, 10f,20f,35f};
    public float[] progressTimer = { 0f, 0f, 0f, 0f, 0f};


    void Start()
    {
        Application.targetFrameRate = 30;
        mainCurrency = 100;
        rebirthCost = 10000;
        AlienLevel = new double[5];
        SuitsLevel = new double[2];
        upgradesActivated = new bool[unlockingSystem.unlockCost.Length];
        upgradesCounts = new double[AlienLevel.Length];
        Research1Level = new double[2];
        copyArray = new double[AlienLevel.Length];
        alienUpgradeCosts = new double[AlienLevel.Length];
        earnedCrystal = new bool[alienLevel.Length];
        for (int id = 0; id < AlienLevel.Length; id++)
        {
            alienLevel[id] = 0;
        }

        for (int id = 0; id < unlockingSystem.unlockCost.Length; id++)
        {
            upgradesActivated[id] = false;
        }

        Research1Level[0] = 0;
        Research1Level[1] = 0;
        upgradeLevel1 = 0;
        suitsLevel[0] = 0;
        suitsLevel[1] = 0;
        mainResetLevel = 0;
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
        AutoObjectsAssigning();
        CurrencyText.text = ExponentLetterSystem(mainCurrency, "F2");
        RPointsText.text = ExponentLetterSystem(ResearchPointsCalculator(), "F2") + "RP/s ";
        RebirthPrice.text = "Level \n" + ExponentLetterSystem(rebirthCost, "F2");
        RebirthLevel.text = "Rebirth " + mainResetLevel ;
        CrystalsAmount.text = "Crystals: " + crystalCurrency.ToString("F0") ;
        
        for (int id = 0; id < AlienLevelText.Length; id++) {

            AutoValuesAssigning(id, suitsUpgrades.suitsUpgradesCosts, 5, id + 1);
            AutoValuesAssigning(id, upgradesCounts, 0.3, 1.4);
            AutoValuesAssigning(id, alienUpgradeCosts, 3, 8.3);
            ProgressBarsIncomeTimer();
            AlienLevelText[id].text = "Level: " + alienLevel[id].ToString("F0");
            ButtonUpgradeMaxText[id].text = "Buy: " + BuyMaxCount(id) + "\n Price: " + ExponentLetterSystem(BuyCount(id), "F2");
            EarningStage[id].text = ExponentLetterSystem(StageEarningPerSecond(id), "F2") + "RP/s ";
            SoloEarningCrystals(id);
            InteractableButtons(id, upgradeButtons);
        }
        
        StartCoroutine("MySave");
        SaveDate();
    }

    public void SoloEarningCrystals(int id)
    {
        if (alienLevel[id] % 10 == 0 && earnedCrystal[id] == true)
        {
            crystalCurrency++;
            earnedCrystal[id] = false;
        } else if(alienLevel[id] % 10 != 0)
        {
            earnedCrystal[id] = true;
        }
    }
    // Working only for two atm
    public void AutoValuesAssigning(int id, double[] ArrayToIncrease, double baseValue, double valueMultiplier)
    {
            Array.Resize(ref ArrayToIncrease, AlienLevel.Length);
            ArrayToIncrease[id] = valueMultiplier * baseValue;
            //copyArray[id] = ArrayToIncrease[id];
            //ArrayToIncrease[id + 1] = copyArray[id] * valueMultiplier;
    }

    public void AutoObjectsAssigning()
    {
        progressBarObject = GameObject.FindGameObjectsWithTag("progressBars");
        progressBar = new Image[progressBarObject.Length];
        levelStageTextObject = GameObject.FindGameObjectsWithTag("stageLevels");
        AlienLevelText = new Text[levelStageTextObject.Length];
        upgradeButtonObject = GameObject.FindGameObjectsWithTag("upgradeStageButtons");
        upgradeButtons = new Button[upgradeButtonObject.Length];
        buyMaxTextObject = GameObject.FindGameObjectsWithTag("buyMaxStageButtons");
        ButtonUpgradeMaxText = new Text[buyMaxTextObject.Length];
        earningStageObject = GameObject.FindGameObjectsWithTag("barIncomeText");
        EarningStage = new Text[earningStageObject.Length];
        //unlockingSystem.unlockButtons = GameObject.FindGameObjectsWithTag("unlockButtons");
        //unlockingSystem.unlockStage = new Button[unlockingSystem.unlockButtons.Length];
        //unlockUpgradeText = GameObject.FindGameObjectsWithTag("unlockText");
        //unlockingSystem.unlockText = new Text[unlockUpgradeText.Length];
        //unlockGameObjects = GameObject.FindGameObjectsWithTag("upgradeObjects");
        //unlockingSystem.upgradeObjects = new GameObject[unlockGameObjects.Length];

        // looking for all stages on the game
        Stages = GameObject.FindGameObjectsWithTag("Stages");
        for (int id = 0; id < progressBarObject.Length; id++)
        {
            progressBar[id] = progressBarObject[id].GetComponent<Image>();
            AlienLevelText[id] = levelStageTextObject[id].GetComponent<Text>();
            upgradeButtons[id] = upgradeButtonObject[id].GetComponent<Button>();
            ButtonUpgradeMaxText[id] = buyMaxTextObject[id].GetComponent<Text>();
            EarningStage[id] = earningStageObject[id].GetComponent<Text>();
        }

        //for (int id = 0; id < unlockUpgradeText.Length; id++)
        //{
        //    unlockingSystem.unlockStage[id] = unlockingSystem.unlockButtons[id].GetComponent<Button>();
        //    unlockingSystem.unlockText[id] = unlockUpgradeText[id].GetComponent<Text>();
        //    unlockingSystem.upgradeObjects[id] = unlockGameObjects[id];
        //}
    }

    public double ResearchPointsCalculator()
    {
        double temp = 0;
        temp += research.ResearchBoost();
        temp += suitsUpgrades.SuitsBoost();
        temp += RebirthBoost();
        for (int id = 0; id < AlienLevelText.Length; id++)
        {
                    temp += AlienLevel[id] * upgradesCounts[id];

        }
        Debug.Log(temp + "Outcome");
        return temp;
    }

    public void ProgressBarsIncomeTimer()
    {
    
        // If level 0 bar doesn't move
        for (int id = 0; id < progressBarObject.Length; id++)
        {    
            if (AlienLevel[id] >= 1)
            {
                progressTimer[id] += Time.deltaTime;
                progressBar[id].fillAmount = (progressTimer[id] / upgradeMaxTime[id]);

                if (progressTimer[id] >= upgradeMaxTime[id])
                {
                    mainCurrency += ResearchPointsCalculator();
                    progressTimer[id] = 0f;
                }
            }
            else
            {
                progressBar[id].fillAmount = 0;
                progressTimer[id] = 0f;

            }
        }
    }

    public void InteractableButtons(int id, Button[] ButtonStatus)
    {
        if (mainCurrency >= BuyCount(id))
        {
            ButtonStatus[id].interactable = true;
        }
        else
        {
            ButtonStatus[id].interactable = false;
        }
    }

    public double StageEarningPerSecond(int id)
    {
        double temp = 0;

        temp += AlienLevel[id] * upgradesCounts[id];
        return temp;
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
                CanvasGroupMenuSwitch(false, canvasSuitsTab);
                break;

            case "shopMenu":
                CanvasGroupMenuSwitch(false, canvasMainGame);
                CanvasGroupMenuSwitch(true, canvasShop);
                CanvasGroupMenuSwitch(false, canvasRebirthTab);
                CanvasGroupMenuSwitch(false, canvasResearchTab);
                CanvasGroupMenuSwitch(false, canvasSuitsTab);
                break;

            case "rebirth":
                CanvasGroupMenuSwitch(false, canvasMainGame);
                CanvasGroupMenuSwitch(false, canvasShop);
                CanvasGroupMenuSwitch(true, canvasRebirthTab);
                CanvasGroupMenuSwitch(false, canvasResearchTab);
                CanvasGroupMenuSwitch(false, canvasSuitsTab);
                break;

            case "researchMenu":
                CanvasGroupMenuSwitch(false, canvasMainGame);
                CanvasGroupMenuSwitch(false, canvasShop);
                CanvasGroupMenuSwitch(false, canvasRebirthTab);
                CanvasGroupMenuSwitch(true, canvasResearchTab);
                CanvasGroupMenuSwitch(false, canvasSuitsTab);
                break;
            case "suitsMenu":
                CanvasGroupMenuSwitch(false, canvasMainGame);
                CanvasGroupMenuSwitch(false, canvasShop);
                CanvasGroupMenuSwitch(false, canvasRebirthTab);
                CanvasGroupMenuSwitch(false, canvasResearchTab);
                CanvasGroupMenuSwitch(true, canvasSuitsTab);
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
        crystalCurrency = gameData.crystals;
        AlienLevel[0] = gameData.alienLevelData;
        AlienLevel[1] = gameData.alienLevelData2;
        AlienLevel[2] = gameData.alienLevelData3;
        AlienLevel[3] = gameData.alienLevelData4;
        AlienLevel[4] = gameData.alienLevelData5;
        upgradeLevel1 = gameData.upgradeLevelData;
        mainResetLevel = gameData.mainResetLevelData;
        Research1Level[0] = gameData.researchLevel1;
        Research1Level[1] = gameData.researchLevel2;
        upgradesActivated[0] = gameData.upgradeActivated;
        rebirthCost = gameData.rebirthCostData;
        SuitsLevel[0] = gameData.suitsLevel1;
        SuitsLevel[1] = gameData.suitsLevel2;
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
        int crystalsInMax;

        switch (buyModeID)
        {
            case 0:
                n = System.Math.Floor(System.Math.Log(c * (r - 1) / (h * System.Math.Pow(r, u)) + 1, r));
                crystalsInMax = (int)n / 10;
                Debug.Log(crystalsInMax);
                crystalCurrency += crystalsInMax;
                break;
            case 1:
                n = 1;
                break;
            case 2:
                n = 10;
                crystalCurrency++;
                break;
            case 3:
                n = 100;
                crystalCurrency += 10;
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
            alienLevel[0] = 0;
            upgradeLevel1 = 0;
            mainResetLevel++;
            Research1Level[0] = 0;
            Research1Level[1] = 0;
            SuitsLevel[0] = 0;
            SuitsLevel[1] = 0;
            upgradesActivated[0] = false;
            upgradesActivated[1] = false;
            rebirthCost *= (System.Math.Pow(2, mainResetLevel) * (System.Math.Pow(2, 1) - 1) / (2 - 1));
        }
        
    }
    // Deleted +1 on return
    public double RebirthBoost()
    {
      
            double rBoost = 0;
            //for (int id = 0; id < AlienLevel.Length; id++)
            //{
            //    rBoost += AlienLevel[id] * 0.1;
            //}
           
            rBoost += 0.05 * mainResetLevel * 1.7;
            return rBoost;

    }
}
