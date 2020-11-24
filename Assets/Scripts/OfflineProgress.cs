using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class OfflineProgress : MonoBehaviour
{
    public IdleScript idleScript;
    public GameObject offlineRewards;
    public Text offlineRewardText;
    public Text offlineTimeText;


    public void OfflineProgressLoad()
    {

        var tempOfflineTime = Convert.ToInt64(PlayerPrefs.GetString("OfflineTime"));
        var oldTime = DateTime.FromBinary(tempOfflineTime);
        var currentTime = DateTime.Now;
        var difference = currentTime.Subtract(oldTime);
        var rawTime = (float)difference.TotalSeconds;
        var offlineTime = rawTime;

        offlineRewards.SetActive(true);
        TimeSpan timeSpan = TimeSpan.FromSeconds(rawTime);
        offlineTimeText.text = $"{timeSpan:dd\\:hh\\:mm\\:ss}";

        double totalRewards = idleScript.upgradeLevel1 * offlineTime;
        idleScript.mainCurrency += totalRewards;
        offlineRewardText.text = totalRewards.ToString("F0");
    }

    public void CloseOfflineProgress()
    {
        offlineRewards.SetActive(false);
    }

}
