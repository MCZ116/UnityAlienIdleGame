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
    public BonusManager bonusManager;
    public int buyModeID;
    public Text CurrencyText;
    public Text IncomePerSecond;
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
    public GameObject settingsScreenObject;
    public GameObject[] planets;
    public GameObject homeSafeZone;
    [System.NonSerialized]
    private bool[] activeTab;

    [SerializeField] ResearchManager researchManager;

    [SerializeField] PlanetManager planetManager;

    [SerializeField] BuildingManager buildingManager;

    public SuitsUpgrades suitsUpgrades;

    public CanvasGroup[] canvasPlanetsTabs;

    public CanvasGroup[] canvasTabs;

    public string[] tabsNames = {"gameMenu","shopMenu","researchMenu","rebirth","suitsMenu", "planetsMenu" };

    public CanvasGroup canvasMainGame;

    private double[] suitsLevel;
    public double[] SuitsLevel { get => suitsLevel; private set => suitsLevel = value; }
    private int planetID;

    public double incomeMultiplier = 1.0; // gets higher after resets
    public int resetLevel = 1; // how many times the player has reset
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
        rebirthCost = 50000;
        planets = FindObsWithTag("planetTab");
        
        SuitsLevel = new double[6];

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

        suitsLevel[0] = 0;
        suitsLevel[1] = 0;
        resetLevel = 1;
        
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
        
        //astronautBehaviour.AstronautsObjectActivationControl();
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

        CurrencyText.text = ExponentLetterSystem(mainCurrency, "F2");
        IncomePerSecond.text = ExponentLetterSystem(GetTotalIncomePerSecond(), "F2") + "/s ";
        RebirthPriceText.text = ExponentLetterSystem(rebirthCost, "F2");
        RebirthLevel.text = "Returns: " + ExponentLetterSystem(resetLevel, "F0");
        ProfileLevel.text = ExponentLetterSystem(resetLevel, "F0");
        CrystalsAmount.text = crystalCurrency.ToString("F0");
        nickName.text = "Nick: " + PlayerPrefs.GetString("Nick");

        RebirthButtonStatus();
        RebirthUnlock();
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
                profitPerSecond *= researchManager.GetGlobalIncomeMultiplier();
                profitPerSecond *= bonusManager.GetIncomeMultiplier();
                total += profitPerSecond;
            }
        }
        return total * incomeMultiplier;
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


    public void AddCurrency(double amount)
    {
        double income = amount * incomeMultiplier;
        mainCurrency += income;
        totalCurrencyEarned += income; // track every earned coin
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

        SaveSystem.SaveGameData(this,researchManager, planetManager, buildingManager);

    }

    public void Load()
    {
        GameData gameData = SaveSystem.LoadData();

        mainCurrency = gameData.researchPointsData;
        crystalCurrency = gameData.crystals;
        resetLevel = gameData.resetLevel;
        rebirthCost = gameData.rebirthCostData;
        incomeMultiplier = gameData.incomeMultiplier;
        totalCurrencyEarned = gameData.totalCurrencyEarned;

        for (int id = 0; id < SuitsLevel.Length; id++)
        {
            SuitsLevel[id] = gameData.suitsLevel[id];
        }

        planetManager.ApplyLoadedData(gameData, planetManager.allPlanets);
        researchManager.ApplyLoadedData(gameData, researchManager.allResearches);
        buildingManager.ApplyLoadedData(gameData);
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

    public void ResetProgress()
    {
        resetLevel++;

        // Prestige bonus based on total earned
        double bonus = Math.Floor(totalCurrencyEarned / 1e6);
        incomeMultiplier += bonus * 0.05; // +5% per million

        // Reset everything else
        buildingManager.ResetAllBuildings();
        mainCurrency = 100;

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

        ChangePlanetTab(0);
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
