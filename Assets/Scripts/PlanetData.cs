using System;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "PlanetData", menuName = "ScriptableObjects/PlanetData")]
public class PlanetData : ScriptableObject, IDescribable
{
    public string planetName;
    [SerializeField] Sprite icon;
    public double basePrice = 100000;
    public int planetId;
    public int tier;
    public string description;
    public string Description => description;
    public Sprite Icon => icon;

    [Header("Unlock requirements")]
    public ResearchData requiredResearch;


    public double GetPrice()
    {
        return basePrice * Mathf.Pow(2, tier);
    }

    public string GetResearchRequirement()
    {
        return requiredResearch != null ? requiredResearch.researchName : "";
    }

}
