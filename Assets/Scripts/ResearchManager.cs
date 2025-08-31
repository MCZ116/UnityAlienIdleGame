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
}
