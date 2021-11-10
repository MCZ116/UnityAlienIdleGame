using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class UnlockingSystem : MonoBehaviour
{
    public GameManager idleScript;
    public Research research;
    public int researchID = 0;
    public GameObject[] upgradeObjects;
    public GameObject[] planetsPanelsObjects;
    public Button[] planetsUnlockBtnObj;
    public Button nextButton;
    public GameObject[] unlockButtons;
    private GameObject[] unlockTextObject;
    public Text[] unlockText;
    public Text[] planetPriceText;
    public string changecanvas;

    [System.NonSerialized]
    public bool[] animationUnlockConfirm = { false, false, false, false, false, false, false, false, false };
    [System.NonSerialized]
    public double[] unlockCost = { 2000, 4000, 8000, 20000, 100000, 135000, 160000, 180000, 220000 };
    public double[] planetCost = { 220000 };

    void Start()
    {


    }

    private void Update()
    {
        for (int id = 0; id < upgradeObjects.Length; id++)
        {
            unlockText[id].text = GameManager.ExponentLetterSystem(unlockCost[id], "F2");
        }  
        for (int id = 0; id < planetsPanelsObjects.Length; id++)
        {
            if (!idleScript.planetUnlocked[id])
            {
                planetPriceText[id].enabled = true;
                planetPriceText[id].text = GameManager.ExponentLetterSystem(planetCost[id], "F2");
            }
            else
                planetPriceText[id].enabled = false;
        }
       
}

    public void UnlockingStages(int id)
    {

        if ((idleScript.mainCurrency >= unlockCost[id]) && (idleScript.upgradesActivated[id] == false))
        {
            animationUnlockConfirm[id] = true;
            idleScript.mainCurrency -= unlockCost[id];
            upgradeObjects[id].SetActive(true);
            idleScript.upgradesActivated[id] = true;
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

            if (idleScript.upgradesActivated[id] == true)
            {
                upgradeObjects[id].SetActive(true);
                unlockButtons[id].SetActive(false);
            }
            else if (idleScript.upgradesActivated[id] == false)
            {

                upgradeObjects[id].SetActive(false);
                unlockButtons[id].SetActive(true);
            }
        }
    }

    // Let's think about that system...
    public void ResearchUnlocking(int id)
    {
        if (id == 0 || id == 3 && !research.researchCanBeDone[researchID])
        {
            research.researchCanBeDone[researchID] = true;
            researchID++;
        }
    }

    public void PlanetsUnlockCheck()
    {
        for (int id = 0; id < planetsPanelsObjects.Length; id++)
        {

            if (idleScript.planetUnlocked[id] == true)
            {
                planetsPanelsObjects[id].SetActive(true);
                nextButton.interactable = true;
            }
            else if (!idleScript.planetUnlocked[id])
            {

                planetsPanelsObjects[id].SetActive(false);
                nextButton.interactable = false;
            }
        }
    }

    public void PlanetsUnlocking(int id)
    {
        if (research.researchCanBeDone[1]==true && idleScript.mainCurrency >= planetCost[id] && idleScript.planetUnlocked[id] == false)
        {
            idleScript.mainCurrency -= planetCost[id]; 
            planetsPanelsObjects[id].SetActive(true);
            planetsUnlockBtnObj[id].interactable = true;
            idleScript.planetUnlocked[id] = true;
        } 

    }

}
