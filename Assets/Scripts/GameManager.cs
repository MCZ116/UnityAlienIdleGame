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
    public Text RebirthPrice;
    public Text RebirthLevel;
    public Text CrystalsAmount;
    public Text ProfileLevel;
    public double mainCurrency;
    public double crystalCurrency;
    public double rebirthCost;

    double[] stageUpgradeCosts;
    public double upgradeLevel1;
    public double mainResetLevel;
    public GameObject[] progressBarObject;
    public GameObject settingsScreenObject;
    public GameObject[] levelStageTextObject;
    public GameObject[] upgradeButtonObject;
    public GameObject[] buyMaxTextObject;
    public GameObject[] earningStageObject;
    public GameObject[] Stages;
    private GameObject[] unlockUpgradeText;
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

    private double[] stageLevel;

    public double[] StageLevel { get => stageLevel; set => stageLevel = value; }

    private double[] research1Level;

    public double[] Research1Level { get => research1Level; set => research1Level = value; }

    private double[] suitsLevel;

    public double[] SuitsLevel { get => suitsLevel; set => suitsLevel = value; }

    public int[] astronautsLevel;
    public int[] astronautBuyStartID;
    public double[] upgradesCounts;
    public float[] upgradeMaxTime = { 5f, 10f, 10f, 20f, 35f, 5f, 10f, 10f, 20f, 35f };
    public float[] progressTimer;
    public bool[] researchCanBeDone;
    public int researchID;

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
        unlockingSystem.unlockCost = new double[9];
        unlockingSystem.unlockCost[0] = 2000;
        unlockingSystem.unlockCost[1] = 4000;
        unlockingSystem.unlockCost[2] = 8000;
        unlockingSystem.unlockCost[3] = 20000;
        unlockingSystem.unlockCost[4] = 100000;
        unlockingSystem.unlockCost[5] = 135000;
        unlockingSystem.unlockCost[6] = 160000;
        unlockingSystem.unlockCost[7] = 180000;
        unlockingSystem.unlockCost[8] = 220000;
        unlockingSystem.planetCost = new double[1];
        unlockingSystem.planetCost[0] = 220000;
        progressTimer = new float[10];
        activeTab = false;
        StageLevel = new double[10];
        Research1Level = new double[4];
        astronautsLevel = new int[stageLevel.Length];
        SuitsLevel = new double[6];
        astronautBuyStartID = new int[stageLevel.Length];
        researchUnlocked = new bool[Research1Level.Length];
        unlockingSystem.animationUnlockConfirm = new bool[9];
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
        Debug.Log("ProgressBarLenght = " + progressBarObject.Length);

    }

    void Start()
    {

        for (int id = 0; id < astronautBehaviour.astronautsUpgrades.Length; id++) // add unlockingSystem.unlockCost.Length*4
        {
            confirmAstronautBuy[id] = false;
        }

        ChangeBuyModeText.text = "1";

        //Load after assigning variables and before loading unlock status or it won't appear
        Load();
        // Assignign once before update for offline calculations
        for (int id = 0; id < stageLevel.Length; id++)
        {
            AutoValuesAssigning(id, upgradesCounts, 0.3, 1.4);
        }

        for (int id = 0; id < astronautsLevel.Length; id++)
        {
            Debug.Log("astroLevelAfterLoad = " + astronautsLevel.Length);
        }
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
        RebirthPrice.text = ExponentLetterSystem(rebirthCost, "F2");
        RebirthLevel.text = "Returns: " + ExponentLetterSystem(mainResetLevel, "F0");
        ProfileLevel.text = ExponentLetterSystem(mainResetLevel, "F0");
        CrystalsAmount.text = crystalCurrency.ToString("F0");

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

    //public void AutoObjectsAssigning()
    //{
    //    progressBarObject = GameObject.FindGameObjectsWithTag("progressBars");
    //    progressBar = new Image[progressBarObject.Length];
    //    levelStageTextObject = GameObject.FindGameObjectsWithTag("stageLevels");
    //    AlienLevelText = new Text[levelStageTextObject.Length];
    //    upgradeButtonObject = GameObject.FindGameObjectsWithTag("upgradeStageButtons");
    //    upgradeButtons = new Button[upgradeButtonObject.Length];
    //    buyMaxTextObject = GameObject.FindGameObjectsWithTag("buyMaxStageButtons");
    //    ButtonUpgradeMaxText = new Text[buyMaxTextObject.Length];
    //    earningStageObject = GameObject.FindGameObjectsWithTag("barIncomeText");
    //    EarningStage = new Text[earningStageObject.Length];
    //    Stages = GameObject.FindGameObjectsWithTag("Stages");

    //    for (int id = 0; id < alienLevel.Length; id++)
    //    {
    //        progressBar[id] = progressBarObject[id].GetComponent<Image>();
    //        AlienLevelText[id] = levelStageTextObject[id].GetComponent<Text>();
    //        upgradeButtons[id] = upgradeButtonObject[id].GetComponent<Button>();
    //        ButtonUpgradeMaxText[id] = buyMaxTextObject[id].GetComponent<Text>();
    //        EarningStage[id] = earningStageObject[id].GetComponent<Text>();
    //    }
    //}
    // Important part of code calculating all income and bonuses in game
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

            case "moonMenu":
                CanvasGroupMenuSwitch(true, canvasMainGame);
                CanvasGroupMenuSwitch(false, canvasShop);
                CanvasGroupMenuSwitch(false, canvasRebirthTab);
                CanvasGroupMenuSwitch(false, canvasResearchTab);
                CanvasGroupMenuSwitch(false, canvasSuitsTab);
                CanvasGroupMenuSwitch(true, moonMenu);
                CanvasGroupMenuSwitch(false, marsMenu);
                CanvasGroupMenuSwitch(false, planetsMenu);
                activeTab = false;
                break;

            case "marsMenu":
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
                        activeTab = false;
                }                    
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
        StageLevel[0] = gameData.alienLevelData;
        StageLevel[1] = gameData.alienLevelData2;
        StageLevel[2] = gameData.alienLevelData3;
        StageLevel[3] = gameData.alienLevelData4;
        StageLevel[4] = gameData.alienLevelData5;
        StageLevel[5] = gameData.alienLevelData6;
        StageLevel[6] = gameData.alienLevelData7;
        StageLevel[7] = gameData.alienLevelData8;
        StageLevel[8] = gameData.alienLevelData9;
        StageLevel[9] = gameData.alienLevelData10;
        upgradeLevel1 = gameData.upgradeLevelData;
        mainResetLevel = gameData.mainResetLevelData;
        Research1Level[0] = gameData.researchLevel1;
        Research1Level[1] = gameData.researchLevel2;
        Research1Level[2] = gameData.researchLevel3;
        Research1Level[3] = gameData.researchLevel4;
        upgradesActivated[0] = gameData.upgradeActivated;
        upgradesActivated[1] = gameData.upgradeActivated2;
        upgradesActivated[2] = gameData.upgradeActivated3;
        upgradesActivated[3] = gameData.upgradeActivated4;
        upgradesActivated[4] = gameData.upgradeActivated5;
        upgradesActivated[5] = gameData.upgradeActivated6;
        upgradesActivated[6] = gameData.upgradeActivated7;
        upgradesActivated[7] = gameData.upgradeActivated8;
        upgradesActivated[8] = gameData.upgradeActivated9;
        confirmAstronautBuy[0] = gameData.astronautsbuy1;
        confirmAstronautBuy[1] = gameData.astronautsbuy2;
        confirmAstronautBuy[2] = gameData.astronautsbuy3;
        confirmAstronautBuy[3] = gameData.astronautsbuy4;
        confirmAstronautBuy[4] = gameData.astronautsbuy5;
        confirmAstronautBuy[5] = gameData.astronautsbuy6;
        confirmAstronautBuy[6] = gameData.astronautsbuy7;
        confirmAstronautBuy[7] = gameData.astronautsbuy8;
        confirmAstronautBuy[8] = gameData.astronautsbuy9;
        confirmAstronautBuy[9] = gameData.astronautsbuy10;
        confirmAstronautBuy[10] = gameData.astronautsbuy11;
        confirmAstronautBuy[11] = gameData.astronautsbuy12;
        confirmAstronautBuy[12] = gameData.astronautsbuy13;
        confirmAstronautBuy[13] = gameData.astronautsbuy14;
        confirmAstronautBuy[14] = gameData.astronautsbuy15;
        confirmAstronautBuy[15] = gameData.astronautsbuy16;
        confirmAstronautBuy[16] = gameData.astronautsbuy17;
        confirmAstronautBuy[17] = gameData.astronautsbuy18;
        confirmAstronautBuy[18] = gameData.astronautsbuy19;
        confirmAstronautBuy[19] = gameData.astronautsbuy20;
        confirmAstronautBuy[20] = gameData.astronautsbuy21;
        confirmAstronautBuy[21] = gameData.astronautsbuy22;
        confirmAstronautBuy[22] = gameData.astronautsbuy23;
        confirmAstronautBuy[23] = gameData.astronautsbuy24;
        confirmAstronautBuy[24] = gameData.astronautsbuy25;
        confirmAstronautBuy[25] = gameData.astronautsbuy26;
        confirmAstronautBuy[26] = gameData.astronautsbuy27;
        confirmAstronautBuy[27] = gameData.astronautsbuy28;
        confirmAstronautBuy[28] = gameData.astronautsbuy29;
        confirmAstronautBuy[29] = gameData.astronautsbuy30;
        confirmAstronautBuy[30] = gameData.astronautsbuy31;
        confirmAstronautBuy[31] = gameData.astronautsbuy32;
        confirmAstronautBuy[32] = gameData.astronautsbuy33;
        confirmAstronautBuy[33] = gameData.astronautsbuy34;
        confirmAstronautBuy[34] = gameData.astronautsbuy35;
        confirmAstronautBuy[35] = gameData.astronautsbuy36;
        confirmAstronautBuy[36] = gameData.astronautsbuy37;
        confirmAstronautBuy[37] = gameData.astronautsbuy38;
        confirmAstronautBuy[38] = gameData.astronautsbuy39;
        confirmAstronautBuy[39] = gameData.astronautsbuy40;
        rebirthCost = gameData.rebirthCostData;
        SuitsLevel[0] = gameData.suitsLevel1;
        SuitsLevel[1] = gameData.suitsLevel2;
        SuitsLevel[2] = gameData.suitsLevel3;
        SuitsLevel[3] = gameData.suitsLevel4;
        SuitsLevel[4] = gameData.suitsLevel5;
        SuitsLevel[5] = gameData.suitsLevel6;
        astronautsLevel[0] = gameData.astronautsLevel;
        astronautsLevel[1] = gameData.astronautsLevel2;
        astronautsLevel[2] = gameData.astronautsLevel3;
        astronautsLevel[3] = gameData.astronautsLevel4;
        astronautsLevel[4] = gameData.astronautsLevel5;
        astronautsLevel[5] = gameData.astronautsLevel6;
        astronautsLevel[6] = gameData.astronautsLevel7;
        astronautsLevel[7] = gameData.astronautsLevel8;
        astronautsLevel[8] = gameData.astronautsLevel9;
        astronautsLevel[9] = gameData.astronautsLevel10;
        astronautBuyStartID[0] = gameData.astronautIDStart1;
        astronautBuyStartID[1] = gameData.astronautIDStart2;
        astronautBuyStartID[2] = gameData.astronautIDStart3;
        astronautBuyStartID[3] = gameData.astronautIDStart4;
        astronautBuyStartID[4] = gameData.astronautIDStart5;
        astronautBuyStartID[5] = gameData.astronautIDStart6;
        astronautBuyStartID[6] = gameData.astronautIDStart7;
        astronautBuyStartID[7] = gameData.astronautIDStart8;
        astronautBuyStartID[8] = gameData.astronautIDStart9;
        astronautBuyStartID[9] = gameData.astronautIDStart10;
        planetUnlocked[0] = gameData.planetUnlocked1;
        researchCanBeDone[0] = gameData.researchCanBeDone1;
        researchCanBeDone[1] = gameData.researchCanBeDone2;
        researchCanBeDone[2] = gameData.researchCanBeDone3;
        researchCanBeDone[3] = gameData.researchCanBeDone4;
        researchUnlocked[0] = gameData.researchUnlocked1;
        researchUnlocked[1] = gameData.researchUnlocked2;
        researchUnlocked[2] = gameData.researchUnlocked3;
        researchUnlocked[3] = gameData.researchUnlocked4;
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
        if (mainCurrency >= rebirthCost)
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
}
