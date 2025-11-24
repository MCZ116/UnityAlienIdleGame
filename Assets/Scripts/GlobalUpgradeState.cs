using UnityEngine;

[System.Serializable]
public class GlobalUpgradeState : MonoBehaviour
{
    public GlobalUpgradeData upgradeData;
    public int level = 0;

    public double GetUpgradeCost(GlobalUpgradeData data)
     => data.baseCost * Mathf.Pow(data.costMultiplier, level);
}
