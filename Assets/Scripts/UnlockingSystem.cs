using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class UnlockingSystem : MonoBehaviour
{
    public GameManager gameManager;
    public ResearchManager researchManager;
    public GameObject[] upgradeObjects;
    public Button nextButton;
    public GameObject[] gateUnlockButtonObject;
    public Text[] unlockText;

    [System.NonSerialized]
    public double[] unlockCost;

    //private void Awake()
    //{
    //    unlockText = new Text[upgradeObjects.Length];
    //    unlockCost = new double[upgradeObjects.Length];
    //    UpgradeCostCalculator(unlockCost, 550, 4.5); // Calculating unlock price for stages  

    //    //AssignUnlockObjects();
    //    //for (int id = 0; id < unlockText.Length; id++)
    //    //{
    //    //    unlockText[id] = gateUnlockButtonObject[id].transform.Find("StagePrice").GetComponent<Text>();
    //    //}
      
    //}

    //private void Start()
    //{
    //    LoadUnlocksStatus();
    //}

    //void Update()
    //{
    //    StageUnlockTextControl();
    //}

    //public void AssignUnlockObjects()
    //{
    //    int unlockObjectIdPosition = gameManager.stages.Length - upgradeObjects.Length;
    //    for (int id = 0; id < upgradeObjects.Length; id++)
    //    {
    //        gateUnlockButtonObject[id] = gameManager.stages[id+unlockObjectIdPosition].transform.Find("LockedStage").gameObject;
    //    }
    //}

    //public void StageUnlockTextControl()
    //{
    //    for (int id = 0; id < upgradeObjects.Length; id++)
    //    {
    //        unlockText[id].text = GameManager.ExponentLetterSystem(unlockCost[id], "F2");

    //        if (gameManager.mainCurrency >= unlockCost[id])
    //        {
    //            unlockText[id].color = Color.green;
    //        }
    //        else
    //            unlockText[id].color = Color.red;
    //    }
    //}

    //public void UnlockingStages(int id)
    //{

    //    if ((gameManager.mainCurrency >= unlockCost[id]) && (gameManager.upgradesActivated[id] == false))
    //    {
    //        gameManager.mainCurrency -= unlockCost[id];
    //        upgradeObjects[id].SetActive(true);
    //        gameManager.upgradesActivated[id] = true;
    //        gateUnlockButtonObject[id].GetComponent<Button>().interactable = false;
    //        gameManager.StageLevel[id + 1] += 1;

    //    }
    //    else
    //        upgradeObjects[id].SetActive(false);

    //}

    //public void UpgradeCostCalculator(double[] costArray, double basePrice, double multiplier)
    //{
    //    for (int id = 0; id < costArray.Length; id++)
    //    {
    //        costArray[id] = basePrice;
    //        basePrice *= multiplier;
    //    }
    //}

    //public void LoadUnlocksStatus()
    //{
    //    for (int id = 0; id < upgradeObjects.Length; id++)
    //    {

    //        if (gameManager.upgradesActivated[id] == true)
    //        {
    //            upgradeObjects[id].SetActive(true);
    //            gateUnlockButtonObject[id].SetActive(false);
    //        }
    //        else if (gameManager.upgradesActivated[id] == false)
    //        {

    //            upgradeObjects[id].SetActive(false);
    //            gateUnlockButtonObject[id].SetActive(true);
    //            gateUnlockButtonObject[id].GetComponent<Button>().interactable = true;
    //        }
    //    }
    //}

}
