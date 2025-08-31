using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Research", menuName = "ScriptableObjects/Research Node")]
public class ResearchData : ScriptableObject, IDescribable
{
    public string researchName;
    public double price = 10000;
    public List<ResearchData> requiredResearches;
    public Sprite icon;
    public int tier;
    public string description;

    [Header("Effects")]
    public bool useFormula = true;
    [Tooltip("Custom multiplier (1.1 = +10%) if useFormula = false")]
    public double customMultiplier = 1.0;
    [Tooltip("Multiplier per tier (0.1 = 10%) if useFormula = true")]
    public double formulaPercentPerTier = 0.1;

    public string Description => description;
    public Sprite Icon => icon;

    public double GetPrice()
    {
        return Math.Round(price * Math.Pow(2, tier));
    }

    public double GetIncomeMultiplier()
    {
        return useFormula ? 1.0 + formulaPercentPerTier * tier : customMultiplier;
    }
}