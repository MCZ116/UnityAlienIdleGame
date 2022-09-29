using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AstronautBehaviour : MonoBehaviour
{
    [SerializeField]
    private GameManager gameManager;
    [SerializeField]
    private UnlockingSystem unlockingSystem;

    public GameObject[] astronautsUpgrades;
    public GameObject[] astronautsObjectsContainer;

    private Animator animationIdle;

    private double[] astronautCost;

    public Text[] AstronautCostText;

    public Button[] astronautsBuyButton;

    public bool[] astronautMaxConfirm;

    int astronautsTotalAmount;

    void Start()
    {
        astronautCost = new double[gameManager.StageLevel.Length];

        for (int id = 0; id < astronautCost.Length; id++)
        {
            astronautCost[id] = 50;
        }

        astronautMaxConfirm = new bool[gameManager.StageLevel.Length];

        for (int id = 0; id < astronautMaxConfirm.Length; id++)
        {
            astronautMaxConfirm[id] = false;
        }

        AssignAstronautSection();
        // It's here because first load of game not always check that so all buttons are interactable
        AstronautButtonTextCheck();
    }

    void Update()
    {
        AstronautButtonTextCheck();
        Debug.Log(astronautsUpgrades.Length + " " + astronautsObjectsContainer.Length + " Length of up as and containers");
    }

    public void AstronautButtonTextCheck()
    {
        for (int id = 0; id < gameManager.StageLevel.Length; id++)
        {
            if (astronautMaxConfirm[id] == false)
            {
                AstronautCostText[id].text = AstronautPriceDisplay(id).ToString("F0");

                AstronautCostText[id].color = Color.white;

                astronautsBuyButton[id].GetComponent<Image>().color = Color.white;
                
            }
            else
            {
                AstronautCostText[id].text = "MAX";

                astronautsBuyButton[id].GetComponent<Image>().color = Color.yellow;

                AstronautCostText[id].color = Color.black;
            }
            AstronautsBuyButtonControl(id);
        }
    }

    public void AssignAstronautSection()
    {
        for (int id = 0; id < gameManager.StageLevel.Length - 1; id++)
        {
            astronautsBuyButton[id + 1] = unlockingSystem.upgradeObjects[id].transform.Find("BuyAstronautsButton").GetComponent<Button>();
            AstronautCostText[id + 1] = unlockingSystem.upgradeObjects[id].transform.Find("BuyAstronautsButton").GetComponentInChildren<Text>();
        }

    }

    //NEED DEBUG
    public void AssigningAstronautsOnStart()
    {
        int TempID = 0;
        for (int id = 0; id < astronautsObjectsContainer.Length; id++)
        {
            int i = 0;
            
            for (int n = TempID; n < astronautsUpgrades.Length; n++)
            {
                if (astronautsObjectsContainer[id].transform.childCount > i)
                {
                    astronautsUpgrades[n] = astronautsObjectsContainer[id].transform.GetChild(i).gameObject;
                    i++;
                }
                else
                {
                    break;
                }
                    
            }

            TempID = TempID + astronautsObjectsContainer[id].transform.childCount;
            astronautsTotalAmount = TempID;

        }

        for (int id = 0; id < astronautsUpgrades.Length; id++)
        {

            astronautsUpgrades[id].SetActive(false);

            if (gameManager.confirmAstronautBuy[id] == true)
            {
                astronautsUpgrades[id].SetActive(true);
            }
        }
    }
  
    public void AstronautsAppearing(int id)
    {
        var h = astronautCost[id];
        var astronautTempCost = h * (gameManager.astronautsLevel[id]+1);
        
        if (astronautTempCost == 200) { astronautTempCost = 300; }

        if ( astronautTempCost <= 300 && gameManager.astronautsLevel[id] <= 3 && gameManager.crystalCurrency >= astronautTempCost)
        {  
            gameManager.crystalCurrency -= astronautTempCost;
            //GameManager.instance.allObjects[gameManager.astronautBuyStartID[id]].SetActive(true);
            astronautsUpgrades[gameManager.astronautBuyStartID[id]].SetActive(true);
            gameManager.confirmAstronautBuy[gameManager.astronautBuyStartID[id]] = true;
            gameManager.astronautBuyStartID[id]++;
            gameManager.astronautsLevel[id]++;
            AstronautsBoost();

        }
    }

    // Calculating price for displaying
    public double AstronautPriceDisplay(int id)
    {
        var h = astronautCost[id];

        var astronautTempCost = h * (gameManager.astronautsLevel[id]+1);

        if (astronautTempCost == 200) { astronautTempCost = 300; }
        if (gameManager.astronautsLevel[id] == 4)
        {
            astronautMaxConfirm[id] = true;
        }

        return astronautTempCost;
    }
    // Setting button interactable off and on depending from amount of crystals needed
    public void AstronautsBuyButtonControl(int id)
    {
        var h = astronautCost[id];
        var astronautTempCost = h * (gameManager.astronautsLevel[id] + 1);
        if (astronautTempCost == 200) { astronautTempCost = 300; }
        if (gameManager.crystalCurrency >= astronautTempCost && astronautMaxConfirm[id] == false)
        {
            astronautsBuyButton[id].interactable = true;
        }
        else
        {
            astronautsBuyButton[id].interactable = false;
        }
    }

    // Controlling display of activated astronauts
    public void AstronautsControl()
    {
        // NEED TO CHANGE IT A BIT 
        for (int id = 0; id < astronautsTotalAmount; id++)
        {

            if (gameManager.confirmAstronautBuy[id] == false)
            {
                astronautsUpgrades[id].SetActive(false);

            }
            else if (gameManager.confirmAstronautBuy[id] == true)
            {
                astronautsUpgrades[id].SetActive(true);
            }
        }
    }
 
    public double AstronautsBoost()
    {
        double asBoost = 0;

        for (int id = 0; id < gameManager.astronautsLevel.Length; id++)
        {

            asBoost += gameManager.astronautsLevel[id] * ((gameManager.StageLevel[id] * gameManager.upgradesCounts[id]) * 0.1);

        }

        return asBoost;
        
    }

}
