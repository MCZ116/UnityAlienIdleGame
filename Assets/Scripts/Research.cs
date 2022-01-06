using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Research : MonoBehaviour
{
    public GameManager gameManager;
    public UnlockingSystem unlockingSystem;

    public Text researchTextField;
    public Text[] researchLevels;
    public Text[] researchPriceText;
    public GameObject researchTextWindow;
    //public GameObject[] researchIcon;
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
        researchText = new string[4];
        researchText[0] = "Improve oxgen tanks capacity for about 2%. Better oxygen tanks allow astronauts to stay longer on the surface of the planet";
        researchText[1] = "Durability of drills improve digging for about 10%";
        researchText[2] = "Finally we can drink some water!";
        researchText[3] = "Yummy we can plant cabbage in space!";
        researchCosts = new double[4];
        researchCosts[0] = 10000;
        researchCosts[1] = 18000;
        researchCosts[2] = 50000;
        researchCosts[3] = 200000;
        researchTextWindow.SetActive(false);

        foreach (GameObject button in GameObject.FindGameObjectsWithTag("researchIcon"))
        {
            researchIcon.Add(button);
        }
    }

    void Update()
    {
        for (int id = 0; id < gameManager.Research1Level.Length; id++)
        {
            ResearchConnectorsCheck(id);
            Debug.Log("ResearchLevel = " + gameManager.Research1Level[id] + "of ID" + id);
            if (gameManager.Research1Level[id] >= 1)
            {
                researchLevels[id].enabled = true;
                researchLevels[id].text = gameManager.Research1Level[id].ToString("F0");
                
            }
            else
            {
                researchLevels[id].enabled = false;
            }
            researchPriceText[id].text = GameManager.ExponentLetterSystem(ResearchCostCalc(id), "F2");
        }
        HideIfClickedOutside(researchTextWindow);

        ResearchButtonStatus();
    }

    public void ResearchInfoWindowOnClick(int id)
    {
        researchTextWindow.SetActive(true);
        researchTextField.text = $"{researchText[id]}";

        //researchImage[id] = researchIcon[id].GetComponent<Image>();
        researchWindowIcon.sprite = researchImage[id].sprite;
        

    }

    public void ResearchButtonStatus()
    {
       
        for (int id = 0; id < researchLevels.Length; id++)
        {
            //researchButton[id] = researchIcon[id].GetComponent<Button>();

            if (!gameManager.researchCanBeDone[id])
            {
                researchButton[id].interactable = false;
            }
            else
            {
                researchButton[id].interactable = true;
            }

            if (gameManager.mainCurrency >= ResearchCostCalc(id))
            {
                researchPriceText[id].color = Color.green;
            }
            else
                 researchPriceText[id].color = Color.red;

        }
    }

    private void HideIfClickedOutside(GameObject panel)
    {
        if (Input.GetMouseButton(0) && panel.activeSelf && !RectTransformUtility.RectangleContainsScreenPoint(
                panel.GetComponent<RectTransform>(),
                Input.mousePosition,
                Camera.main))
        {
            panel.SetActive(false);
        }
    }

    public double ResearchCostCalc(int id)
    {
        var h = researchCosts[id];
        var c = gameManager.mainCurrency;
        var r = 1.07;
        var u = gameManager.Research1Level[id];
        double n = 1;
        var costResearchUpgrade = h * (System.Math.Pow(r, u) * (System.Math.Pow(r, n) - 1) / (r - 1));

        return costResearchUpgrade;
    }

    public void ResearchUpgradeButton(int id)
    {
        
        var h = researchCosts[id];
        var c = gameManager.mainCurrency;
        var r = 1.07;
        var u = gameManager.Research1Level[id];
        double n = 1;
        var costResearchUpgrade = h * (System.Math.Pow(r, u) * (System.Math.Pow(r, n) - 1) / (r - 1));
        
        if (gameManager.mainCurrency >= costResearchUpgrade && gameManager.researchCanBeDone[id] == true)
        {
            gameManager.mainCurrency -= costResearchUpgrade;
            gameManager.Research1Level[id] += (int)n;
            ResearchBoost();
            gameManager.researchUnlocked[id] = true;
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

