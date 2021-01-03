using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class UnlockingSystem : MonoBehaviour
{
    public IdleScript idleScript;
    double[] unlockCost = { 2000 };
    public GameObject[] upgradeObjects;
    public GameObject[] unlockButtons;
    public Text unlockText;


    private void Update()
    {
        
        unlockText.text = "Buy for : 2a";
        for (int id = 0; id < unlockCost.Length; id++)
        {
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
        for (int id = 0; id < unlockCost.Length; id++)
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
