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
    public GameObject[] unlockButtons;
    private GameObject[] unlockTextObject;
    public Text[] unlockText;

    [System.NonSerialized]
    public bool[] animationUnlockConfirm = { false, false, false, false, false, false, false, false };
    [System.NonSerialized]
    public double[] unlockCost = { 2000, 4000, 8000, 20000, 100000, 135000, 160000, 180000 };

    //public Button[] unlockStage;

    void Start()
    {
       
    }

    private void Update()
    {
        for (int id = 0; id < upgradeObjects.Length; id++)
        {
            unlockText[id].text = GameManager.ExponentLetterSystem(unlockCost[id], "F2");
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

    public void ResearchUnlocking(int id)
    {
        if (id == 0 || id == 3 && !research.researchCanBeDone[researchID])
        {
            research.researchCanBeDone[researchID] = true;
            researchID++;
        }
    }

}
