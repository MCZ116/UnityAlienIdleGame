using UnityEngine;
using UnityEngine.UI;

public class IdleScript : MonoBehaviour
{

    public int delayTime = 1;
    public Text LightPowerText;
    public Text CurrencyText;
    public Text RPointsText;
    public Text ButtonLightsText;
    public double mainCurrency;
    public double lightUpgradeCosts;
    public double researchPointsPerSecond;
    public double upgradeLevel1;

    public GameObject alienScreen;

    public CanvasGroup canvasShop;

    public CanvasGroup canvasMainGame;

    private double lightPower;

    public double LightPower { get => lightPower; set => lightPower = value; }

    void Start()
    {
        Application.targetFrameRate = 30;
        mainCurrency = 100;
        lightPower = 0;
        lightUpgradeCosts = 25;
        upgradeLevel1 = 0;
    }

    void Update()
    {
        researchPointsPerSecond = upgradeLevel1;

        LightPowerText.text = "Light Energy: " + lightPower.ToString("F0");
        CurrencyText.text = "Research Points: " + mainCurrency.ToString("F0");
        RPointsText.text = researchPointsPerSecond.ToString("F0") + "RP/s ";
        ButtonLightsText.text = "Upgrade: " + lightUpgradeCosts.ToString("F0");

        mainCurrency += researchPointsPerSecond * Time.deltaTime;
    }

    public void canvasGroupMenuSwitch(bool status, CanvasGroup choosenGroup)
    {
        if (status)
        {
            choosenGroup.alpha = 1;
            choosenGroup.interactable = true;
            choosenGroup.blocksRaycasts = true;
        }
        else
        {
            choosenGroup.alpha = 0;
            choosenGroup.interactable = false;
            choosenGroup.blocksRaycasts = false;
        }
    }

    public void changeTab(string tabName)
    {
        switch (tabName)
        {
            case "gameMenu":
                canvasGroupMenuSwitch(true, canvasMainGame);
                canvasGroupMenuSwitch(false, canvasShop);
                alienScreen.SetActive(true);
                break;

            case "shopMenu":
                canvasGroupMenuSwitch(false, canvasMainGame);
                canvasGroupMenuSwitch(true, canvasShop);
                alienScreen.SetActive(false);
                break;
        } 
    }

    //public void Save()
    //{

    //    mainCurrency = double.Parse(PlayerPrefs.GetString("mainCurrency", "100"));
    //    lightPower = double.Parse(PlayerPrefs.GetString("lightPower", "0"));
    //    lightUpgradeCosts = double.Parse(PlayerPrefs.GetString("lightUpgradeCosts", "25"));
    //    upgradeLevel1 = double.Parse(PlayerPrefs.GetString("upgradeLevel1", "0"));

    //}

    //public void Load()
    //{
    //    PlayerPrefs.SetString("mainCurrency", mainCurrency.ToString());
    //    PlayerPrefs.SetString("lightPower", lightPower.ToString());
    //    PlayerPrefs.SetString("lightUpgradeCosts", lightUpgradeCosts.ToString());
    //    PlayerPrefs.SetString("upgradeLevel1", mainCurrency.ToString());
    //}

    public void LightButtonClicked()
    {
        if (mainCurrency >= lightUpgradeCosts)
        {
            mainCurrency -= lightUpgradeCosts;
            lightUpgradeCosts *= 1.07;
            upgradeLevel1++;
            upgradeLevel1 *= 1.07;
            lightPower++;
     
        }
    }
}
