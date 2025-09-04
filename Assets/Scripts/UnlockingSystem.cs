using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class UnlockingSystem : MonoBehaviour
{
    public GameManager gameManager;
    public Research research;
    public ResearchManager researchManager;
    public List<ResearchData> unlockingPlanetResearches;
    public GameObject[] upgradeObjects;
    public GameObject[] planetsPanelsObjects;
    public Button[] planetsUnlockBtnObj;
    public Button nextButton;
    public GameObject[] gateUnlockButtonObject;
    public Text[] unlockText;
    public Text[] planetPriceText;
    public Text[] planetRequirementResearch;

    [System.NonSerialized]
    public double[] unlockCost;
    public double[] planetCost;
    private string[] researchNames;
    public int researchID = 0;
    private int[] researchTextGreenAtID = { 6, 12, 17, 21 };
    public bool[] planetCanBeUnlocked;
    public int planetsForUnlockAmount;

    private void Awake()
    {
        planetsForUnlockAmount = GameObject.FindGameObjectsWithTag("unlockPlanets").Length;
        planetCanBeUnlocked = new bool[planetsForUnlockAmount];
        planetCost = new double[planetsForUnlockAmount];
        UpgradeCostCalculator(planetCost, 200000, 2200.7);

        unlockText = new Text[upgradeObjects.Length];
        unlockCost = new double[upgradeObjects.Length];
        UpgradeCostCalculator(unlockCost, 550, 4.5); // Calculating unlock price for stages  

        researchNames = new string[research.researchUnlocked.Length];
        researchNames[0] = "Oxygen Recycle";
        researchNames[1] = "Ion Engines";
        researchNames[2] = "Water Filter";
        researchNames[3] = "Plant Space Growing";
        researchNames[4] = "Low Gravity Lander";
        researchNames[5] = "Ion Engines II";
        researchNames[6] = "Space 3D Printer";
        researchNames[7] = "Space Drones";
        researchNames[8] = "Low Gravity Lander";
        researchNames[9] = "Ion Engines III";
        researchNames[10] = "Space 3D Printer";
        researchNames[11] = "Space Drones";
        researchNames[12] = "Low Gravity Lander";
        researchNames[13] = "Ion Engines IV";
        researchNames[14] = "Space 3D Printer";
        researchNames[15] = "Space Drones";

        planetCanBeUnlocked = new bool[4];

        AssignUnlockObjects();
        for (int id = 0; id < unlockText.Length; id++)
        {
            unlockText[id] = gateUnlockButtonObject[id].transform.Find("StagePrice").GetComponent<Text>();
        }
      
    }

    private void Start()
    {

        // TODO Change when refactoring planets
        unlockingPlanetResearches = new List<ResearchData>();
        foreach (var research in researchManager.allResearches)
        {
            if (research.planetId > -1)
            {
                unlockingPlanetResearches.Add(research);
            }
        }

        for (int id = 0; id < planetsForUnlockAmount; id++)
        {
            planetCanBeUnlocked[id] = false;
            planetRequirementResearch[id].text = unlockingPlanetResearches[id].researchName;
        }
        planetCanBeUnlocked[0] = true;

        PlanetsUnlockCheck();
        LoadUnlocksStatus();
    }

    void Update()
    {
        StageUnlockTextControl();
        PlanetStatusCheck();
    }

    public void AssignUnlockObjects()
    {
        for (int id = 0; id < planetsForUnlockAmount; id++)
        {
            planetsPanelsObjects[id] = gameManager.planets[id + 1].transform.GetChild(0).gameObject;
        }

        int unlockObjectIdPosition = gameManager.stages.Length - upgradeObjects.Length;
        for (int id = 0; id < upgradeObjects.Length; id++)
        {
            gateUnlockButtonObject[id] = gameManager.stages[id+unlockObjectIdPosition].transform.Find("LockedStage").gameObject;
        }
    }

    public void StageUnlockTextControl()
    {
        for (int id = 0; id < upgradeObjects.Length; id++)
        {
            unlockText[id].text = GameManager.ExponentLetterSystem(unlockCost[id], "F2");

            if (gameManager.mainCurrency >= unlockCost[id])
            {
                unlockText[id].color = Color.green;
            }
            else
                unlockText[id].color = Color.red;
        }
    }

    public void PlanetStatusCheck()
    {
        for (int id = 0; id < planetsPanelsObjects.Length; id++)
        {
            if (!gameManager.planetUnlocked[id])
            {
                planetPriceText[id].gameObject.SetActive(true);
                planetRequirementResearch[id].gameObject.SetActive(true);
                planetPriceText[id].text = GameManager.ExponentLetterSystem(planetCost[id], "F2");
            }
            else
            {
                planetPriceText[id].gameObject.SetActive(false);
                planetRequirementResearch[id].gameObject.SetActive(false);
            }
                

            if (gameManager.mainCurrency >= planetCost[id])
            {
                planetPriceText[id].color = Color.green;
            } else
                planetPriceText[id].color = Color.red;
        }

        foreach (var research in unlockingPlanetResearches)
        {
            if (researchManager.unlockedResearches.Contains(research) && unlockingPlanetResearches.Contains(research))
            {
                planetRequirementResearch[research.planetId].color = Color.green;
            }
            else {
                planetRequirementResearch[research.planetId].color = Color.red;
            }
        }
    }

    public void UnlockingStages(int id)
    {

        if ((gameManager.mainCurrency >= unlockCost[id]) && (gameManager.upgradesActivated[id] == false))
        {
            gameManager.mainCurrency -= unlockCost[id];
            upgradeObjects[id].SetActive(true);
            gameManager.upgradesActivated[id] = true;
            gateUnlockButtonObject[id].GetComponent<Button>().interactable = false;
            gameManager.StageLevel[id + 1] += 1;

            if (!research.researchCanBeDone[0])
            {
                research.researchCanBeDone[0] = true;
            }

        }
        else
            upgradeObjects[id].SetActive(false);

    }

    public void UpgradeCostCalculator(double[] costArray, double basePrice, double multiplier)
    {
        for (int id = 0; id < costArray.Length; id++)
        {
            costArray[id] = basePrice;
            basePrice *= multiplier;
        }
    }

    public void LoadUnlocksStatus()
    {
        for (int id = 0; id < upgradeObjects.Length; id++)
        {

            if (gameManager.upgradesActivated[id] == true)
            {
                upgradeObjects[id].SetActive(true);
                gateUnlockButtonObject[id].SetActive(false);
            }
            else if (gameManager.upgradesActivated[id] == false)
            {

                upgradeObjects[id].SetActive(false);
                gateUnlockButtonObject[id].SetActive(true);
                gateUnlockButtonObject[id].GetComponent<Button>().interactable = true;
            }
        }
    }

    public void ResearchUnlocking(int id)
    {
        if(research.researchCanBeDone.Length - 1 > id)
        {
            research.researchCanBeDone[id+1] = true;
        }
    }

    public void PlanetsUnlockCheck()
    {
        for (int id = 0; id < planetsPanelsObjects.Length; id++)
        {

            if (gameManager.planetUnlocked[id] == true)
            {
                planetsPanelsObjects[id].SetActive(true);
            }
            else if (!gameManager.planetUnlocked[id])
            {

                planetsPanelsObjects[id].SetActive(false);
            }
        }
    }
    // TODO Change when refactoring planets
    public void PlanetsUnlocking(int id)
    {
        if (researchManager.unlockedResearches.Contains(unlockingPlanetResearches[id]) && gameManager.mainCurrency >= planetCost[id] && gameManager.planetUnlocked[id] == false)
        {
            gameManager.mainCurrency -= planetCost[id];
            planetsPanelsObjects[id].SetActive(true);
            planetsUnlockBtnObj[id].interactable = true;
            gameManager.planetUnlocked[id] = true;
        }
    }

}
