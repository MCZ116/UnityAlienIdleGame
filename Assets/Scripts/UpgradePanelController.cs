using UnityEngine;

public class UpgradePanelController : MonoBehaviour
{
    public static UpgradePanelController Instance;

    [Header("Assign in Inspector")]
    public RectTransform panel;           // Panel GameObject
    public Canvas canvas;                 // Canvas containing UI
    public UpgradePanelUI panelUI;        // Script on the panel controlling texts/buttons

    private BuildingUI currentBuilding;

    private void Awake()
    {
        if (Instance != null) Destroy(gameObject);
        else Instance = this;

        panel.gameObject.SetActive(false);
    }

    public void TogglePanel(BuildingUI building)
    {
        if (currentBuilding == building)
        {
            HidePanel();
            return;
        }

        currentBuilding = building;
        ShowPanel();
    }

    private void ShowPanel()
    {
        if (panel == null || currentBuilding == null) return;

        panel.gameObject.SetActive(true);
        panelUI.SetBuilding(currentBuilding);
        panel.SetAsLastSibling(); // Always on top
        UpdatePanelPosition();
    }

    public void HidePanel()
    {
        if (panel != null)
            panel.gameObject.SetActive(false);

        currentBuilding = null;
    }

    private void Update()
    {
        if (currentBuilding != null)
            UpdatePanelPosition();
    }

    private void UpdatePanelPosition()
    {
        if (currentBuilding == null || panel == null) return;

        RectTransform buildingRT = currentBuilding.GetComponent<RectTransform>();

        // Convert building position to canvas local position
        Vector2 anchoredPos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvas.transform as RectTransform,
            RectTransformUtility.WorldToScreenPoint(canvas.worldCamera, buildingRT.position),
            canvas.worldCamera,
            out anchoredPos
        );

        // Add offset (pixels above the building)
        anchoredPos.y += 120;
        anchoredPos.x += 180;

        // Optional: Clamp inside canvas bounds
        Vector2 panelSize = panel.sizeDelta;
        RectTransform canvasRT = canvas.transform as RectTransform;

        anchoredPos.x = Mathf.Clamp(anchoredPos.x,
            -canvasRT.sizeDelta.x / 2 + panelSize.x / 2,
            canvasRT.sizeDelta.x / 2 - panelSize.x / 2);

        anchoredPos.y = Mathf.Clamp(anchoredPos.y,
            -canvasRT.sizeDelta.y / 2 + panelSize.y / 2,
            canvasRT.sizeDelta.y / 2 - panelSize.y / 2);

        panel.anchoredPosition = anchoredPos;
    }
}
