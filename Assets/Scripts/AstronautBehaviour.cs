using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AstronautBehaviour : MonoBehaviour
{
    [SerializeField]
    private GameManager gameManager;

    public GameObject[] upgradeAstronauts;
    private GameObject[] astronautsUpgrades;

    private Animator animationIdle;

    private double[] astronautCost = { 50, 100, 150, 300 };

    public Text[] AstronautCostText;

    public Button[] astronautsBuyButton;

 


    void Start()
    {
        gameManager.astronautsID = new int[4];
        AssigningAstronautsOnStart();
    }

    void Update()
    {
        for (int id = 0; id < AstronautCostText.Length; id++)
        {
            AstronautCostText[id].text = AstronautPriceDisplay(id).ToString("F0");
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

            if(gameManager.confirmAstronautBuy[gameManager.astronautsID[id]] == true)
            {
                upgradeAstronauts[gameManager.astronautsID[id]].SetActive(true);
            }
            Debug.Log(gameManager.confirmAstronautBuy[id] + "  ConfirmAstronautBuy ID" + id);
        }
    }
    // Button for buying astronauts
    public void AstronautsAppearing(int id)
    {
        var h = astronautCost[id];

        var astronautTempCost = h * AstronautsLvlAssigning();
        if(astronautTempCost > 150) { astronautTempCost = 300;}

        if (gameManager.astronautsID[id] <= 3)
        {
            if (gameManager.crystalCurrency >= astronautTempCost)
            {
                gameManager.crystalCurrency -= astronautTempCost;
                upgradeAstronauts[gameManager.astronautsID[id]].SetActive(true);
                gameManager.confirmAstronautBuy[gameManager.astronautsID[id]] = true;
                gameManager.astronautsID[id]++;
                AstronautsLvlAssigning();
                AstronautsBoost();
            }
        }
    }
    // Calculating price for displaying
    public double AstronautPriceDisplay(int id)
    {
        var h = astronautCost[id];

        var astronautTempCost = h * AstronautsLvlAssigning();
        if (astronautTempCost > 150) { astronautTempCost = 300; }

        return astronautTempCost;
    }
    // Setting button interactable off and on depending from amount of crystals needed
    public void AstronautsBuyButtonControl(int id)
    {
        if (gameManager.crystalCurrency >= astronautCost[id])
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
        for (int id = 0; id < 4; id++)
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
    public double AstronautsLvlAssigning()
    {
        double AstroLvl = 1;
        for (int id = 0; id < upgradeAstronauts.Length; id++)
        {

            AstroLvl += gameManager.astronautsID[id];

        }

        return AstroLvl;
    }

    // A function which calculate an income boost of research points based on level of astronauts
    public double AstronautsBoost()
    {
        double asBoost = 0;

       
            for (int id = 0; id < upgradeAstronauts.Length; id++)
            {

                asBoost += gameManager.astronautsID[id] * 0.15;
                //Debug.Log(gameManager.astronautsID[id] + " astronautsLvl ID" +id);
                //Debug.Log(asBoost + " asBoost");
                //Debug.Log(upgradeAstronauts.Length + " upgradeAstronauts");

            }

            return asBoost;
        
    }

}
