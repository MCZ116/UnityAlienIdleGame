using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;

public class RewardPopup : MonoBehaviour
{
    public static RewardPopup Instance;

    [Header("UI")]
    public Image rewardIcon;
    public TextMeshProUGUI amountText;
    public Button collectButton;
    public Button doubleButton;
    public Button closeButton;

    [Header("Icons")]
    public Sprite coinIcon;
    public Sprite crystalIcon;

    private double baseAmount;
    private bool isCrystal;

    public GameObject root;
    public GameObject buttons;

    void Awake()
    {
        Instance = this;
        gameObject.SetActive(false);

        collectButton.onClick.AddListener(OnCollect);
        doubleButton.onClick.AddListener(OnDouble);
        closeButton.onClick.AddListener(Close);

        closeButton.gameObject.SetActive(false);
    }

    public void Show(double amount, bool crystal)
    {
        baseAmount = amount;
        isCrystal = crystal;

        amountText.text = GameManager.ExponentLetterSystem(amount, "F0");

        rewardIcon.sprite = isCrystal ? crystalIcon : coinIcon;

        GameManager.PositionIconNextToText(amountText,rewardIcon,20f);

        root.SetActive(true);
    }

    public void OnCollect()
    {
        GiveReward(baseAmount, isCrystal);
        Close();
    }

    private void GiveReward(double amount, bool crystal)
    {
        if (crystal)
            GameManager.instance.AddCrystal(amount);
        else
            GameManager.instance.AddCurrency(amount);
    }

    void OnDouble()
    {
        AdsManager.Instance.PrepareDoubleReward(baseAmount, isCrystal);
        AdsManager.Instance.ShowRewardedAd("SpinDoublePopup");
    }

    public void ApplyDoubleReward()
    {
        buttons.SetActive(false);

        GameManager.instance.AddReward(baseAmount * 2, isCrystal);
        amountText.text = GameManager.ExponentLetterSystem(baseAmount * 2, "F0");
        GameManager.PositionIconNextToText(amountText, rewardIcon, 20f);

        closeButton.gameObject.SetActive(true);
    }

    public void Close()
    {
        gameObject.SetActive(false);
        closeButton.gameObject.SetActive(false);
        buttons.SetActive(true);
    }

}
