using System;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BuildingState : MonoBehaviour
{
    public BuildingData data;
    public int level = 0;
    public float currentProgress; // for progress bar, optional
    public double profitPerSecond;  // income rate
    public float timer;
    public bool IsUnlocked => level > 0;
    public GameObject placeholderVisual;
    public GameObject activeVisual;

    [Header("Astronauts")]
    public List<GameObject> astronautObjects; // assign in inspector
    public int astronautsHired = 0; // extra unlocked astronauts (first one free)
    public int lastCrystalCheckpoint = 0;

    [SerializeField] ResearchManager researchManager;

    public double GetCurrentPrice()
    {
        double indexMultiplier = Math.Pow(data.buildingMultiplier, data.buildingIndex);
        double levelMultiplier = Math.Pow(data.levelMultiplier, level);

        return data.basePrice * indexMultiplier * levelMultiplier;
    }

    public double GetBaseProfit(BuildingData data)
    {
        // Example: 8x per building
        return Math.Pow(data.baseProfitMultiplier, data.buildingIndex);
    }

    public double GetCurrentProfit()
    {
        if (!IsUnlocked) return 0;

        double buildingBase = Math.Pow(data.baseProfitMultiplier, data.buildingIndex);
        double levelGrowth = Math.Pow(1.15, level - 1);
        double totalProfit = buildingBase * levelGrowth;
        double astronautMultiplier = data.GetAstronautMultiplier(astronautsHired);
        totalProfit *= astronautMultiplier;

        return totalProfit;
    }

    public int GetAstronautCost()
    {
        if (astronautsHired >= data.maxAstronauts)
            return 0;

        return data.astronautBaseCost + astronautsHired * data.astronautCostStep;
    }

    public void UnlockNextAstronaut()
    {
        if (astronautsHired + 1 >= astronautObjects.Count) return;

        astronautsHired++;
        var astronaut = astronautObjects[astronautsHired];
        astronaut.SetActive(true);
    }

    public void RestoreAstronauts()
    {
        if (!IsUnlocked)
        {
            foreach (var astro in astronautObjects)
                astro.SetActive(false);
            return;
        }
        // First astronaut is always active if building is unlocked
        astronautObjects[0].SetActive(true);
        // Ensure only correct astronauts are active
        for (int i = 0; i < astronautObjects.Count; i++)
        {
            astronautObjects[i].SetActive(i <= astronautsHired);
        }
    }

    public void UpdateVisuals()
    {
        bool unlocked = IsUnlocked;

        placeholderVisual.SetActive(!unlocked);
        activeVisual.SetActive(unlocked);
        astronautObjects[0].SetActive(true);
    }

}
