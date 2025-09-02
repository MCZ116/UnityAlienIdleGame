using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResearchTreeLineDrawerUI : MonoBehaviour
{
    public RectTransform linesContainer;
    public RectTransform linePrefab;

    private List<ResearchLine> allLines = new List<ResearchLine>();

    public void DrawAllLines()
    {
        allLines.Clear(); // clear old lines

        var nodeUIs = FindObjectsOfType<ResearchButtonUI>();
        foreach (var nodeUI in nodeUIs)
        {
            foreach (var prereqData in nodeUI.research.requiredResearches)
            {
                var prereqUI = System.Array.Find(nodeUIs, x => x.research == prereqData);
                if (prereqUI != null)
                {
                    // Create line only once
                    RectTransform line = Instantiate(linePrefab, linesContainer);
                    PositionLine(line, prereqUI.GetComponent<RectTransform>(), nodeUI.GetComponent<RectTransform>());

                    // Store for later color updates
                    var rLine = new ResearchLine(line, prereqData, nodeUI.research);
                    allLines.Add(rLine);
                }
            }
        }
    }

    private void PositionLine(RectTransform line, RectTransform start, RectTransform end)
    {
        // Convert world positions to LinesContainer local positions
        Vector2 startPos, endPos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            linesContainer, RectTransformUtility.WorldToScreenPoint(null, start.position), null, out startPos);
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            linesContainer, RectTransformUtility.WorldToScreenPoint(null, end.position), null, out endPos);

        Vector2 direction = endPos - startPos;
        float distance = direction.magnitude;

        // Move line to midpoint
        line.localPosition = startPos + direction / 2f;
        // Stretch it
        line.sizeDelta = new Vector2(distance, line.sizeDelta.y);
        // Rotate it
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        line.localRotation = Quaternion.Euler(0, 0, angle);
    }

    public void UpdateLineColors(ResearchManager researchManager)
    {
        foreach (var rLine in allLines)
        {
            bool isUnlocked = researchManager.IsUnlocked(rLine.startResearch);
            rLine.lineImage.color = isUnlocked ? Color.green : Color.gray;
        }
    }
}
