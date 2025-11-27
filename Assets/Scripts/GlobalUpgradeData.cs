using UnityEngine;

[CreateAssetMenu(fileName = "NewGlobalUpgrade", menuName = "ScriptableObjects/GlobalUpgrade")]
public class GlobalUpgradeData : ScriptableObject
{
    public string upgradeName;
    public string description;
    public Sprite icon;

    public float baseCost = 10;         
    public float costMultiplier = 1.25f;     
    public float baseBonus = 0.05f;         
    public UpgradeType type;         
}

public enum UpgradeType
{
    BuildingIncome,
    OfflineIncome,
    ResearchCost,
    AstronautsIncome,
    GlobalBoost
}
