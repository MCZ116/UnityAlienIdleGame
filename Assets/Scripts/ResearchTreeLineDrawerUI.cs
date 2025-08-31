using UnityEngine;
using UnityEngine.UI;

public class ResearchTreeLineDrawerUI : MonoBehaviour
{
    public RectTransform linesContainer;
    public RectTransform linePrefab;

    public void DrawAllLines()
    {
        var nodeUIs = FindObjectsOfType<ResearchButtonUI>();
        foreach (var nodeUI in nodeUIs)
        {
            foreach (var prereqData in nodeUI.research.requiredResearches)
            {
                var prereqUI = System.Array.Find(nodeUIs, x => x.research == prereqData);
                if (prereqUI != null)
                {
                    DrawUILine(prereqUI.GetComponent<RectTransform>(), nodeUI.GetComponent<RectTransform>());
                }
            }
        }
    }

    private void DrawUILine(RectTransform start, RectTransform end)
    {
        RectTransform line = Instantiate(linePrefab, linesContainer);

        // Convert world positions of buttons to LinesCanvas local positions
        Vector2 startPos, endPos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            linesContainer, RectTransformUtility.WorldToScreenPoint(null, start.position), null, out startPos);
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            linesContainer, RectTransformUtility.WorldToScreenPoint(null, end.position), null, out endPos);

        Vector2 direction = endPos - startPos;
        float distance = direction.magnitude;

        line.localPosition = startPos + direction / 2f;
        line.sizeDelta = new Vector2(distance, line.sizeDelta.y);

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        line.localRotation = Quaternion.Euler(0, 0, angle);
    }

}
