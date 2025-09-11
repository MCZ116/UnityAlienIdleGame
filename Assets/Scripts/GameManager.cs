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
    public BonusTime bonusTime;
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
    private bool[] activeTab;

    public Image[] progressBar;

    public Button[] upgradeButtons;

    [SerializeField] ResearchManager researchManager;

    [SerializeField] PlanetManager planetManager;

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
    public double[] stageIncome;
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
        mainCurrency = 200;
        rebirthCost = 50000;
        planets = FindObsWithTagAndSortByNumber("planetTab");
        stages = FindObsWithTagAndSortByNumber("Stages");
        
        StageLevel = new double[stages.Length];
        upgradeMaxTime = new float[StageLevel.Length];
        StageMaxTimeCalc();
        progressTimer = new float[stageLevel.Length];
        confirmAstronautBuy = new bool[StageLevel.Length * 4];
        astronautsLevel = new int[stageLevel.Length];
        SuitsLevel = new double[6];
        astronautBuyStartID = new int[stageLevel.Length];       
        stageIncome = new double[StageLevel.Length];
        upgradesActivated = new bool[unlockingSystem.upgradeObjects.Length];
        stageUpgradeCosts = new double[StageLevel.Length];
        earnedCrystal = new bool[StageLevel.Length];

        planetID = 0;
   
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
        
        offline.offlineRewards.SetActive(true);
    }

    void Start()
    {
        if (PlayerPrefs.GetInt("NeverDone", 0) <= 0)
        {
            Save();
            PlayerPrefs.SetInt("NeverDone", 1);
        }
        offline.offlineRewards.SetActive(false);
        ChangeBuyModeText.text = "1";

        //Load after assigning variables and before loading unlock status or it won't appear
        Load();
        
        // Assignign once before update for offline calculations
        for (int id = 0; id < stageLevel.Length; id++)
        {
            AutoValuesAssigning(id, stageUpgradeCosts, 50, 2.07 * id + 1);
            AutoValuesAssigning(id, stageIncome, 5, 1.37);
        }

        astronautBehaviour.AstronautsObjectActivationControl();
        offline.OfflineProgressLoad();
        ChangePlanetTab(0);
    }

    IEnumerator MySave()
    {
        yield return new WaitForSeconds(5f);
        Save();
    }

    void Update()
    {
        QuitButtonAndroid();
        
        CurrencyText.text = ExponentLetterSystem(mainCurrency, "F2");
        RPointsText.text = ExponentLetterSystem(TotalIncome(), "F2") + "/s ";
        RebirthPriceText.text = ExponentLetterSystem(rebirthCost, "F2");
        RebirthLevel.text = "Returns: " + ExponentLetterSystem(mainResetLevel, "F0");
        ProfileLevel.text = ExponentLetterSystem(mainResetLevel, "F0");
        CrystalsAmount.text = crystalCurrency.ToString("F0");
        nickName.text = "Nick: " + PlayerPrefs.GetString("Nick");

        for (int id = 0; id < stageLevel.Length; id++)
        {
            ProgressBarsIncomeTimer();
            StageLevelText[id].text = StageLevel[id].ToString("F0");
            ButtonUpgradeMaxText[id].text = "X" + BuyMaxCount(id) + "\n" + ExponentLetterSystem(BuyCount(id), "F2");
            EarningStage[id].text = ExponentLetterSystem(StageIncomePerSecond(id), "F2") + " " + "/s ";
            SoloEarningCrystals(id);
            InteractableButtons(id, upgradeButtons);
        }

        RebirthButtonStatus();
        RebirthUnlock();
        StartCoroutine("MySave");
        SaveDate();

    }

    public GameObject[] FindObsWithTagAndSortByNumber(string tag)
    {
        GameObject[] foundObs = GameObject.FindGameObjectsWithTag(tag).OrderBy(go => GetStageNumber(go.name)).ToArray();
        return foundObs;
    }

    public GameObject[] FindObsWithTag(string tag)
    {
        GameObject[] foundObs = GameObject.FindGameObjectsWithTag(tag);
        return foundObs;
    }

    int GetStageNumber(string name)
    {
        int startIndex = name.IndexOf('.') - 1;
        while (startIndex >= 0 && char.IsDigit(name[startIndex]))
        {
            startIndex--;
        }
        startIndex++;
        if (int.TryParse(name.Substring(startIndex, name.IndexOf('.') - startIndex), out int result))
        {
            return result;
        }
        return int.MaxValue; 
    }

    public void AutoAssigningObjects()
    {
        StageLevelText = new Text[stageLevel.Length];
        EarningStage = new Text[stageLevel.Length];
        ButtonUpgradeMaxText = new Text[stageLevel.Length];
        upgradeButtons = new Button[stageLevel.Length];
        progressBarObject = new GameObject[stageLevel.Length];
        progressBar = new Image[stageLevel.Length];
        stageObjects = new GameObject[stageLevel.Length];

        for (int id = 0; id < stages.Length; id++)
        {
            stageObjects[id] = stages[id].transform.GetChild(0).Find("StageElements").gameObject;
            progressBarObject[id] = stageObjects[id].transform.Find("ProgressBarBack").GetChild(0).gameObject;
            progressBar[id] = progressBarObject[id].GetComponentInChildren<Image>();

            StageLevelText[id] = stageObjects[id].transform.Find("LevelWindowStage").GetComponentInChildren<Text>();
            EarningStage[id] = stageObjects[id].transform.Find("ProgressBarBack").GetComponentInChildren<Text>();
            ButtonUpgradeMaxText[id] = stageObjects[id].transform.Find("BuyMaxUpgrade").GetComponentInChildren<Text>();
            upgradeButtons[id] = stageObjects[id].transform.Find("BuyMaxUpgrade").GetComponent<Button>();
        }


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

    public double AutoValuesAssigning(int id, double[] ArrayToIncrease, double baseValue, double valueMultiplier)
    {

        if(ArrayToIncrease[0] == 0 && id == 0)
        {
            ArrayToIncrease[id] = baseValue;
        }
        return ArrayToIncrease[id] = baseValue * valueMultiplier * (id + 1);

    }

    public double AutoIncomeAssigning(int id, double[] baseValue, double[] ownedLevels, double valueMultiplier)
    {
        double temp = ((baseValue[id] * ownedLevels[id]) * valueMultiplier);
        return temp;
    }

    public double StageIncomePerSecond(int id)
    {
        double temp = 0;
        temp += AutoIncomeAssigning(id, stageIncome, stageLevel, RebirthBoost());
        temp *= suitsUpgrades.SuitsBoost();
        temp += researchManager.GetTotalIncome(stageIncome[id]);
        return temp;
    }

    public void StageMaxTimeCalc()
    {
        const float baseConstValue = 40; 
        float baseValue = baseConstValue;
        float changeValue = 25;
        
        for (int id = 0; id < stageLevel.Length; id++)
        {
            upgradeMaxTime[id] = baseValue;

            baseValue += changeValue;

            if (id % 4 == 0 && id != 0)
            {
                baseValue = baseConstValue;
            }
        }
    }

    public double TotalIncome()
    {
        double temp = 0;

        for (int id = 0; id < stageLevel.Length; id++)
        {
            temp += StageIncomePerSecond(id);

        }

        return temp * bonusTime.GetIncomeMultiplier();
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
                    mainCurrency += TotalIncome();
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
        if (!planetManager.CanAccessPlanet(planetID))
            return;

        for (int id = 0; id < canvasPlanetsTabs.Length; id++)
        {
            Debug.Log("Checking " + id + " with planetID " + planetID);
            if (id != planetID)
            {
                Debug.Log("Switching off " + id);
                CanvasGroupMenuSwitch(false, canvasPlanetsTabs[id]);
                planets[id].SetActive(false);
            } else
            {
                Debug.Log("Switching on " + id);
                CanvasGroupMenuSwitch(true, canvasPlanetsTabs[id]);
                planets[id].SetActive(true);
            }
        }

        DisableAllTabs();
        CanvasGroupMenuSwitch(true, canvasMainGame);
        this.planetID = planetID;
    }

    public void SwitchPlanetsButtons(string buttonName)
    {
        if (buttonName == "Next")
        {
            int next = planetManager.GetNextUnlockedPlanetId(planetID);
            if (next != -1)
            {
                planetID = next;
                ChangePlanetTab(planetID);
                Debug.Log("Next to " + planetID);
            }
            else Debug.Log("No next unlocked planet");
        }
        else if (buttonName == "Prev")
        {
            int prev = planetManager.GetPrevUnlockedPlanetId(planetID);
            if (prev != -1)
            {
                planetID = prev;
                ChangePlanetTab(planetID);
                Debug.Log("Prev to " + planetID);
            }
            else Debug.Log("No previous unlocked planet");
        }
    }


    public void DisableAllTabs()
    {
        for (int id = 0; id < canvasTabs.Length; id++)
        {
            CanvasGroupMenuSwitch(false, canvasTabs[id]);
            activeTab[id] = false;
        }
    }

    public void Save()
    {

        SaveSystem.SaveGameData(this,researchManager, planetManager);

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

        planetManager.ApplyLoadedData(gameData, planetManager.allPlanets);
        researchManager.ApplyLoadedData(gameData, researchManager.allResearches);
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
        if (mainCurrency >= rebirthCost && researchManager.unlockedResearches.Count >= 6)
        {
            mainCurrency = 100;
            stageLevel[0] = 1;
            upgradeLevel1 = 0;
            mainResetLevel++;

            for (int id = 1; id < stageLevel.Length; id++)
            {
                stageLevel[id] = 0;
            }

            for (int id = 0; id < SuitsLevel.Length; id++)
            {
                SuitsLevel[id] = 0;
            }
        
            for (int id = 0; id < upgradesActivated.Length; id++)
            {
                upgradesActivated[id] = false;
            }

            researchManager.unlockedResearches.Clear();
            planetManager.unlockedPlanets.Clear();

            // First planet always unlocked
            planetManager.unlockedPlanets.Add(planetManager.allPlanets[0]);

            astronautBehaviour.AstronautsObjectActivationControl();
            unlockingSystem.LoadUnlocksStatus();
            rebirthCost *= (System.Math.Pow(2, mainResetLevel) * (System.Math.Pow(2, 1) - 1) / (2 - 1));
            ChangePlanetTab(0);
        }
        
    }

    public double RebirthBoost()
    {
      
        double rBoost = 1;
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
        if (researchManager.unlockedResearches.Count >= 6)
        {
            rebirthRequirements.color = Color.green;
        }
        else
            rebirthRequirements.color = Color.red;
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
}
