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
    }

    public void HidePanel()
    {
        if (panel != null)
            panel.gameObject.SetActive(false);

        currentBuilding = null;
    }
}
