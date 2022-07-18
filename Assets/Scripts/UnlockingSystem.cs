using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class UnlockingSystem : MonoBehaviour
{
    public GameManager gameManager;
    public Research research;
    public GameObject[] upgradeObjects;
    public GameObject[] planetsPanelsObjects;
    public Button[] planetsUnlockBtnObj;
    public Button nextButton;
    public GameObject[] unlockButtons;
    public Text[] unlockText;
    public Text[] planetPriceText;
    public Text[] planetRequirementResearch;

    [System.NonSerialized]
    public bool[] animationUnlockConfirm;
    [System.NonSerialized]
    public double[] unlockCost;
    public double[] planetCost;
    private string[] researchNames;

    private void Start()
    {
        researchNames = new string[8];
        researchNames[0] = "Oxygen Recycle";
        researchNames[1] = "Ion Engines";
        researchNames[2] = "Water Filter";
        researchNames[3] = "Plant Space Growing";
        researchNames[4] = "Low Gravity Lander";
        researchNames[5] = "Ion Engines II";
        researchNames[6] = "Space 3D Printer";
        researchNames[7] = "Space Drones";
        int researchID = 1;
        for (int id = 0; id < planetsPanelsObjects.Length; id++)
        {
            planetRequirementResearch[id].text = researchNames[researchID];
            researchID = researchID + 4;
        }

    }

    void Update()
    {
        StageUnlockTextControl();
        PlanetStatusCheck();
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
                planetPriceText[id].enabled = true;
                planetRequirementResearch[id].enabled = true;
                planetPriceText[id].text = GameManager.ExponentLetterSystem(planetCost[id], "F2");
            }
            else
            {
                planetPriceText[id].enabled = false;
                planetRequirementResearch[id].enabled = false;
            }
                

            if (gameManager.mainCurrency >= planetCost[id])
            {
                planetPriceText[id].color = Color.green;
            } else
                planetPriceText[id].color = Color.red;

            if (gameManager.researchUnlocked[1])
            {
                planetRequirementResearch[id].color = Color.green;
            } else
                planetRequirementResearch[id].color = Color.red;
        }
    }

    public void UnlockingStages(int id)
    {

        if ((gameManager.mainCurrency >= unlockCost[id]) && (gameManager.upgradesActivated[id] == false))
        {
            gameManager.mainCurrency -= unlockCost[id];
            upgradeObjects[id].SetActive(true);
            gameManager.upgradesActivated[id] = true;
            unlockButtons[id].GetComponent<Button>().interactable = false;
            animationUnlockConfirm[id] = true;
            //Debug.Log("Upgrade Unlocked!");
            ResearchUnlocking(id);
            gameManager.StageLevel[id + 1] += 1;
        }
        else
            upgradeObjects[id].SetActive(false);

    }

    public void LoadUnlocksStatus()
    {
        for (int id = 0; id < upgradeObjects.Length; id++)
        {

            if (gameManager.upgradesActivated[id] == true)
            {
                upgradeObjects[id].SetActive(true);
                unlockButtons[id].SetActive(false);
            }
            else if (gameManager.upgradesActivated[id] == false)
            {

                upgradeObjects[id].SetActive(false);
                unlockButtons[id].SetActive(true);
                unlockButtons[id].GetComponent<Button>().interactable = true;
            }
        }
    }

    public void ResearchUnlocking(int id)
    {
        for (int researchUnlockID = 0; researchUnlockID < gameManager.StageLevel.Length; researchUnlockID = researchUnlockID + 2)
        {
            if (id == researchUnlockID && !gameManager.researchCanBeDone[gameManager.researchID])
            {
                gameManager.researchCanBeDone[gameManager.researchID] = true;
                gameManager.researchID++;
            }
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

    public void PlanetsUnlocking(int id)
    {
        for (int researchID = 0; researchID < gameManager.Research1Level.Length; researchID = researchID + 4)
        {

            if (gameManager.researchUnlocked[researchID] == true && gameManager.mainCurrency >= planetCost[id] && gameManager.planetUnlocked[id] == false)
            {
                gameManager.mainCurrency -= planetCost[id];
                planetsPanelsObjects[id].SetActive(true);
                planetsUnlockBtnObj[id].interactable = true;
                gameManager.planetUnlocked[id] = true;
            }

        }

    }

}
