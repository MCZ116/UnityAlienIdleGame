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

    void Awake()
    {
        AssignAstronautSection();
        AssigningAstronautsOnStart();
    }

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

        
        // It's here because first load of game not always check that so all buttons are interactable
        AstronautButtonTextCheck();
    }

    void Update()
    {
        AstronautButtonTextCheck();
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
        astronautsBuyButton = new Button[gameManager.stageObjects.Length];
        AstronautCostText = new Text[gameManager.stageObjects.Length];
        astronautsObjectsContainer = new GameObject[gameManager.stageObjects.Length];

        for (int id = 0; id < gameManager.stageObjects.Length; id++)
        {
            astronautsBuyButton[id] = gameManager.stageObjects[id].transform.Find("BuyAstronautsButton").GetComponent<Button>();
            AstronautCostText[id] = gameManager.stageObjects[id].transform.Find("BuyAstronautsButton").GetComponentInChildren<Text>();
            astronautsObjectsContainer[id] = gameManager.stages[id].transform.GetChild(2).gameObject;
        }

    }

    public void AssigningAstronautsOnStart()
    {
        int currentIndex = 0;

        for (int containerIndex = 0; containerIndex < astronautsObjectsContainer.Length; containerIndex++)
        {
            Transform containerTransform = astronautsObjectsContainer[containerIndex].transform;

            for (int childIndex = 0; childIndex < containerTransform.childCount; childIndex++)
            {
                astronautsUpgrades[currentIndex] = containerTransform.GetChild(childIndex).gameObject;
                currentIndex++;
            }
        }
    }

    public void AstronautsBuyButton(int id)
    {
        var h = astronautCost[id];
        var astronautTempCost = h * (gameManager.astronautsLevel[id]+1);
        
        if (astronautTempCost == 200) { astronautTempCost = 300; }

        if ( astronautTempCost <= 300 && gameManager.astronautsLevel[id] <= 3 && gameManager.crystalCurrency >= astronautTempCost)
        {  
            gameManager.crystalCurrency -= astronautTempCost;
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

    public void AstronautsObjectActivationControl()
    {
        for (int id = 0; id < astronautsUpgrades.Length; id++)
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

            asBoost += (gameManager.astronautsLevel[id] * 2) * ((gameManager.StageLevel[id] * gameManager.stageIncome[id]));
            
        }
        return asBoost;   
    }
}
