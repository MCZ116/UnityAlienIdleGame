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


    public void OfflineProgressLoad()
    {
        if (gameManager.ResearchPointsCalculator() != 0)
        {
            var tempOfflineTime = Convert.ToInt64(PlayerPrefs.GetString("OfflineTime"));
            var oldTime = DateTime.FromBinary(tempOfflineTime);
            var currentTime = DateTime.Now;
            var difference = currentTime.Subtract(oldTime);
            var rawTime = (float)difference.TotalSeconds;
            var offlineTime = rawTime;

            offlineRewards.SetActive(true);
            TimeSpan timeSpan = TimeSpan.FromSeconds(rawTime);
            offlineTimeText.text = $"Time: {timeSpan:dd\\:hh\\:mm\\:ss}";

            totalRewards = gameManager.ResearchPointsCalculator() * offlineTime;
            Debug.Log(totalRewards + "OutcomeOffline");
            gameManager.mainCurrency += totalRewards;
            offlineRewardText.text = GameManager.ExponentLetterSystem(totalRewards, "F2");
        }
    }

    public void CloseOfflineProgress()
    {
        offlineRewards.SetActive(false);
    }

}
