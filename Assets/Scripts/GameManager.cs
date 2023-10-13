using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public OfflineProgress offline;
    public AudioControler AC;
    public int buyModeID;
    public Text[] StageLevelText;
    public Text[] EarningStage;
    public Text CurrencyText;
    public Text RPointsText;
    public Text[] ButtonUpgradeMaxText;
    public Text ChangeBuyModeText;
    public Text RebirthPriceText;
    public Text RebirthLevel;
    public Text CrystalsAmount;
    public Text ProfileLevel;
    public Text rebirthRequirements;
    public Text nickName;
    public double mainCurrency;
    public double crystalCurrency;
    public double rebirthCost;

    double[] stageUpgradeCosts;
    public double upgradeLevel1;
    public double mainResetLevel;
    public GameObject[] progressBarObject;
    public GameObject settingsScreenObject;
    public GameObject[] planets;
    public GameObject[] stages;
    public GameObject[] stageObjects;
    public GameObject homeSafeZone;
    public static GameManager instance = null;
    public bool[] upgradesActivated;
    private bool[] earnedCrystal;
    [System.NonSerialized]
    public bool[] confirmAstronautBuy;
    public bool[] planetUnlocked;
    
    private bool[] activeTab;
    double[] copyArray;
    //bool activateRB = false;
    public Image[] progressBar;

    public Button[] upgradeButtons;

    public Research research;

    public AstronautBehaviour astronautBehaviour;

    public SuitsUpgrades suitsUpgrades;

    public UnlockingSystem unlockingSystem;

    public AnimationUnlockSystem unlockingAnimations;

    public CanvasGroup[] canvasPlanetsTabs;

    public CanvasGroup[] canvasTabs;

    public string[] tabsNames = {"gameMenu","shopMenu","researchMenu","rebirth","suitsMenu", "planetsMenu" };

    public CanvasGroup canvasMainGame;

    private double[] stageLevel;

    public double[] StageLevel { get => stageLevel; private set => stageLevel = value; }

    private double[] suitsLevel;

    public double[] SuitsLevel { get => suitsLevel; private set => suitsLevel = value; }

    public int[] astronautsLevel;
    public int[] astronautBuyStartID;
    public double[] upgradesCounts;
    public float[] upgradeMaxTime;
    public float[] progressTimer;

    private int planetID;

    void Awake()
    {

        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
            Destroy(gameObject);

        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 60;
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        mainCurrency = 100;
        rebirthCost = 10000;
        stages = GameObject.FindGameObjectsWithTag("Stages");
        stageObjects = GameObject.FindGameObjectsWithTag("stageObjects");
        StageLevel = new double[stages.Length];
        upgradeMaxTime = new float[StageLevel.Length];
        StageMaxTimeCalc();
        progressTimer = new float[stageLevel.Length];
        confirmAstronautBuy = new bool[StageLevel.Length * 4];
        astronautsLevel = new int[stageLevel.Length];
        SuitsLevel = new double[6];
        astronautBuyStartID = new int[stageLevel.Length];       
        upgradesCounts = new double[StageLevel.Length];
        upgradesActivated = new bool[unlockingSystem.upgradeObjects.Length];
        stageUpgradeCosts = new double[StageLevel.Length];
        earnedCrystal = new bool[StageLevel.Length];
        planetUnlocked = new bool[unlockingSystem.planetsPanelsObjects.Length];
        
        planetUnlocked[0] = false;
        planetID = 0;
   
        planets = GameObject.FindGameObjectsWithTag("planetTab");
        canvasPlanetsTabs = new CanvasGroup[planets.Length];

        for (int id = 0; id < StageLevel.Length; id++)
        {
            progressTimer[id] = 0f;
            StageLevel[id] = 0;
            astronautBuyStartID[id] = id * 4;
            astronautsLevel[id] = 0;
        }

        for (int id = 0; id < unlockingSystem.upgradeObjects.Length; id++)
        {      
            upgradesActivated[id] = false;
        }

        for (int id = 0; id < canvasPlanetsTabs.Length; id++)
        {
            canvasPlanetsTabs[id] = planets[id].GetComponent<CanvasGroup>();
        }

        canvasTabs = homeSafeZone.GetComponentsInChildren<CanvasGroup>()
            .Where(child => child.name.Contains("Menu")).ToArray();

        activeTab = new bool[canvasTabs.Length];
        for (int id = 0; id < canvasTabs.Length; id++)
        {
            activeTab[id] = false;
        }

        for (int id = 0; id < astronautBehaviour.astronautsUpgrades.Length; id++) // add unlockingSystem.unlockCost.Length*4
        {
            confirmAstronautBuy[id] = false;
        }


        upgradeLevel1 = 0;
        suitsLevel[0] = 0;
        suitsLevel[1] = 0;
        mainResetLevel = 1;

        AutoAssigningObjects();
        
        astronautBehaviour.AssigningAstronautsOnStart();                

        if (PlayerPrefs.GetInt("NeverDone", 0) <= 0)
        {
            Save();
            PlayerPrefs.SetInt("NeverDone", 1);
        }

    }

    void Start()
    {

        ChangeBuyModeText.text = "1";

        //Load after assigning variables and before loading unlock status or it won't appear
        Load();

        // Assignign once before update for offline calculations
        for (int id = 0; id < stageLevel.Length; id++)
        {
            AutoValuesAssigning(id, upgradesCounts, 0.3, 1.4);
        }

        astronautBehaviour.AstronautsControl();
        offline.OfflineProgressLoad();
        
    }

    IEnumerator MySave()
    {
        yield return new WaitForSeconds(5f);
        Save();
    }

    void Update()
    {
        QuitButtonAndroid();
        
        unlockingSystem.PlanetsUnlockCheck();
        CurrencyText.text = ExponentLetterSystem(mainCurrency, "F2");
        RPointsText.text = ExponentLetterSystem(ResearchPointsCalculator(), "F2") + "/s ";
        RebirthPriceText.text = ExponentLetterSystem(rebirthCost, "F2");
        RebirthLevel.text = "Returns: " + ExponentLetterSystem(mainResetLevel, "F0");
        ProfileLevel.text = ExponentLetterSystem(mainResetLevel, "F0");
        CrystalsAmount.text = crystalCurrency.ToString("F0");
        nickName.text = "Nick: " + PlayerPrefs.GetString("Nick");

        for (int id = 0; id < stageLevel.Length; id++)
        {
            AutoValuesAssigning(id, upgradesCounts, 0.3, 1.4);
            AutoValuesAssigning(id, stageUpgradeCosts, 20, 2);
            ProgressBarsIncomeTimer();
            StageLevelText[id].text = StageLevel[id].ToString("F0");
            ButtonUpgradeMaxText[id].text = "X" + BuyMaxCount(id) + "\n" + ExponentLetterSystem(BuyCount(id), "F2");
            EarningStage[id].text = ExponentLetterSystem(StageEarningPerSecond(id), "F2") + " " + "/s ";
            SoloEarningCrystals(id);
            InteractableButtons(id, upgradeButtons);
        }

        for (int id = 0; id < suitsLevel.Length; id++)
        {
            AutoValuesAssigning(id, suitsUpgrades.suitsUpgradesCosts, 10000, ((id+1) * 150.32));
        }
        RebirthButtonStatus();
        RebirthUnlock();
        StartCoroutine("MySave");
        SaveDate();

    }

    public void QuitButtonAndroid()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    public void QuitGame()
    {
        Save();
        Application.Quit();
    }

    public void EnterSettings()
    {
        settingsScreenObject.SetActive(true);
    }

    public void ExitScreenButton()
    {
        settingsScreenObject.SetActive(false);
    }

    public void SoloEarningCrystals(int id)
    {
        if (stageLevel[id] % 10 == 0 && earnedCrystal[id] == true)
        {
            crystalCurrency++;
            earnedCrystal[id] = false;
        } else if (stageLevel[id] % 10 != 0)
        {
            earnedCrystal[id] = true;
        }
    }

    public void AutoValuesAssigning(int id, double[] ArrayToIncrease, double baseValue, double valueMultiplier)
    {

        if(ArrayToIncrease[0] == 0 && id == 0)
        {
            ArrayToIncrease[id] = baseValue;
        }
        ArrayToIncrease[id] = baseValue * valueMultiplier * (id+1);

    }
    // New assigning system

    public void AutoAssigningObjects()
    {
        StageLevelText = new Text[stageLevel.Length];
        EarningStage = new Text[stageLevel.Length];
        ButtonUpgradeMaxText = new Text[stageLevel.Length];
        progressBarObject = new GameObject[stageLevel.Length];

        for (int id = 0; id < stageObjects.Length; id++)
        {
            progressBarObject[id] = stageObjects[id].transform.Find("ProgressBarBack").GetChild(0).gameObject;
            progressBar[id] = progressBarObject[id].GetComponentInChildren<Image>();

            StageLevelText[id] = stageObjects[id].transform.Find("LevelWindowStage").GetComponentInChildren<Text>();
            EarningStage[id] = stageObjects[id].transform.Find("ProgressBarBack").GetComponentInChildren<Text>();
            ButtonUpgradeMaxText[id] = stageObjects[id].transform.Find("BuyMaxUpgrade").GetComponentInChildren<Text>();
            upgradeButtons[id] = stageObjects[id].transform.Find("BuyMaxUpgrade").GetComponent<Button>();
        }


    }

    public void ResearchMultiplierCalculator()
    {
        double baseValue = 0.2;
        double increment = 0.3;

        for (int id = 0; id < research.upgradeResearchValues.Length; id++)
        {
            research.upgradeResearchValues[id] = baseValue;
            baseValue += increment;
        }
    }

    public void StageMaxTimeCalc()
    {
        float baseValue = 5;
        float changeValue = 5;
        int multiplier = 0;
        for (int id = 0; id < stageLevel.Length; id++)
        {
            upgradeMaxTime[id] = baseValue;

            if (upgradeMaxTime[id] % (15 + multiplier) == 0)
            {
                changeValue = 10;
            }

            baseValue = baseValue + changeValue;

            if (upgradeMaxTime[id] % (35 + multiplier) == 0)
            {
                multiplier++;
                baseValue = 5 + multiplier;
                changeValue = 5;       
            }
        }
    }

    public double ResearchPointsCalculator()
    {
        double temp = 0;

        for (int id = 0; id < stageLevel.Length; id++)
        {
            temp += (stageLevel[id] * upgradesCounts[id]);

        }

        temp += research.ResearchBoost();
        temp += astronautBehaviour.AstronautsBoost();
        temp += suitsUpgrades.SuitsBoost();
        temp += RebirthBoost();

        return temp;
    }

    public void ProgressBarsIncomeTimer()
    {

        // If level 0 bar doesn't move
        for (int id = 0; id < progressBarObject.Length; id++)
        {
            if (stageLevel[id] >= 1)
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
        if (mainCurrency >= BuyCount(id) && BuyCount(id) != 0)
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

        temp += stageLevel[id] * upgradesCounts[id];
        temp += astronautBehaviour.AstronautsBoostStage(id);
        return temp;
    }

    public static string ExponentLetterSystem(double value, string numberToString)
    {

        if (value <= 1000) return value.ToString(numberToString);

        var exponentSci = Math.Floor(Math.Log10(value));
        var exponentEng = 3 * Math.Floor(exponentSci / 3);
        var letterOne = ((char)Math.Floor((((double)exponentEng - 3) / 3) % 26 + 97)).ToString();

        if ((double)exponentEng / 3 >= 27)
        {
            var letterTwo = ((char)(Math.Floor(((double)exponentEng - 3 * 26) / (3 * 26)) % 26 + 97)).ToString();
            return (value / Math.Pow(10, exponentEng)).ToString(numberToString) + letterTwo + letterOne;
        }
        if (value > 1000)
        {
            return (value / Math.Pow(10, exponentEng)).ToString(numberToString) + letterOne;

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

        for (int id = 0; id < canvasTabs.Length; id++)
        {
            if (tabName.Equals(tabsNames[id]) && !activeTab[id])
            {
                CanvasGroupMenuSwitch(true, canvasTabs[id]);
                activeTab[id] = true;
            }
            else if(!tabName.Equals(tabsNames[id]))
            {
                CanvasGroupMenuSwitch(false, canvasTabs[id]);
                activeTab[id] = false;
            }
            else if (tabName.Equals(tabsNames[id]) && activeTab[id])
            {
                CanvasGroupMenuSwitch(false, canvasTabs[id]);
                activeTab[id] = false;
                CanvasGroupMenuSwitch(true, canvasMainGame);
            }
        }
    }

    public void ChangePlanetTab(int planetID)
    {
        Enumerable.Range(0, canvasPlanetsTabs.Length)
            .Where(id => id != planetID && planetUnlocked[planetID])
            .ToList()
            .ForEach(id => CanvasGroupMenuSwitch(false, canvasPlanetsTabs[id]));

        for (int id = 0; id < canvasTabs.Length; id++)
        {
            CanvasGroupMenuSwitch(false, canvasTabs[id]);
            activeTab[id] = false;
        }

        CanvasGroupMenuSwitch(true, canvasPlanetsTabs[planetID]);
        CanvasGroupMenuSwitch(true, canvasMainGame);
        this.planetID = planetID;
    }

    public void SwitchPlanetsButtons(string buttonName)
    {
        if (buttonName == "Next" && planetID < planetUnlocked.Length && planetUnlocked[planetID] == true)
        {
            planetID += 1;
            ChangePlanetTab(planetID);

        } else if(buttonName == "Prev" && planetID < planetUnlocked.Length && planetID != 0)
        {

            planetID -= 1;
            ChangePlanetTab(planetID); 
        }
    }

    public void Save()
    {

        SaveSystem.SaveGameData(this,research);

    }

    public void Load()
    {
        GameData gameData = SaveSystem.LoadData();

        mainCurrency = gameData.researchPointsData;
        crystalCurrency = gameData.crystals;

        for (int id = 0; id < stageLevel.Length; id++)
        {
            StageLevel[id] = gameData.stageLevelData[id];
            astronautsLevel[id] = gameData.astronautsLevel[id];
            astronautBuyStartID[id] = gameData.astronautIDStartPosition[id];
        }

        upgradeLevel1 = gameData.upgradeLevelData;
        mainResetLevel = gameData.mainResetLevelData;

        for (int id = 0; id < research.ResearchLevel.Length; id++)
        {
            research.ResearchLevel[id] = gameData.researchLevel[id];
            research.researchCanBeDone[id] = gameData.researchCanBeDone[id];
            research.researchUnlocked[id] = gameData.researchUnlocked[id];
        }

        for (int id = 0; id < upgradesActivated.Length; id++)
        {
            upgradesActivated[id] = gameData.upgradeActivated[id];
        }

        for (int id = 0; id < confirmAstronautBuy.Length; id++)
        {
            confirmAstronautBuy[id] = gameData.astronautsbuy[id];
        }

        rebirthCost = gameData.rebirthCostData;

        for (int id = 0; id < SuitsLevel.Length; id++)
        {
            SuitsLevel[id] = gameData.suitsLevel[id];
        }


        for (int id = 0; id < planetUnlocked.Length; id++)
        {
            planetUnlocked[id] = gameData.planetUnlocked[id];
        }
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
                ChangeBuyModeText.text = "1";
                buyModeID = 1;
                break;
            case 1:
                ChangeBuyModeText.text = "10";
                buyModeID = 2;
                break;
            case 2:
                ChangeBuyModeText.text = "100";
                buyModeID = 3;
                break;
            case 3:
                ChangeBuyModeText.text = "MAX";
                buyModeID = 0;
                break;
        }
    }

    public double BuyCount(int id)
    {
        var h = stageUpgradeCosts[id];
        var c = mainCurrency;
        var r = 1.07;
        var u = StageLevel[id];
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
        var h = stageUpgradeCosts[id];
        var c = mainCurrency;
        var r = 1.07;
        var u = StageLevel[id];
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
            var h = stageUpgradeCosts[id];
            var c = mainCurrency;
            var r = 1.07;
            var u = StageLevel[id];

        double n = 0;
        int crystalsInMax;

        switch (buyModeID)
        {
            case 0:
                n = System.Math.Floor(System.Math.Log(c * (r - 1) / (h * System.Math.Pow(r, u)) + 1, r));
                crystalsInMax = (int)n / 10;
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
              stageLevel[id] += (int)n;
              mainCurrency -= costUpgrade;
              upgradeLevel1 += (int)n;
            }

        AC.ButtonClickSound();
    }

    public void FullReset()
    {
        if (mainCurrency >= rebirthCost && research.researchUnlocked[0])
        {
            mainCurrency = 100;
            stageLevel[0] = 1;
            upgradeLevel1 = 0;
            mainResetLevel++;

            for (int id = 1; id < stageLevel.Length; id++)
            {
                stageLevel[id] = 0;
            }

            for (int id = 0; id < research.ResearchLevel.Length; id++)
            {
                research.ResearchLevel[id] = 0;
            }
            
            for (int id = 0; id < SuitsLevel.Length; id++)
            {
                SuitsLevel[id] = 0;
            }
        
            for (int id = 0; id < upgradesActivated.Length; id++)
            {
                upgradesActivated[id] = false;
            }

            for (int id = 0; id < research.researchCanBeDone.Length; id++)
            {
                research.researchCanBeDone[id] = false;
                research.researchUnlocked[id] = false;
            }

            for (int id = 0; id < planetUnlocked.Length; id++)
            {
                planetUnlocked[id] = false;
            }

            astronautBehaviour.AstronautsControl();
            unlockingSystem.LoadUnlocksStatus();
            unlockingSystem.PlanetsUnlockCheck();
            rebirthCost *= (System.Math.Pow(2, mainResetLevel) * (System.Math.Pow(2, 1) - 1) / (2 - 1));
            ChangePlanetTab(0);
        }
        
    }

    public double RebirthBoost()
    {
      
        double rBoost = 0;
        // secure to not give bonus before going higher than 1 lvl of rebirth
        if (mainResetLevel != 1)
        {
            rBoost += 0.5 * mainResetLevel * 1.7;
            return rBoost;
        } 
        return rBoost;

    }

    public void RebirthButtonStatus()
    {
        if (mainCurrency >= rebirthCost)
        {
            RebirthPriceText.color = Color.green;
        }
        else
            RebirthPriceText.color = Color.red;
    }

    //TODO
    public void RebirthUnlock()
    {
        if (research.researchUnlocked[1])
        {
            rebirthRequirements.color = Color.green;
        }
        else
            rebirthRequirements.color = Color.red;
    }
}
