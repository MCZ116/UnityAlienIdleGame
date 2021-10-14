using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AstronautBehaviour : MonoBehaviour
{
    [SerializeField]
    private GameManager gameManager;

    public GameObject[] upgradeAstronauts;
    public GameObject[] astronautsUpgrades;

    private Animator animationIdle;

    private double[] astronautCost = { 50, 50, 50, 50, 50, 50, 50, 50, 50, 50 };

    public Text[] AstronautCostText;

    public Button[] astronautsBuyButton;

    private bool[] astronautMaxConfirm;
   

    //[SerializeField]
    //public int[] astronautBuyStartID;
    //public int idButton;
    //public int idAstro;

    void Start()
    {
        gameManager.astronautsLevel = new int[10];
        astronautMaxConfirm = new bool[AstronautCostText.Length];

        for (int id = 0; id < astronautMaxConfirm.Length; id++)
        {
            astronautMaxConfirm[id] = false;
        }
       
    }

    void Update()
    {
        for (int id = 0; id < AstronautCostText.Length; id++)
        {
            if (astronautMaxConfirm[id] == false)
            {
                AstronautCostText[id].text = AstronautPriceDisplay(id).ToString("F0");
            }
            else
            {
                AstronautCostText[id].text = "MAX";

                astronautsBuyButton[id].GetComponent<Image>().color = Color.yellow;

                AstronautCostText[id].color = Color.black;
            }
            AstronautsBuyButtonControl(id);
        }
        for (int id = 0; id < astronautsUpgrades.Length; id++)
        {
            //Debug.Log(gameManager.confirmAstronautBuy[id] + "  ConfirmAstronautBuy ID" + id);
           
        }
    }

    public void AssigningAstronautsOnStart()
    {
        astronautsUpgrades = GameObject.FindGameObjectsWithTag("astronauts");
        Debug.Log(astronautsUpgrades.Length + " astronautsObjects");
        upgradeAstronauts = new GameObject[astronautsUpgrades.Length];


        for (int id = 0; id < astronautsUpgrades.Length; id++)
        {
            upgradeAstronauts[id] = astronautsUpgrades[id];
            upgradeAstronauts[id].SetActive(false);

            if(gameManager.confirmAstronautBuy[id] == true)
            {
                upgradeAstronauts[id].SetActive(true);
            }
            //Debug.Log(gameManager.confirmAstronautBuy[id] + "  ConfirmAstronautBuy ID " + id);
        }
    }
    // Button listener tries

    //void OnEnable()
    //{

    //            astronautsBuyButton[idButton].onClick.AddListener(() => ButtonCallBack(idButton, idAstro));                             

    //}

    //private void ButtonCallBack(int buttonID, int astronautBuyStartID)
    //{
    //    Debug.Log("Button Clicked. Received intID: " + buttonID + " with int: " + astronautBuyStartID + " asIDstar: " + gameManager.astronautsID[astronautBuyStartID]);
    //    var h = astronautCost[buttonID];

    //    var astronautTempCost = h * AstronautsLvlAssigning();
    //    if (astronautTempCost > 150) { astronautTempCost = 300; }

    //    if (gameManager.astronautsID[astronautBuyStartID] <= 3)
    //    {
    //        if (gameManager.crystalCurrency >= astronautTempCost)
    //        {
    //            gameManager.crystalCurrency -= astronautTempCost;
    //            upgradeAstronauts[gameManager.astronautsID[astronautBuyStartID]].SetActive(true);
    //            gameManager.confirmAstronautBuy[gameManager.astronautsID[astronautBuyStartID]] = true;
    //            gameManager.astronautsID[astronautBuyStartID]++;
    //            AstronautsLvlAssigning();
    //            AstronautsBoost();
    //        }
    //    }
    //}

    // Button for buying astronauts
    public void AstronautsAppearing(int id)
    {
        var h = astronautCost[id];
        var astronautTempCost = h * (gameManager.astronautsLevel[id]+1);
        Debug.Log(" TempCostAstro: " + h * (gameManager.astronautsLevel[id] + 1));
        
        if (astronautTempCost == 200) { astronautTempCost = 300; }

        if ( astronautTempCost <= 300 && gameManager.astronautsLevel[id] <= 3 && gameManager.crystalCurrency >= astronautTempCost)
        {  
            gameManager.crystalCurrency -= astronautTempCost;
            upgradeAstronauts[gameManager.astronautBuyStartID[id]].SetActive(true);
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
        if (astronautTempCost == 250)
        {
            astronautMaxConfirm[id] = true;
        }

        return astronautTempCost;
    }
    // Setting button interactable off and on depending from amount of crystals needed
    public void AstronautsBuyButtonControl(int id)
    {
        if (gameManager.crystalCurrency >= astronautCost[id] && astronautMaxConfirm[id] == false)
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
            if (gameManager.confirmAstronautBuy[id] == false)
            {
                upgradeAstronauts[id].SetActive(false);

            }
            else if (gameManager.confirmAstronautBuy[id] == true)
            {
                upgradeAstronauts[id].SetActive(true);
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
