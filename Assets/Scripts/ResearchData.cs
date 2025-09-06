using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "Research", menuName = "ScriptableObjects/Research Node")]
public class ResearchData : ScriptableObject, IDescribable
{
    public int researchId;
    public string researchName;
    public double basePrice = 10000;
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

    [Header("Planet Unlock (optional)")]
    public int planetId = -1; // -1 means no planet unlock
    public Sprite planetIcon;

    public double GetPrice()
    {
        return Math.Round(basePrice * Math.Pow(2, tier));
    }

    public double GetIncomeMultiplier()
    {
        return useFormula ? 1.0 + formulaPercentPerTier * tier : customMultiplier;
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        if (researchId == 0)
        {
            string guid = AssetDatabase.AssetPathToGUID(AssetDatabase.GetAssetPath(this));
            researchId = Mathf.Abs(guid.GetHashCode()); // unique int
            EditorUtility.SetDirty(this);
        }
    }
#endif
}