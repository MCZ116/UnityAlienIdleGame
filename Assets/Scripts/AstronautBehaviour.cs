using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AstronautBehaviour : MonoBehaviour
{
    [SerializeField]
    private GameManager gameManager;

    public GameObject[] astronautsUpgrades;
    public GameObject[] astronautsObjectsContainer;

    private Animator animationIdle;

    private double[] astronautCost;

    public Text[] AstronautCostText;

    public Button[] astronautsBuyButton;

    public bool[] astronautMaxConfirm;
   

    //[SerializeField]
    //public int[] astronautBuyStartID;
    //public int idButton;
    //public int idAstro;

    void Start()
    {
        astronautCost = new double[gameManager.StageLevel.Length];
        for (int id = 0; id < astronautCost.Length; id++)
        {
            astronautCost[id] = 50;
        }

        astronautMaxConfirm = new bool[AstronautCostText.Length];

        for (int id = 0; id < astronautMaxConfirm.Length; id++)
        {
            astronautMaxConfirm[id] = false;
        }
       
    }

    void Update()
    {
        AstronautButtonTextCheck();
        Debug.Log(astronautsUpgrades.Length + " " + astronautsObjectsContainer.Length + " Length of up as and containers");
    }

    public void AstronautButtonTextCheck()
    {
        for (int id = 0; id < AstronautCostText.Length; id++)
        {
            if (astronautMaxConfirm[id] == false)
            {
                AstronautCostText[id].text = AstronautPriceDisplay(id).ToString("F0");

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

    //NEED DEBUG
    public void AssigningAstronautsOnStart()
    {
        int TempID = 0;
        for (int id = 0; id < astronautsObjectsContainer.Length; id++)
        {
            int i = 0;
            
            Debug.Log("MainLoopAstroAs: " + id);
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
            Debug.Log("TempID: " + TempID);
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
        Debug.Log(" TempCostAstro: " + h * (gameManager.astronautsLevel[id] + 1));
        
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

            Debug.Log("AstroPrice: " + astronautTempCost );
        }
        //Debug.Log("Button Clicked. Received int: " + gameManager.astronautsLevel[id] + " ID: " + id);
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
        for (int id = 0; id < 40; id++)
        {

            //if (gameManager.confirmAstronautBuy[id] == false)
            //{
            //    GameManager.instance.allObjects[id].SetActive(false);

            //}
            //else if (gameManager.confirmAstronautBuy[id] == true)
            //{
            //    GameManager.instance.allObjects[id].SetActive(true);
            //}

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
    // Function used to increase level of astronauts and calculations
    //public double AstronautsLvlAssigning()
    //{
    //    double AstroLvl = 1;
    //    for (int id = 0; id < upgradeAstronauts.Length; id++)
    //    {

    //        AstroLvl += gameManager.astronautsID[id];

    //    }

    //    return AstroLvl;
    //}

    // A function which calculate an income boost of research points based on level of astronauts
    public double AstronautsBoost()
    {
        double asBoost = 0;

       
            for (int id = 0; id < gameManager.astronautsLevel.Length; id++)
            {

                asBoost += gameManager.astronautsLevel[id] * 0.15;

            }

            return asBoost;
        
    }

}
