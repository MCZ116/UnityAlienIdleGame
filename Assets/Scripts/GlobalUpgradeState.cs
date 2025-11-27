using System;
using UnityEngine;

[System.Serializable]
public class GlobalUpgradeState : MonoBehaviour
{
    public GlobalUpgradeData upgradeData;
    public int level = 0;

    public double GetUpgradeCost(GlobalUpgradeData data)
     => Math.Round(data.baseCost* Math.Pow(data.costMultiplier, level));
}
