using System.Collections.Generic;
using Unity.Android.Gradle.Manifest;
using UnityEngine;

[System.Serializable]
public class BuildingState : MonoBehaviour
{
    public BuildingData data;
    public int level;
    public float currentProgress; // for progress bar, optional
    public double profitPerSecond;  // income rate
    public float timer;
    public bool IsUnlocked => level > 0;
    public GameObject placeholderVisual;
    public GameObject activeVisual;

    [Header("Astronauts")]
    public List<GameObject> astronautObjects; // assign in inspector
    public int astronautsHired; // extra unlocked astronauts (first one free)

    [SerializeField] ResearchManager researchManager;

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

    public double GetCurrentPrice()
    {
        return data.basePrice
               * Mathf.Pow((float)data.buildingMultiplier, data.buildingIndex)
               * Mathf.Pow((float)data.levelMultiplier, level);
    }

    public double GetCurrentProfit()
    {
        if (!IsUnlocked) return 0;
        double baseProfit = data.GetProfit(level);

        // Apply astronaut bonus
        return baseProfit * data.GetAstronautMultiplier(astronautsHired);
    }


    public double GetCurrentAstronautsPrice()
    {
        return data.GetAstronautCost(astronautsHired);
    }
}
