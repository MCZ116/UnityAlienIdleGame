using Unity.Android.Gradle.Manifest;
using UnityEngine;

[System.Serializable]
public class BuildingState : MonoBehaviour
{
    public BuildingData data;
    public int level;
    public int astronautsHired;
    public float currentProgress; // for progress bar, optional
    public double profitPerSecond;  // income rate
    public float timer;
    [SerializeField] ResearchManager researchManager;

    public double GetCurrentPrice()
    {
        return data.basePrice
               * Mathf.Pow((float)data.buildingMultiplier, data.buildingIndex)
               * Mathf.Pow((float)data.levelMultiplier, level);
    }

    public double GetCurrentProfit()
    {
        // Base profit (depends on level, etc.)
        double baseProfit = data.GetProfit(level);

        // Apply astronaut bonus
        return baseProfit * data.GetAstronautMultiplier(astronautsHired);
    }


    public double GetCurrentAstronautsPrice()
    {
        return data.GetAstronautCost(astronautsHired);
    }
}
