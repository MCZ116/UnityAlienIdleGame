using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class OfflineProgress : MonoBehaviour
{
    //public IdleScript idleScript;
    //public GameObject offlineRewards;
    //public Text offlineRewardText;
    //public DateTime currentTime;
    //public DateTime oldTime;

    //public void OfflineProgressLoad()
    //{
    //    GameData gameData = SaveSystem.LoadData();
    //    var oldTime = gameData.oldTimeSave;
    //    var currentTime = DateTime.Now;
    //    var difference = currentTime.Subtract(oldTime);
    //    var rawTime = (float)difference.TotalSeconds;
    //    var offlineTime = rawTime / 10;

    //    offlineRewards.SetActive(true);
    //    TimeSpan timeSpan = TimeSpan.FromSeconds(rawTime);

    //    double totalRewards = idleScript.researchPointsPerSecond * offlineTime;
    //    offlineRewardText.text = totalRewards.ToString("F0");
    //}

    //public void CloseOfflineProgress()
    //{
    //    offlineRewards.SetActive(false);
    //}

}
