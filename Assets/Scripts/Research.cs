using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Research : CostCalculator
{
    public GameManager gameManager;
    public UnlockingSystem unlockingSystem;

    public GameObject[] researchSectionObject;
    public Text researchTextField;
    public Text[] researchLevels;
    public Text[] researchPriceText;
    public GameObject researchTextWindow;
    public Button[] researchButton;
    public List<GameObject> researchIcon = new List<GameObject>();
    public Image researchWindowIcon;
    public Image[] researchImage;

    public Image[] researchConnDefault;

    [SerializeField]
    private Sprite researchConnActive;
    [SerializeField]
    private Sprite researchConnDisabled;

    double[] researchCosts;
    public double[] upgradeResearchValues;
    string[] researchText;
    
    void Start()
    {
        researchText = new string[gameManager.Research1Level.Length];
        researchText[0] = "Improve oxgen tanks capacity for about 2%. Better oxygen tanks allow astronauts to stay longer on the surface of the planet";
        researchText[1] = "Ion engines allow us to travel further and faster by using less energy!";
        researchText[2] = "Finally we can drink some water!";
        researchText[3] = "Yummy we can plant cabbage in space!";
        researchText[4] = "Improve oxgen tanks capacity for about 2%. Better oxygen tanks allow astronauts to stay longer on the surface of the planet";
        researchText[5] = "Ion engines allow us to travel further and faster by using less energy!";
        researchText[6] = "Finally we can drink some water!";
        researchText[7] = "Yummy we can plant cabbage in space!";
        researchText[8] = "Test text";
        researchText[9] = "Test text";
        researchText[10] = "Test text";
        researchText[11] = "Test text";
        researchText[12] = "Test text";
        researchText[13] = "Test text";
        researchText[14] = "Test text";
        researchText[15] = "Test text";
        researchCosts = new double[researchText.Length];
        ResearchCostMultiplier();
        researchTextWindow.SetActive(false);
    }

    void Update()
    {
        for (int id = 0; id < gameManager.Research1Level.Length; id++)
        {
            ResearchConnectorsCheck(id);
            if (gameManager.Research1Level[id] >= 1)
            {
                researchLevels[id].enabled = true;
                researchLevels[id].text = gameManager.Research1Level[id].ToString("F0");
                
            }
            else
            {
                researchLevels[id].enabled = false;
            }
            // Using abstract class for test
           researchPriceText[id].text = GameManager.ExponentLetterSystem(CostCalc(id, researchCosts[id],gameManager.Research1Level[id],gameManager.mainCurrency), "F2");
        }
        //HideIfClickedOutside(researchTextWindow);
        ResearchButtonStatus();
    }
    // Need debug
    public void AssigningResearchObjects()
    {
        int idR = 0;
        int idIcon = 1;
        for (int id = 0; id < researchLevels.Length; id++)
        {
            researchLevels[id] = researchSectionObject[idR].transform.Find("ResearchIcon" + idIcon).transform.Find("ResearchLvl").GetComponent<Text>();
            researchPriceText[id] = researchSectionObject[idR].transform.Find("ResearchIcon" + idIcon).transform.Find("ResearchPrice").GetComponent<Text>();
            researchButton[id] = researchSectionObject[idR].transform.Find("ResearchIcon" + idIcon).transform.Find("ResearchUpgrade1").GetComponent<Button>();
            researchImage[id] = researchSectionObject[idR].transform.Find("ResearchIcon" + idIcon).transform.Find("ResearchUpgrade1").GetComponent<Image>();
            idIcon++;
            if((id+1) % 4 == 0)
            {
                idR++;
                idIcon = 1;
            }
        }    
    }

    public void ResearchCostMultiplier()
    {
        double basePrice = 10000;
        double multiplier = 3.2;
        for (int id = 0; id < researchCosts.Length; id++)
        {
            researchCosts[id] = basePrice;
            basePrice *= multiplier;
        }
    }

    public void ResearchInfoWindowOnClick(int id)
    {
        researchTextWindow.SetActive(true);
        researchTextField.text = $"{researchText[id]}";
        researchWindowIcon.sprite = researchImage[id].sprite;
       
    }

    public void ResearchButtonStatus()
    {
       
        for (int id = 0; id < researchLevels.Length; id++)
        {

            if (!gameManager.researchCanBeDone[id])
            {
                researchButton[id].interactable = false;
            }
            else
            {
                researchButton[id].interactable = true;
            }

            if (gameManager.mainCurrency >= CostCalc(id, researchCosts[id], gameManager.Research1Level[id],gameManager.mainCurrency))
            {
                researchPriceText[id].color = Color.green;
            }
            else
                 researchPriceText[id].color = Color.red;

        }
    }

    //private void HideIfClickedOutside(GameObject panel)
    //{
    //    if (Input.GetMouseButton(0) && panel.activeSelf && !RectTransformUtility.RectangleContainsScreenPoint(panel.GetComponent<RectTransform>(),
    //        Input.mousePosition,Camera.main))
    //    {
    //        panel.SetActive(false);
    //    }
    //}


    public void ResearchUpgradeButton(int id)
    {
        double n = 1;
 
        var costResearchUpgrade = CostCalc(id, researchCosts[id], gameManager.Research1Level[id],gameManager.mainCurrency);

        if (gameManager.mainCurrency >= costResearchUpgrade && gameManager.researchCanBeDone[id] == true)
        {
            gameManager.mainCurrency -= costResearchUpgrade;
            gameManager.Research1Level[id] += (int)n;
            ResearchBoost();
            gameManager.researchUnlocked[id] = true;
            unlockingSystem.ResearchUnlocking(id);
        }
        ResearchInfoWindowOnClick(id);
    }

    public void ResearchConnectorsCheck(int id)
    {
        if (gameManager.researchUnlocked[id] == true)
        {
            researchConnDefault[id].sprite = researchConnActive;
        } else
            researchConnDefault[id].sprite = researchConnDisabled;
    }

    public double ResearchBoost()
    {
        double resBoost = 0;

        for (int id = 0; id < gameManager.Research1Level.Length; id++) {
            resBoost += gameManager.Research1Level[id] * upgradeResearchValues[id];
        }

        return resBoost;
    }

}

