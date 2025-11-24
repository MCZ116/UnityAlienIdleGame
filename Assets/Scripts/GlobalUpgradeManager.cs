using System;
using System.Collections.Generic;
using UnityEngine;

public class GlobalUpgradeManager : MonoBehaviour
{
    public List<GlobalUpgradeState> playerUpgrades = new();
    [SerializeField] BuildingManager buildingManager;

    public void AddAntimatter(double amount)
    {
        GameManager.instance.antiMatter += amount;
        UpdateUI();
    }

    public void BuyUpgrade(GlobalUpgradeState state)
    {
        double cost = state.GetUpgradeCost(state.upgradeData);

        if (GameManager.instance.antiMatter >= cost)
        {
            GameManager.instance.antiMatter -= cost;
            state.level++;
            UpdateUI();
        }
    }

    public void UpdateUI() 
    { 
        GameManager.instance.AntiMatter.text = GameManager.ExponentLetterSystem(GameManager.instance.antiMatter);
    }

    public double CalculateAntimatterReward(double totalEarnings)
    {
        return Math.Floor(Math.Pow(totalEarnings / 1e12, 0.25));
    }

    public double GetGlobalIncomeBoost()
    {
        double boost = 1.0;

        foreach (var state in playerUpgrades)
        {
            if (state.upgradeData.type == UpgradeType.BuildingIncome)
                boost += state.upgradeData.baseBonus * state.level;
            if (state.upgradeData.type == UpgradeType.AstronautsIncome)
                boost += state.upgradeData.baseBonus * state.level;
            if (state.upgradeData.type == UpgradeType.ResearchCost)
                boost += state.upgradeData.baseBonus * state.level;
            if (state.upgradeData.type == UpgradeType.OfflineIncome)
                boost += state.upgradeData.baseBonus * state.level;
            if (state.upgradeData.type == UpgradeType.GlobalBoost)
                boost += state.upgradeData.baseBonus * state.level;
        }

        return boost;
    }

    public double GetGlobalIncomeBoostWithPreview(GlobalUpgradeData preview)
    {
        double boost = 1.0;
        foreach (var state in playerUpgrades)
        {
            int level = state.level;
            if (state.upgradeData == preview)
                level += 1; // simulate next level for preview
            if (state.upgradeData.type == UpgradeType.BuildingIncome)
                boost += state.upgradeData.baseBonus * level;
            if (state.upgradeData.type == UpgradeType.AstronautsIncome)
                boost += state.upgradeData.baseBonus * level;
            if (state.upgradeData.type == UpgradeType.ResearchCost)
                boost += state.upgradeData.baseBonus * level;
            if (state.upgradeData.type == UpgradeType.OfflineIncome)
                boost += state.upgradeData.baseBonus * level;
            if (state.upgradeData.type == UpgradeType.GlobalBoost)
                boost += state.upgradeData.baseBonus * level;
        }
        return boost;
    }

    public void ApplyLoadedData(GameData data)
    {
        for (int i = 0; i < playerUpgrades.Count; i++)
        {
            playerUpgrades[i].level = data.globalUpgradeLevels[i];
        }
    }

}
