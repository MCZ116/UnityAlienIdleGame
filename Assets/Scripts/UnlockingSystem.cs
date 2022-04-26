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
                planetRequirementResearch[id].text = "Ion Engines";
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
        if((gameManager.upgradesActivated[id] == false))
        {
            gameManager.StageLevel[id+1] += 1;
        }

        if ((gameManager.mainCurrency >= unlockCost[id]) && (gameManager.upgradesActivated[id] == false))
        {
            gameManager.mainCurrency -= unlockCost[id];
            upgradeObjects[id].SetActive(true);
            gameManager.upgradesActivated[id] = true;
            unlockButtons[id].GetComponent<Button>().interactable = false;
            animationUnlockConfirm[id] = true;
            //Debug.Log("Upgrade Unlocked!");
            ResearchUnlocking(id);
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
            }
        }
    }

    // Let's think about that system...
    public void ResearchUnlocking(int id)
    {
        if (id == 0 || id == 3 || id == 5 || id == 7 && !gameManager.researchCanBeDone[gameManager.researchID])
        {
            gameManager.researchCanBeDone[gameManager.researchID] = true;
            gameManager.researchID++;
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
        if (gameManager.researchUnlocked[1] == true && gameManager.mainCurrency >= planetCost[id] && gameManager.planetUnlocked[id] == false)
        {
            gameManager.mainCurrency -= planetCost[id]; 
            planetsPanelsObjects[id].SetActive(true);
            planetsUnlockBtnObj[id].interactable = true;
            gameManager.planetUnlocked[id] = true;
        } 

    }

}
