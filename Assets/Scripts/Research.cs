using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Research : CostCalculator
{
    public GameManager gameManager;
    public UnlockingSystem unlockingSystem;

    public GameObject[] researchSectionObject;
    public Text researchTextField;
    public Text[] researchPriceText;
    public GameObject researchTextWindow;
    public GameObject[] researches;
    public Button[] researchButton;
    public List<GameObject> researchIcon = new List<GameObject>();
    public Image researchWindowIcon;
    public Image[] researchImage;
    public bool[] researchUnlocked;
    public bool[] researchCanBeDone;

    public Image[] researchConnDefault;

    [SerializeField]
    private Sprite researchConnActive;
    [SerializeField]
    private Sprite researchConnDisabled;

    double[] researchCosts;
    public double[] upgradeResearchValues;
    string[] researchText;

    private void Awake()
    {
        researches = gameManager.FindObsWithTag("researchIcon");
        researchUnlocked = new bool[researches.Length];
        researchCanBeDone = new bool[researches.Length];
        upgradeResearchValues = new double[researches.Length];

        for (int id = 0; id < researches.Length; id++)
        {
            researchUnlocked[id] = false;
            researchCanBeDone[id] = false;
        }

        gameManager.ResearchMultiplierCalculator();
        researchPriceText = new Text[researches.Length];
        researchButton = new Button[researches.Length];
        researchImage = new Image[researches.Length];
        AssigningResearchObjects();
    }

    void Start()
    {
        researchText = new string[researches.Length];
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

        ResearchActiveStatusAssign();
    }

    void Update()
    {
        for (int id = 0; id < researchUnlocked.Length; id++)
        {
            ResearchConnectorsCheck(id);
        }

        HideIfClickedOutside(researchTextWindow);
        ResearchButtonStatus();
    }

    public void AssigningResearchObjects()
    {
        int idR = 0;
        int idIcon = 1;
        for (int id = 0; id < researchUnlocked.Length; id++)
        {
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
        double multiplier = 8.3;
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

        for (int id = 0; id < researchUnlocked.Length; id++)
        {

            researchButton[id].interactable = researchCanBeDone[id];

            researchPriceText[id].color = !researchUnlocked[id] ? (gameManager.mainCurrency >= researchCosts[id] ? Color.green : Color.red) : Color.green;

        }
    }

    private void HideIfClickedOutside(GameObject panel)
    {
        if (Input.GetMouseButton(0) && panel.activeSelf && !RectTransformUtility.RectangleContainsScreenPoint(panel.GetComponent<RectTransform>(),
            Input.mousePosition, Camera.main))
        {
            panel.SetActive(false);
        }
    }


    public void ResearchUpgradeButton(int id)
    {
 
        var costResearchUpgrade = researchCosts[id];

        if (gameManager.mainCurrency >= costResearchUpgrade && researchCanBeDone[id] == true && !researchUnlocked[id])
        {
            gameManager.mainCurrency -= costResearchUpgrade;
            researchUnlocked[id] = true;
            researchPriceText[id].text = "Activated";
            unlockingSystem.ResearchUnlocking(id);
        }
        ResearchInfoWindowOnClick(id);
    }

    public void ResearchConnectorsCheck(int id)
    {
        if (researchUnlocked[id] == true)
        {
            researchConnDefault[id].sprite = researchConnActive;
        } else
            researchConnDefault[id].sprite = researchConnDisabled;
    }

    public double ResearchBoost()
    {
        double resBoost = 1;

        for (int id = 0; id < researchUnlocked.Length; id++)
        {
            if (researchUnlocked[id])
            {
                resBoost += upgradeResearchValues[id];
            }
        }
 
        return resBoost;
    }

    public void ResearchActiveStatusAssign()
    {
        for (int id = 0; id < researchUnlocked.Length; id++)
        {
            if (!researchUnlocked[id])
            {
                researchPriceText[id].text = GameManager.ExponentLetterSystem(researchCosts[id], "F2");
            }
            else
            {
                researchPriceText[id].text = "Activated";
                researchPriceText[id].color = Color.green;
            }
        }
    }

}

