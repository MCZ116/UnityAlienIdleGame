using System;
using UnityEngine.UI;
using UnityEngine;

public class OfflineProgress : MonoBehaviour
{
    public GameManager gameManager;
    public GameObject offlineRewards;
    public Text offlineRewardText;
    public Text offlineTimeText;
    public double totalRewards;
    public RewardedAdsButton rewardedAdsButton;
    public float BuffSeconds { get; private set; }

    public void OfflineProgressLoad()
    {
        if (gameManager.TotalIncome() != 0 && PlayerPrefs.HasKey("OfflineTime"))
        {
            // 1. Load saved offline time
            var tempOfflineTime = Convert.ToInt64(PlayerPrefs.GetString("OfflineTime"));
            var oldTime = DateTime.FromBinary(tempOfflineTime);
            var currentTime = DateTime.Now;
            var totalOfflineTime = currentTime - oldTime;

            // 2. Try to get saved buff end time
            DateTime buffEndTime = DateTime.MinValue;
            if (PlayerPrefs.HasKey("BuffEnd"))
            {
                var tempBuffTime = Convert.ToInt64(PlayerPrefs.GetString("BuffEnd"));
                buffEndTime = DateTime.FromBinary(tempBuffTime);
            }

            // 3. Calculate how much of the offline time was under buff
            TimeSpan buffDuration = TimeSpan.Zero;
            if (buffEndTime > oldTime)
            {
                DateTime buffEffectiveEnd = (buffEndTime < currentTime) ? buffEndTime : currentTime;
                buffDuration = buffEffectiveEnd - oldTime;
            }

            float buffSeconds = (float)buffDuration.TotalSeconds;
            BuffSeconds = buffSeconds;

            float normalSeconds = (float)(totalOfflineTime - buffDuration).TotalSeconds;

            double baseIncome = gameManager.TotalIncome();

            // 4. Calculate total rewards
            totalRewards = baseIncome * normalSeconds + baseIncome * 2f * buffSeconds;

            // 5. Show reward UI
            offlineRewards.SetActive(true);
            offlineTimeText.text = $"Offline Time: {totalOfflineTime:d\\dhh\\hmm\\m}";
            offlineRewardText.text = GameManager.ExponentLetterSystem(totalRewards, "F2");

            // 6. Add base income only (double part added if ad watched)
            gameManager.mainCurrency += baseIncome * normalSeconds;
        }
    }

    public void DoubleOfflineIncome()
    {
        rewardedAdsButton.ButtonHandler("DoubleReward");
        rewardedAdsButton.ShowAd();
    }

    public void CloseOfflineProgress()
    {
        offlineRewards.SetActive(false);
    }

}
