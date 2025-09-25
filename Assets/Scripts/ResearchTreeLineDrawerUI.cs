using NUnit.Framework.Interfaces;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;

public class ResearchTreeLineDrawerUI : MonoBehaviour
{
    public RectTransform linesContainer;
    public RectTransform linePrefab;

    private List<ResearchLine> allLines = new List<ResearchLine>();
    [SerializeField] private Transform researchPanel;
    private List<ResearchButtonUI> nodeUIs;

    public void DrawAllLines()
    {
        nodeUIs = researchPanel.GetComponentsInChildren<ResearchButtonUI>(true).ToList();

        allLines.Clear();
        foreach (var nodeUI in nodeUIs)
        {
            foreach (var prereqData in nodeUI.research.requiredResearches)
            {
                var prereqUI = nodeUIs.Find(x => x.research == prereqData);
                if (prereqUI != null)
                {
                    RectTransform line = Instantiate(linePrefab, linesContainer);
                    PositionLine(line, prereqUI.GetComponent<RectTransform>(), nodeUI.GetComponent<RectTransform>());
                    allLines.Add(new ResearchLine(line, prereqData, nodeUI.research));
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
