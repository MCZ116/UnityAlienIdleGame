using System;
using System.Collections;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public OfflineProgress offline;
    public AudioControler AC;
    public BonusManager bonusManager;
    public int buyModeID;
    public TextMeshProUGUI CurrencyText;
    public TextMeshProUGUI TotalIncomePerSecond;
    public Text ChangeBuyModeText;
    public TextMeshProUGUI ReturnPriceText;
    public TextMeshProUGUI ReturnsCount;
    public TextMeshProUGUI AntiMatterEarned;
    public TextMeshProUGUI AntiMatterToReward;
    public TextMeshProUGUI CrystalsAmount;
    public TextMeshProUGUI AntiMatter;
    public Text ProfileLevel;
    public TextMeshProUGUI returnRequirements;
    public Text nickName;
    public double mainCurrency;
    public double crystalCurrency;
    public double antiMatter;
    public double returnCost;
    public GameObject settingsScreenObject;
    public GameObject[] planets;
    public GameObject homeSafeZone;
    public GameObject returnConfirmMessage;
    public Button globalUpgradesBtn;
    [System.NonSerialized]
    private bool[] activeTab;

    [SerializeField] ResearchManager researchManager;

    [SerializeField] PlanetManager planetManager;

    [SerializeField] BuildingManager buildingManager;

    [SerializeField] GlobalUpgradeManager globalUpgradeManager;

    public CanvasGroup[] canvasPlanetsTabs;

    public CanvasGroup[] canvasTabs;

    public string[] tabsNames;

    public CanvasGroup canvasMainGame;

    private int planetID;

    public int returnCount = 0; // how many times the player has reset
    public double totalCurrencyEarned = 0; // lifetime tracker

    public static GameManager instance = null;

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
        returnCost = 50000;
        planets = FindObsWithTag("planetTab");
        
        planetID = 0;
        buyModeID = 0;

        canvasPlanetsTabs = new CanvasGroup[planets.Length];

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
        
        offline.offlineRewards.SetActive(true);
    }

    void Start()
    {
        if (!SaveSystem.SaveExists())
        {
            // First run: reset all buildings and astronauts
            foreach (var building in buildingManager.buildings)
            {
                building.level = 0;
                building.astronautsHired = 0;
                building.RestoreAstronauts();
                building.UpdateVisuals();
            }

            Save();
            PlayerPrefs.SetInt("NeverDone", 1);
        }
        else
        {
            Load();
        }

        offline.offlineRewards.SetActive(false);
        ChangeBuyModeText.text = "1";
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

        ProcessBuildingIncomeCycle();

        CurrencyText.text = ExponentLetterSystem(mainCurrency);
        TotalIncomePerSecond.text = ExponentLetterSystem(GetTotalIncomePerSecond()) + "/s ";
        ReturnPriceText.text = ExponentLetterSystem(returnCost);
        ReturnsCount.text = "Returns: " + returnCount;
        ProfileLevel.text = "1"; //Will be implemented later with XP system
        CrystalsAmount.text = crystalCurrency.ToString("F0");
        AntiMatter.text = ExponentLetterSystem(antiMatter);
        AntiMatterEarned.text = ExponentLetterSystem(globalUpgradeManager.CalculateAntimatterReward(totalCurrencyEarned));
        AntiMatterToReward.text = AntiMatterEarned.text;
        nickName.text = "Nick: " + PlayerPrefs.GetString("Nick");

        GlobalUpgradesController();
        ReturnButtonStatus();
        ReturnUnlock();
        StartCoroutine("MySave");
        SaveDate();
    }

    public double GetTotalIncomePerSecond()
    {
        double total = 0;
        foreach (var building in buildingManager.buildings)
        {
            if (building.level > 0)
            {
                double profitPerSecond = building.GetCurrentProfit() / building.data.incomeInterval;
                profitPerSecond *= GetGlobalMultiplier();
                total += profitPerSecond;
            }
        }
        return total;
    }

    public void ProcessBuildingIncomeCycle()
    {
        foreach (var building in buildingManager.buildings)
        {
            if (building.level <= 0)
            {
                building.currentProgress = 0f;
                continue;
            }

            building.timer += Time.deltaTime;
            building.currentProgress = building.timer / building.data.incomeInterval;

            if (building.timer >= building.data.incomeInterval)
            {
                double profit = building.GetCurrentProfit();
                profit *= researchManager.GetGlobalIncomeMultiplier();
                profit *= bonusManager.GetIncomeMultiplier();
                AddCurrency(profit);
                building.timer = 0f;
                building.currentProgress = 0f;
            }
        }
    }

    public double GetGlobalMultiplier()
    {
        return
            researchManager.GetGlobalIncomeMultiplier() *
            bonusManager.GetIncomeMultiplier() *
            globalUpgradeManager.GetGlobalIncomeBoost();
    }

    public void AddCurrency(double income)
    {
        mainCurrency += income;
        totalCurrencyEarned += income; // track every earned coin
    }

    public double GetIncomePerSecondOfBuilding(BuildingState state, int levelsToBuy = 0)
    {
        var building = state;
        if (building.level <= 0 && levelsToBuy == 0)
            return 0;
        {
        if (levelsToBuy == int.MaxValue)
                levelsToBuy = CalculateMaxAffordableLevels(state);

            int targetLevel = state.level + levelsToBuy;

            double newIncome = building.GetProfitAtLevel(targetLevel) / building.data.incomeInterval;
            newIncome *= researchManager.GetGlobalIncomeMultiplier();
            newIncome *= bonusManager.GetIncomeMultiplier();
            newIncome *= globalUpgradeManager.GetGlobalIncomeBoost();

            return newIncome;
        }
    }

    public double GetTotalIncomeWithPreview(GlobalUpgradeData previewUpgrade)
    {
        double total = 0;

        foreach (var building in buildingManager.buildings)
        {
            if (building.level <= 0) continue;

            double profitPerSecond = building.GetCurrentProfit() / building.data.incomeInterval;
            profitPerSecond *= researchManager.GetGlobalIncomeMultiplier();
            profitPerSecond *= bonusManager.GetIncomeMultiplier();

            // Add the preview upgrade temporarily
            profitPerSecond *= globalUpgradeManager.GetGlobalIncomeBoostWithPreview(previewUpgrade);

            total += profitPerSecond;
        }

        return total;
    }

    public void GlobalUpgradesController()
    {
        if (returnCount >= 1)
        {
            globalUpgradesBtn.interactable = true;
        }
        else
        {
            globalUpgradesBtn.interactable = false;
        }
    }

    public GameObject[] FindObsWithTag(string tag)
    {
        GameObject[] objects = GameObject.FindGameObjectsWithTag(tag);

        // Sort them by hierarchy order
        var sorted = objects.OrderBy(go => go.transform.GetSiblingIndex()).ToArray();
        return sorted;
    }

    public void EnterSettings()
    {
        settingsScreenObject.SetActive(true);
    }

    public void ExitScreenButton()
    {
        settingsScreenObject.SetActive(false);
    }

    public static string ExponentLetterSystem(double value, string format = "F2")
    {

        if (value < 1000) return value.ToString(format);

        // Determine exponent
        int exponent = (int)Math.Floor(Math.Log10(value));
        int exponentEng = 3 * (exponent / 3); // round down to nearest 3

        // Number part
        double scaledValue = value / Math.Pow(10, exponentEng);

        // Letter(s) part
        string letters = "";
        int index = exponentEng / 3 - 1; // -1 because 1e3 = k
        while (index >= 0)
        {
            letters = (char)('a' + (index % 26)) + letters;
            index = index / 26 - 1;
        }

        return scaledValue.ToString(format) + letters;

    }

    public static double AggressiveRound(double value)
    {
        if (value <= 0)
            return 0;

        // Determine the magnitude
        double magnitude = Math.Pow(10, Math.Floor(Math.Log10(value)));

        // Scale to 1–10 range
        double scaled = value / magnitude;

        // Round to nearest single digit (1 digit precision)
        // Examples:
        // 5.07 -> 6
        // 42.1 -> 4
        // 507 -> 6
        // 534829 -> 5
        double rounded = Math.Round(scaled);

        // Return full rounded number
        return rounded * magnitude;
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
        UpgradePanelController.Instance.HidePanel();
    }

    public void ChangePlanetTab(int planetID)
    {
        if (!planetManager.CanAccessPlanet(planetID))
            return;

        for (int id = 0; id < canvasPlanetsTabs.Length; id++)
        {
            if (id != planetID)
            {
                CanvasGroupMenuSwitch(false, canvasPlanetsTabs[id]);
                planets[id].SetActive(false);
            } else
            {
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
            }
        }
        else if (buttonName == "Prev")
        {
            int prev = planetManager.GetPrevUnlockedPlanetId(planetID);
            if (prev != -1)
            {
                planetID = prev;
                ChangePlanetTab(planetID);
            }
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

        SaveSystem.SaveGameData(this,researchManager, planetManager, buildingManager, globalUpgradeManager);

    }

    public void Load()
    {
        GameData gameData = SaveSystem.LoadData();

        mainCurrency = gameData.researchPointsData;
        crystalCurrency = gameData.crystals;
        antiMatter = gameData.antiMatter;
        returnCount = gameData.returnCount;
        returnCost = gameData.returnCostData;
        totalCurrencyEarned = gameData.totalCurrencyEarned;

        planetManager.ApplyLoadedData(gameData, planetManager.allPlanets);
        researchManager.ApplyLoadedData(gameData, researchManager.allResearches);
        buildingManager.ApplyLoadedData(gameData);
        globalUpgradeManager.ApplyLoadedData(gameData);
    }

    public void SaveDate()
    {
        PlayerPrefs.SetString("OfflineTime", System.DateTime.Now.ToBinary().ToString());
    }

    public void ChangeBuyButtonMode()
    {
        buyModeID = (buyModeID + 1) % 4;

        switch (buyModeID)
        {
            case 0: ChangeBuyModeText.text = "1"; break;
            case 1: ChangeBuyModeText.text = "10"; break;
            case 2: ChangeBuyModeText.text = "100"; break;
            case 3: ChangeBuyModeText.text = "MAX"; break;
        }
    }

    public int GetBuyAmount()
    {
        switch (buyModeID)
        {
            case 0: return 1;
            case 1: return 10;
            case 2: return 100;
            case 3: return int.MaxValue; // We'll handle MAX separately
            default: return 1;
        }
    }

    public double CalculateTotalCost(BuildingState state, int levelsToBuy)
    {
        double growth = 1.15; // Your upgrade multiplier
        double currentPrice = state.GetCurrentPrice();

        return currentPrice * (Math.Pow(growth, levelsToBuy) - 1) / (growth - 1);
    }

    public int CalculateMaxAffordableLevels(BuildingState state)
    {
        double growth = 1.15;
        double currentPrice = state.GetCurrentPrice();
        double currency = mainCurrency;

        if (currency < currentPrice) return 0;

        int maxLevels = (int)Math.Floor(
            Math.Log(currency * (growth - 1) / currentPrice + 1, growth)
        );

        return maxLevels;
    }

    public double GetCostPreview(BuildingState state)
    {
        int buyAmount = GetBuyAmount();

        if (buyAmount == int.MaxValue)
            buyAmount = CalculateMaxAffordableLevels(state);

        double totalCost = CalculateTotalCost(state, buyAmount);

        return totalCost;
    }

    public void OpenReturnConfirmation()
    {
        returnConfirmMessage.SetActive(true);
    }

    public void CloseReturnConfirmation()
    {
        returnConfirmMessage.SetActive(false);
    }

    public void ResetProgress()
    {
        CloseReturnConfirmation();
        returnCount++;
        double antimatterReward = globalUpgradeManager.CalculateAntimatterReward(totalCurrencyEarned);
        globalUpgradeManager.AddAntimatter(antimatterReward);

        // Reset everything else
        buildingManager.ResetAllBuildings();
        mainCurrency = 200;

        //Reset astronauts animation
        foreach (var buildingState in buildingManager.buildings)
        {
            buildingState.RestoreAstronauts();
        }

        researchManager.unlockedResearches.Clear();
        researchManager.UpdateLinesColor();
        planetManager.unlockedPlanets.Clear();
        buildingManager.ResetAllBuildings();

        // First planet always unlocked
        planetManager.unlockedPlanets.Add(planetManager.allPlanets[0]);
        totalCurrencyEarned = 0;
        ChangePlanetTab(0);
    }

    public void ReturnButtonStatus()
    {
        if (mainCurrency >= returnCost)
        {
            ReturnPriceText.color = Color.green;
        }
        else
            ReturnPriceText.color = Color.red;
    }

    //TODO
    public void ReturnUnlock()
    {
        if (researchManager.unlockedResearches.Count >= 6)
        {
            returnRequirements.color = Color.green;
        }
        else
            returnRequirements.color = Color.red;
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
