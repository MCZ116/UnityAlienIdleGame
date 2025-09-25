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
    public AdsManager adsManager;
    public float BuffSeconds { get; private set; }

    public void OfflineProgressLoad()
    {
        if (gameManager.GetTotalIncomePerSecond() <= 0 || !PlayerPrefs.HasKey("OfflineTime"))
            return;

        // 1. Load saved offline time
        var tempOfflineTime = Convert.ToInt64(PlayerPrefs.GetString("OfflineTime"));
        var oldTime = DateTime.FromBinary(tempOfflineTime);
        var currentTime = DateTime.Now;
        var totalOfflineTime = currentTime - oldTime;

        // 2. Buff end time (if any)
        DateTime buffEndTime = DateTime.MinValue;
        if (PlayerPrefs.HasKey("BuffEnd"))
        {
            var tempBuffTime = Convert.ToInt64(PlayerPrefs.GetString("BuffEnd"));
            buffEndTime = DateTime.FromBinary(tempBuffTime);
        }

        // 3. Calculate buffed vs normal time
        TimeSpan buffDuration = TimeSpan.Zero;
        if (buffEndTime > oldTime)
        {
            DateTime buffEffectiveEnd = (buffEndTime < currentTime) ? buffEndTime : currentTime;
            buffDuration = buffEffectiveEnd - oldTime;
        }

        BuffSeconds = (float)buffDuration.TotalSeconds;
        float normalSeconds = (float)(totalOfflineTime - buffDuration).TotalSeconds;

        double incomePerSecond = gameManager.GetTotalIncomePerSecond();

        // 4. Total rewards
        totalRewards = incomePerSecond * normalSeconds + incomePerSecond * 2f * BuffSeconds;

        // 5. Show UI
        offlineRewards.SetActive(true);
        offlineTimeText.text = $"Offline Time: {totalOfflineTime:d\\dhh\\hmm\\m}";
        offlineRewardText.text = GameManager.ExponentLetterSystem(totalRewards);

        // 6. Add base reward immediately
        gameManager.AddCurrency(totalRewards);
    }

    public void DoubleOfflineReward()
    {
        // Double by adding the same reward again
        gameManager.AddCurrency(totalRewards);
        CloseOfflineProgress();
    }

    public void CloseOfflineProgress()
    {
        offlineRewards.SetActive(false);
    }

}
