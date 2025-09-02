using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResearchManager : MonoBehaviour
{
    public GameManager gameManager;
    [SerializeField] private InfoWindow infoWindow;
    public GameObject researchButtonPrefab;
    public List<ResearchRow> researchRows;
    public List<ResearchData> unlockedResearches = new();
    public List<ResearchData> allResearches;

    void Awake()
    {
        // Collect all researches from researchRows
        allResearches = new List<ResearchData>();
        foreach (var row in researchRows)
        {
            if (row.researches != null)
            {
                allResearches.AddRange(row.researches);
            }
        }
    }

    private IEnumerator Start()
    {
        foreach (var row in researchRows)
        {
            foreach (var research in row.researches)
            {
                GameObject buttonGO = Instantiate(researchButtonPrefab, row.rowParent);
                var buttonUI = buttonGO.GetComponent<ResearchButtonUI>();
                buttonUI.Initialize(research, this, infoWindow);

                var infoButton = buttonGO.GetComponentInChildren<InfoButton>();
                if (infoButton != null)
                {
                    infoButton.SetTarget(research);
                    infoButton.infoWindow = infoWindow;
                }
            }
        }

        // Wait until LayoutGroups position buttons
        yield return new WaitForEndOfFrame();

        // Draw lines
        FindObjectOfType<ResearchTreeLineDrawerUI>().DrawAllLines();
        FindObjectOfType<ResearchTreeLineDrawerUI>().UpdateLineColors(this);
    }

    public bool IsUnlocked(ResearchData research) => unlockedResearches.Contains(research);

    public bool CanUnlock(ResearchData research)
    {
        if (IsUnlocked(research)) return false;
        foreach (var pre in research.requiredResearches ?? new List<ResearchData>())
        {
            if (!IsUnlocked(pre)) return false;
        }
        return gameManager.mainCurrency >= research.price;
    }

    public void Unlock(ResearchData research)
    {
        if (!CanUnlock(research)) return;

        gameManager.mainCurrency -= research.price;
        unlockedResearches.Add(research);
        FindObjectOfType<ResearchTreeLineDrawerUI>().UpdateLineColors(this);
    }

    public double GetTotalIncome(double baseIncome)
    {
        double totalMultiplier = 1.0;

        foreach (var research in unlockedResearches)
        {
            totalMultiplier *= research.GetIncomeMultiplier(); // multiplicative stacking
        }

        return baseIncome * totalMultiplier;
    }

    public void ApplyLoadedData(GameData data, List<ResearchData> allResearches)
    {
        unlockedResearches.Clear();

        foreach (int id in data.researchIds)
        {
            ResearchData match = allResearches.Find(r => r.researchId == id);
            Debug.Log(match.ToString());
            if (match != null)
                unlockedResearches.Add(match);
        }
    }

}
