using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class UnlockingSystem : MonoBehaviour
{
    public IdleScript idleScript;
    double[] unlockCost = { 2000};
    public GameObject[] upgradeObjects;
    public GameObject[] unlockButtons;
    public Text[] unlockText;
    public Button[] unlockStage;

    private void Update()
    {
        Debug.Log(upgradeObjects.Length);
        Debug.Log(idleScript.upgradesActivated.Length);
        for (int id = 0; id < upgradeObjects.Length; id++)
        {
            unlockText[id].text = "Buy for : " + IdleScript.ExponentLetterSystem(unlockCost[id], "F2");
            if (idleScript.upgradesActivated[id] == true)
            {
                upgradeObjects[id].SetActive(true);
                unlockButtons[id].SetActive(false);
            }
            else
            {
                upgradeObjects[id].SetActive(false);
                unlockButtons[id].SetActive(true);
            }
        }
    }

    public void UnlockingStages()
    {
        for (int id = 0; id < upgradeObjects.Length; id++)
        {
            if (idleScript.mainCurrency >= unlockCost[id] & idleScript.upgradesActivated[id] == false)
            {
                idleScript.mainCurrency -= unlockCost[id];
                idleScript.upgradesActivated[id] = true;
                upgradeObjects[id].SetActive(true);
                unlockButtons[id].SetActive(false);
            } else
                upgradeObjects[id].SetActive(false);
                
        }
        
    }
}
