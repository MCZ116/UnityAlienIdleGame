using UnityEngine;

[CreateAssetMenu(fileName = "BuildingData", menuName = "ScriptableObjects/BuildingData")]
public class BuildingData : ScriptableObject, IDescribable
{
    [Header("Building Settings")]
    public string buildingName;
    public string description;

    public Sprite icon;
    public string Description => description;
    public Sprite Icon => icon;

    [Header("Price Formula Parameters")]
    public double basePrice = 100;           // cost of first building
    public double buildingMultiplier = 1.15; // next building more expensive
    public double levelMultiplier = 1.10;    // upgrades get more expensive
    public int buildingIndex; // set automatically

    [Header("Level Settings")]
    public int maxLevel = 1000; // cap (you can tune this per building)

    [Header("Profit Formula Parameters")]
    public double baseProfit;
    public float incomeInterval = 10f;
    public double baseProfitMultiplier = 8;

    [Header("Astronaut Parameters")]
    public int maxAstronauts = 4;
    public int astronautBaseCost = 50;  // first astronaut
    public int astronautCostStep = 50;  // added cost for each new astronaut
    public double astronautBonusPercent = 0.1; // 10% profit per astronaut

    public double GetProfit(int level)
    {
        return baseProfit * level;
    }

    public double GetAstronautMultiplier(int astronautsHired)
    {
        // Each astronaut gives +10%, multiplicative stacking
        return 1.0 + (astronautBonusPercent * astronautsHired);
    }

}
