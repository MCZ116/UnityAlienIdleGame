using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class UnlockingSystem : MonoBehaviour
{
    public IdleScript idleScript;
    public double[] unlockCost = { 2000,4000,8000,20000 };
    public GameObject[] upgradeObjects;
    public GameObject[] unlockButtons;
    private GameObject[] unlockTextObject;
    public Text[] unlockText;
    public bool animationUnlockConfirm = false;
    private Animator unlockAnimation;
    //public Button[] unlockStage;

    private void Update()
    {
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

    public void UnlockingStages(int id)
    {

            if ((idleScript.mainCurrency >= unlockCost[id]) && (idleScript.upgradesActivated[id] == false))
            {
                idleScript.mainCurrency -= unlockCost[id];
                idleScript.upgradesActivated[id] = true;
                upgradeObjects[id].SetActive(true);
                unlockButtons[id].SetActive(false);
                animationUnlockConfirm = true;
            } else
                upgradeObjects[id].SetActive(false);
        
    }

    public void AnimationUnlock()
    {
        unlockAnimation.SetBool("unlockAnimation", true);
    }

}
