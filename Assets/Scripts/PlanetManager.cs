using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlanetManager : MonoBehaviour
{
    [SerializeField] GameManager gameManager;
    [SerializeField] ResearchManager researchManager;
    public GameObject planetButtonPrefab;
    public List<PlanetData> unlockedPlanets = new();
    public List<PlanetData> allPlanets;

    void Awake()
    {
        foreach (var planet in allPlanets)
        {
            if (!unlockedPlanets.Contains(allPlanets[0]))
                unlockedPlanets.Add(allPlanets[0]);
        }
    }

    public bool IsUnlocked(PlanetData planet) => unlockedPlanets.Contains(planet);

    public bool RequirementsMet(PlanetData planet)
    {
        if (IsUnlocked(planet)) return false;

        if (planet.requiredResearch != null && !researchManager.IsUnlocked(planet.requiredResearch))
            return false;

        return true;
    }

    public bool CanUnlock(PlanetData planet)
    {
        return RequirementsMet(planet) && HasEnoughCurrency(planet);
    }

    public bool HasEnoughCurrency(PlanetData planet)
    {
        return gameManager.mainCurrency >= planet.GetPrice();
    }


    public void Unlock(PlanetData planet)
    {
        if (!CanUnlock(planet)) return;

        gameManager.mainCurrency -= planet.GetPrice();
        unlockedPlanets.Add(planet);
    }

    public bool CanAccessPlanet(int planetID)
    {
        if (planetID < 0 || planetID >= allPlanets.Count)
            return false;

        return IsUnlocked(allPlanets[planetID]);
    }

    public int GetNextUnlockedPlanetId(int currentId)
    {
        for (int i = currentId + 1; i < allPlanets.Count; i++)
            if (IsUnlocked(allPlanets[i])) return i;
        return -1;
    }

    public int GetPrevUnlockedPlanetId(int currentId)
    {
        for (int i = currentId - 1; i >= 0; i--)
            if (IsUnlocked(allPlanets[i])) return i;
        return -1;
    }

    public PlanetData GetPlanetUnlockedByResearch(ResearchData research)
    {
        return allPlanets.FirstOrDefault(p => p.requiredResearch == research);
    }

    public void ApplyLoadedData(GameData data, List<PlanetData> allPlanets)
    {
        unlockedPlanets.Clear();

        foreach (int id in data.planetIds)
        {
            PlanetData match = allPlanets.Find(r => r.planetId == id);
            if (match != null)
                unlockedPlanets.Add(match);
        }

        unlockedPlanets.Add(allPlanets[0]); // Ensure starting planet is always unlocked
    }
}
