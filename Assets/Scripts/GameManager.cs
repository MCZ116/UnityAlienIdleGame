using System;
using System.Collections;
using System.Collections.Generic;
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
    public List<GameObject> allObjects = new List<GameObject>();
    public static GameManager instance = null;
    public bool[] upgradesActivated;
    private bool[] earnedCrystal;
    public bool[] confirmAstronautBuy;
    public bool[] planetUnlocked;
    public bool[] researchUnlocked;
    private bool activeTab;
    double[] copyArray;
    //bool activateRB = false;
    public Image[] progressBar;

    public Button[] upgradeButtons;

    public Research research;

    public AstronautBehaviour astronautBehaviour;

    public SuitsUpgrades suitsUpgrades;

    public UnlockingSystem unlockingSystem;

    public UnlockingAnimations unlockingAnimations;

    public CanvasGroup canvasShop;

    public CanvasGroup canvasMainGame;

    public CanvasGroup canvasRebirthTab;

    public CanvasGroup canvasResearchTab;

    public CanvasGroup canvasSuitsTab;

    public CanvasGroup moonMenu;

    public CanvasGroup marsMenu;

    public CanvasGroup planetsMenu;

    public CanvasGroup phobosMenu;

    private double[] stageLevel;

    public double[] StageLevel { get => stageLevel; set => stageLevel = value; }

    private double[] research1Level;

    public double[] Research1Level { get => research1Level; set => research1Level = value; }

    private double[] suitsLevel;

    public double[] SuitsLevel { get => suitsLevel; set => suitsLevel = value; }

    public int[] astronautsLevel;
    public int[] astronautBuyStartID;
    public double[] upgradesCounts;
    public float[] upgradeMaxTime = { 5f, 10f, 10f, 20f, 35f, 5f, 10f, 10f, 20f, 35f, 5f, 10f, 10f, 20f, 35f };
    public float[] progressTimer;
    public bool[] researchCanBeDone;
    public int researchID;
    private int planetID;

    void Awake()
    {

        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
            Destroy(gameObject);

        Debug.Log("GameManagerStarted!");
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 60;
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        mainCurrency = 100;
        rebirthCost = 10000;
        unlockingSystem.unlockCost = new double[14];
        unlockingSystem.unlockCost[0] = 2000;
        unlockingSystem.unlockCost[1] = 4000;
        unlockingSystem.unlockCost[2] = 8000;
        unlockingSystem.unlockCost[3] = 20000;
        unlockingSystem.unlockCost[4] = 100000;
        unlockingSystem.unlockCost[5] = 135000;
        unlockingSystem.unlockCost[6] = 160000;
        unlockingSystem.unlockCost[7] = 180000;
        unlockingSystem.unlockCost[8] = 220000;
        unlockingSystem.unlockCost[9] = 2135000;
        unlockingSystem.unlockCost[10] = 2160000;
        unlockingSystem.unlockCost[11] = 2180000;
        unlockingSystem.unlockCost[12] = 21220000;
        unlockingSystem.unlockCost[13] = 35000000;
        unlockingSystem.planetCost = new double[2];
        unlockingSystem.planetCost[0] = 220000;
        unlockingSystem.planetCost[1] = 10000000;
        progressTimer = new float[15];
        activeTab = false;
        StageLevel = new double[15];
        Research1Level = new double[4];
        astronautsLevel = new int[stageLevel.Length];
        SuitsLevel = new double[6];
        astronautBuyStartID = new int[stageLevel.Length];
        researchUnlocked = new bool[Research1Level.Length];
        unlockingSystem.animationUnlockConfirm = new bool[14];
        upgradesCounts = new double[StageLevel.Length];
        upgradesActivated = new bool[unlockingSystem.unlockCost.Length];
        copyArray = new double[StageLevel.Length + 1];
        stageUpgradeCosts = new double[StageLevel.Length];
        earnedCrystal = new bool[StageLevel.Length];
        planetUnlocked = new bool[unlockingSystem.planetsPanelsObjects.Length];
        researchCanBeDone = new bool[Research1Level.Length];
        research.upgradeResearchValues = new double[research1Level.Length];
        research.upgradeResearchValues[0] = 0.5;
        research.upgradeResearchValues[1] = 0.8;
        research.upgradeResearchValues[2] = 2;
        research.upgradeResearchValues[3] = 2.2;
        planetUnlocked[0] = false;
        researchID = 0;
        planetID = 0;
        // Here was AutoObjectAssigning


        for (int id = 0; id < StageLevel.Length; id++)
        {
            progressTimer[id] = 0f;
            StageLevel[id] = 0;
            astronautBuyStartID[id] = id * 4;
            astronautsLevel[id] = 0;
            //alienUpgradeCosts[id] = 50 + id * 0.2;
            Debug.Log("astroLevelBeforeLoad = " + astronautsLevel.Length);
            Debug.Log("alienlevelBeforeLoad = " + StageLevel.Length);
            Debug.Log("Loaded Assignig");
        }

        for (int id = 0; id < unlockingSystem.animationUnlockConfirm.Length; id++)
        {
            unlockingSystem.animationUnlockConfirm[id] = false;
            upgradesActivated[id] = false;
        }

        for (int id = 0; id < researchUnlocked.Length; id++)
        {
            researchUnlocked[id] = false;
            researchCanBeDone[id] = false;
        }

        for (int id = 0; id < Research1Level.Length; id++)
        {
            Research1Level[id] = 0;
        }

        upgradeLevel1 = 0;
        suitsLevel[0] = 0;
        suitsLevel[1] = 0;
        mainResetLevel = 1;

        //AutoObjectsAssigning();
        //unlockingAnimations.AnimationGateAssigning();
        astronautBehaviour.AssigningAstronautsOnStart();                
        Debug.Log("Price for level = " + stageUpgradeCosts[0]);
        Debug.Log("UpgradeCounts = " + upgradesCounts[0]);

    }

    void Start()
    {

        for (int id = 0; id < astronautBehaviour.astronautsUpgrades.Length; id++) // add unlockingSystem.unlockCost.Length*4
        {
            confirmAstronautBuy[id] = false;
        }

        ChangeBuyModeText.text = "1";

        Debug.Log(confirmAstronautBuy.Length + "Astronauts amount");
        //Load after assigning variables and before loading unlock status or it won't appear
        Load();
        Debug.Log("Loaded");
        // Assignign once before update for offline calculations
        for (int id = 0; id < stageLevel.Length; id++)
        {
            AutoValuesAssigning(id, upgradesCounts, 0.3, 1.4);
        }

        for (int id = 0; id < astronautsLevel.Length; id++)
        {
            Debug.Log("astroLevelAfterLoad = " + astronautsLevel.Length);
        }
        AutoAssigningObjects();
        astronautBehaviour.AstronautsControl();
        unlockingSystem.PlanetsUnlockCheck();
        unlockingSystem.LoadUnlocksStatus();
        offline.OfflineProgressLoad();
        Debug.Log(" AstroLenght: " + StageLevel.Length);
        
    }

    IEnumerator MySave()
    {
        yield return new WaitForSeconds(5f);
        Save();
    }

    void Update()
    {
        QuitButtonAndroid();
        //saving here because of bug which causing not creating save file after deleting it cuz IEnumerator won't have time to work
        Save();
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
        for (int id = 0; id < stageLevel.Length-1; id++)
        {
            progressBarObject[id+1] = unlockingSystem.upgradeObjects[id].transform.Find("ProgressBarBack").GetChild(0).gameObject;
            progressBar[id+1] = progressBarObject[id+1].GetComponentInChildren<Image>();
            Debug.Log(progressBarObject.Length + " - " + progressBar.Length + " - " + id);
        }


    }

    public double ResearchPointsCalculator()
    {
        double temp = 0;

        for (int id = 0; id < stageLevel.Length; id++)
        {
            temp += (stageLevel[id] * upgradesCounts[id]);
            Debug.Log("ID: " + stageLevel[id] + " UpgradeCount: " + upgradesCounts[id]);
            Debug.Log(" Astronaut Earning temp: " + temp);
        }

        temp += research.ResearchBoost();
        temp += astronautBehaviour.AstronautsBoost();
        temp += suitsUpgrades.SuitsBoost();
        temp += RebirthBoost();

        Debug.Log(temp + "Outcome");
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
            Debug.Log(activeTab + "ON" + choosenGroup);
        }
        else
        {
            choosenGroup.alpha = 0;
            choosenGroup.interactable = false;
            choosenGroup.blocksRaycasts = false;
            Debug.Log(activeTab + "OFF" + choosenGroup);
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
                CanvasGroupMenuSwitch(false, planetsMenu);
                break;

            case "shopMenu":
                CanvasGroupMenuSwitch(false, canvasMainGame);
                CanvasGroupMenuSwitch(true, canvasShop);
                CanvasGroupMenuSwitch(false, canvasRebirthTab);
                CanvasGroupMenuSwitch(false, canvasResearchTab);
                CanvasGroupMenuSwitch(false, canvasSuitsTab);
                CanvasGroupMenuSwitch(false, planetsMenu);

                break;

            case "rebirth":
                if (!activeTab)
                {
                    CanvasGroupMenuSwitch(false, canvasMainGame);
                    CanvasGroupMenuSwitch(false, canvasShop);
                    CanvasGroupMenuSwitch(true, canvasRebirthTab);
                    CanvasGroupMenuSwitch(false, canvasResearchTab);
                    CanvasGroupMenuSwitch(false, canvasSuitsTab);
                    CanvasGroupMenuSwitch(false, planetsMenu);
                    activeTab = true;
                }
                else
                {
                    CanvasGroupMenuSwitch(false, canvasShop);
                    CanvasGroupMenuSwitch(false, canvasResearchTab);
                    CanvasGroupMenuSwitch(false, canvasSuitsTab);
                    CanvasGroupMenuSwitch(true, canvasMainGame);
                    CanvasGroupMenuSwitch(false, canvasRebirthTab);
                    CanvasGroupMenuSwitch(false, planetsMenu);
                    activeTab = false;
                }
                break;

            case "researchMenu":
                if (!activeTab)
                {
                    CanvasGroupMenuSwitch(false, canvasMainGame);
                    CanvasGroupMenuSwitch(false, canvasShop);
                    CanvasGroupMenuSwitch(false, canvasRebirthTab);
                    CanvasGroupMenuSwitch(true, canvasResearchTab);
                    CanvasGroupMenuSwitch(false, canvasSuitsTab);
                    CanvasGroupMenuSwitch(false, planetsMenu);
                    activeTab = true;
                }
                else
                {
                    CanvasGroupMenuSwitch(false, canvasResearchTab);
                    CanvasGroupMenuSwitch(true, canvasMainGame);
                    CanvasGroupMenuSwitch(false, canvasShop);
                    CanvasGroupMenuSwitch(false, canvasRebirthTab);
                    CanvasGroupMenuSwitch(false, canvasSuitsTab);
                    CanvasGroupMenuSwitch(false, planetsMenu);
                    activeTab = false;
                }
                break;

            case "suitsMenu":
                if (!activeTab)
                {
                    CanvasGroupMenuSwitch(false, canvasMainGame);
                    CanvasGroupMenuSwitch(false, canvasShop);
                    CanvasGroupMenuSwitch(false, canvasRebirthTab);
                    CanvasGroupMenuSwitch(false, canvasResearchTab);
                    CanvasGroupMenuSwitch(true, canvasSuitsTab);
                    CanvasGroupMenuSwitch(false, planetsMenu);
                    activeTab = true;
                } else
                {
                    CanvasGroupMenuSwitch(false, canvasShop);
                    CanvasGroupMenuSwitch(false, canvasRebirthTab);
                    CanvasGroupMenuSwitch(false, canvasResearchTab);
                    CanvasGroupMenuSwitch(false, canvasSuitsTab);
                    CanvasGroupMenuSwitch(true, canvasMainGame);
                    CanvasGroupMenuSwitch(false, planetsMenu);
                    activeTab = false;
                }

                break;

            case "planetsMenu":
                if (!activeTab)
                {
                    CanvasGroupMenuSwitch(false, canvasMainGame);
                    CanvasGroupMenuSwitch(false, canvasShop);
                    CanvasGroupMenuSwitch(false, canvasRebirthTab);
                    CanvasGroupMenuSwitch(false, canvasResearchTab);
                    CanvasGroupMenuSwitch(false, canvasSuitsTab);
                    CanvasGroupMenuSwitch(true, planetsMenu);
                    activeTab = true;
                }
                else
                {
                    CanvasGroupMenuSwitch(true, canvasMainGame);
                    CanvasGroupMenuSwitch(false, canvasShop);
                    CanvasGroupMenuSwitch(false, canvasRebirthTab);
                    CanvasGroupMenuSwitch(false, canvasResearchTab);
                    CanvasGroupMenuSwitch(false, canvasSuitsTab);
                    CanvasGroupMenuSwitch(false, planetsMenu);
                    activeTab = false;
                }
                break;

        } 
    }

    public void ChangePlanetTab(int planetID)
    {
        switch (planetID)
        {
            case 0:
                CanvasGroupMenuSwitch(true, canvasMainGame);
                CanvasGroupMenuSwitch(false, canvasShop);
                CanvasGroupMenuSwitch(false, canvasRebirthTab);
                CanvasGroupMenuSwitch(false, canvasResearchTab);
                CanvasGroupMenuSwitch(false, canvasSuitsTab);
                CanvasGroupMenuSwitch(true, moonMenu);
                CanvasGroupMenuSwitch(false, marsMenu);
                CanvasGroupMenuSwitch(false, planetsMenu);
                CanvasGroupMenuSwitch(false, phobosMenu);
                activeTab = false;
                break;

            case 1:
                if (planetUnlocked[0])
                {
                    CanvasGroupMenuSwitch(true, canvasMainGame);
                    CanvasGroupMenuSwitch(false, canvasShop);
                    CanvasGroupMenuSwitch(false, canvasRebirthTab);
                    CanvasGroupMenuSwitch(false, canvasResearchTab);
                    CanvasGroupMenuSwitch(false, canvasSuitsTab);
                    CanvasGroupMenuSwitch(false, moonMenu);
                    CanvasGroupMenuSwitch(false, planetsMenu);
                    CanvasGroupMenuSwitch(true, marsMenu);
                    CanvasGroupMenuSwitch(false, phobosMenu);
                    activeTab = false;
                }
                break;

            case 2:
                if (planetUnlocked[1])
                {
                    CanvasGroupMenuSwitch(true, canvasMainGame);
                    CanvasGroupMenuSwitch(false, canvasShop);
                    CanvasGroupMenuSwitch(false, canvasRebirthTab);
                    CanvasGroupMenuSwitch(false, canvasResearchTab);
                    CanvasGroupMenuSwitch(false, canvasSuitsTab);
                    CanvasGroupMenuSwitch(false, moonMenu);
                    CanvasGroupMenuSwitch(false, planetsMenu);
                    CanvasGroupMenuSwitch(false, marsMenu);
                    CanvasGroupMenuSwitch(true, phobosMenu);
                    activeTab = false;
                }
                break;
        }
    }

    public void SwitchPlanetsButtons(string buttonName)
    {
        
        if(buttonName == "Next" && planetID < planetUnlocked.Length && planetUnlocked[planetID] == true)
        {
            planetID += 1;
            ChangePlanetTab(planetID);
            Debug.Log(planetID + " " + planetUnlocked.Length);
        } else if(buttonName == "Prev" && planetID <= planetUnlocked.Length && planetID != 0)
        {
            planetID -= 1;
            ChangePlanetTab(planetID);
            Debug.Log(planetID);
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

        for (int id = 0; id < stageLevel.Length; id++)
        {
            StageLevel[id] = gameData.stageLevelData[id];
            astronautsLevel[id] = gameData.astronautsLevel[id];
            astronautBuyStartID[id] = gameData.astronautIDStartPosition[id];
        }

        upgradeLevel1 = gameData.upgradeLevelData;
        mainResetLevel = gameData.mainResetLevelData;

        for (int id = 0; id < Research1Level.Length; id++)
        {
            Research1Level[id] = gameData.researchLevel[id];
            researchCanBeDone[id] = gameData.researchCanBeDone[id];
            researchUnlocked[id] = gameData.researchUnlocked[id];
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

        researchID = gameData.researchID;
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
                Debug.Log(crystalsInMax);
                crystalCurrency += crystalsInMax;
                break;
            case 1:
                n = 1;
                Debug.Log("Bought 1 of ID = " + id);
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
        if (mainCurrency >= rebirthCost && researchUnlocked[0])
        {
            mainCurrency = 100;
            stageLevel[0] = 1;
            upgradeLevel1 = 0;
            mainResetLevel++;
            researchID = 0;

            for (int id = 1; id < stageLevel.Length; id++)
            {
                stageLevel[id] = 0;
            }

            for (int id = 0; id < Research1Level.Length; id++)
            {
                Research1Level[id] = 0;
            }
            
            for (int id = 0; id < SuitsLevel.Length; id++)
            {
                SuitsLevel[id] = 0;
            }
        
            for (int id = 0; id < upgradesActivated.Length; id++)
            {
                upgradesActivated[id] = false;
                unlockingSystem.animationUnlockConfirm[id] = false;
            }

            for (int id = 0; id < researchCanBeDone.Length; id++)
            {
                researchCanBeDone[id] = false;
                researchUnlocked[id] = false;
            }

            for (int id = 0; id < planetUnlocked.Length; id++)
            {
                planetUnlocked[id] = false;
            }

            for (int id = 0; id < confirmAstronautBuy.Length; id++)
            {
                confirmAstronautBuy[id] = false;
            }

            for (int id = 0; id < astronautsLevel.Length; id++)
            {
                astronautsLevel[id] = 0;
                astronautBehaviour.astronautMaxConfirm[id] = false;
                astronautBuyStartID[id] = id * 4;
            }

            astronautBehaviour.AstronautsControl();
            unlockingSystem.LoadUnlocksStatus();
            unlockingSystem.PlanetsUnlockCheck();
            rebirthCost *= (System.Math.Pow(2, mainResetLevel) * (System.Math.Pow(2, 1) - 1) / (2 - 1));
        }
        
    }

    public double RebirthBoost()
    {
      
        double rBoost = 0;
        // secure to not give bonus before going higher than 1 lvl of rebirth
        if (mainResetLevel != 1)
        {
            rBoost += 0.05 * mainResetLevel * 1.7;
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

    public void RebirthUnlock()
    {
        if (researchUnlocked[1])
        {
            rebirthRequirements.color = Color.green;
        }
        else
            rebirthRequirements.color = Color.red;
    }
}
