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
            AstronautCostText[id].text = astronautCost[id].ToString("F0");
            AstronautsBuyButtonControl(id);
        }
        for (int id = 0; id < astronautsUpgrades.Length; id++)
        {
            Debug.Log(gameManager.confirmAstronautBuy[id] + "  ConfirmAstronautBuy ID" + id);
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

    public void AstronautsAppearing(int id)
    {
        var h = astronautCost[id];
        var c = gameManager.crystalCurrency;
        var r = 2;
        var u = id;
        double n = 1;
        
        if (gameManager.astronautsID[id] <= 3)
        {
            var astronautTempCost = h * (System.Math.Pow(r, u) * (System.Math.Pow(r, n) - 1) / (r - 1));
            if (gameManager.crystalCurrency >= astronautTempCost)
            {
                gameManager.crystalCurrency -= astronautTempCost;
                upgradeAstronauts[gameManager.astronautsID[id]].SetActive(true);
                gameManager.confirmAstronautBuy[gameManager.astronautsID[id]] = true;
                gameManager.astronautsID[id]++;
                AstronautsBoost();
            }
        } 
    }

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

    public double AstronautsBoost()
    {
        double asBoost = 0;

       
            for (int id = 0; id < upgradeAstronauts.Length; id++)
            {

                asBoost += gameManager.astronautsID[id] * 0.15;
                Debug.Log(gameManager.astronautsID[id] + " astronautsLvl ID" +id);
                Debug.Log(asBoost + " asBoost");
                Debug.Log(upgradeAstronauts.Length + " upgradeAstronauts");

            }

            return asBoost;
        
    }

}
