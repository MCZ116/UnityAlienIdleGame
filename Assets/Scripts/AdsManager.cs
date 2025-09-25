using System;
using Unity.Services.Core;
using Unity.Services.LevelPlay;
using UnityEngine;
using UnityEngine.UI;

public class AdsManager : MonoBehaviour
{
    [SerializeField] private string rewardedAdUnitId;
    [SerializeField] private bool useTestMode = false; // Toggle this in Inspector when go live or remove

    [Header("Your buttons")]
    [SerializeField] private Button spinButton;
    [SerializeField] private Button doubleRewardButton;
    [SerializeField] private Button bonusTimeButton;

    private LevelPlayRewardedAd rewardedAd;
    private string currentButton;

    public GameManager gameManager;
    public OfflineProgress offlineProgress;
    public SpinWheel spinWheel;
    public BonusManager bonusManager;

    async void Start()
    {
        SetButtons(false);

        // Hook up buttons
        spinButton.onClick.AddListener(() => ShowRewardedAd("SpinButton"));
        doubleRewardButton.onClick.AddListener(() => ShowRewardedAd("DoubleReward"));
        bonusTimeButton.onClick.AddListener(() => ShowRewardedAd("DoubleTime"));

        if (useTestMode)
    {
            Debug.Log("Ads in TEST MODE – skipping Unity Services init");
            SetButtons(true); // allow testing immediately
            return;
        }

        // Wait for Unity Services to initialize
        try
        {
            await UnityServices.InitializeAsync();
            Debug.Log("Unity Services Initialized");
            LoadRewardedAd();
        }
        catch (Exception e)
        {
            Debug.LogError("Failed to initialize Unity Services: " + e.Message);
        }
    }

    void LoadRewardedAd()
    {
        rewardedAd = new LevelPlayRewardedAd(rewardedAdUnitId);

        // Disable buttons until ad is ready
        SetButtons(false);

        rewardedAd.OnAdLoaded += OnRewardedLoaded;
        rewardedAd.OnAdLoadFailed += OnRewardedFailed;
        rewardedAd.OnAdRewarded += OnRewardedEarned;
        rewardedAd.OnAdClosed += OnRewardedClosed;

        rewardedAd.LoadAd();
    }

    void OnRewardedLoaded(LevelPlayAdInfo adInfo)
    {
        Debug.Log("Rewarded ad loaded!");
        SetButtons(true);
    }

    void OnRewardedFailed(LevelPlayAdError error)
    {
        Debug.Log("Rewarded failed: " + error);
        SetButtons(false);
    }

    void ShowRewardedAd(string button)
    {
        if (useTestMode)
        {
            Debug.Log("Test Mode: Simulating ad watched for " + button);
            currentButton = button;
            OnRewardedEarned(null, null);
            return;
        }

        if (rewardedAd != null && rewardedAd.IsAdReady())
        {
            currentButton = button;
            SetButtons(false);
            rewardedAd.ShowAd();
        }
        else
        {
            Debug.Log("Ad not ready yet!");
        }
    }

    void OnRewardedEarned(LevelPlayAdInfo adInfo, LevelPlayReward reward)
    {
        Debug.Log("Player earned reward from " + currentButton);

        switch (currentButton)
        {
            case "DoubleReward":
                offlineProgress.DoubleOfflineReward();
                break;

            case "SpinButton":
                spinWheel.SpinWheelMenu();
                spinWheel.SpinWheelButton();
                break;

            case "DoubleTime":
                bonusManager.ClickedBonusBtn();
                break;
        }
    }

    void OnRewardedClosed(LevelPlayAdInfo adInfo)
    {
        Debug.Log("Rewarded closed, load a new ad");

        if (!useTestMode)
        {
        // Destroy old ad before creating a new one
        rewardedAd.DestroyAd();
        LoadRewardedAd(); // reload for next round
        }
    }

    void SetButtons(bool state)
    {
        Debug.Log("Setting buttons to " + state);
        spinButton.interactable = state;
        doubleRewardButton.interactable = state;
        bonusTimeButton.interactable = state;
    }
}
