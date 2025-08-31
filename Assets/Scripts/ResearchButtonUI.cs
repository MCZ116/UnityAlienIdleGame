using UnityEngine;
using UnityEngine.UI;

public class ResearchButtonUI : MonoBehaviour
{
    public Button button;
    public Text priceText;
    public Image icon;

    public ResearchData research;
    private ResearchData lastShownResearch;
    private ResearchManager researchManager;
    private InfoWindow infoWindow; 

    public void Initialize(ResearchData data, ResearchManager manager, InfoWindow info)
    {
        research = data;
        researchManager = manager;
        infoWindow = info;
        priceText.text = GameManager.ExponentLetterSystem(research.GetPrice(), "F2");

        button.onClick.AddListener(OnClick);

        if (icon != null && research.icon != null)
            icon.sprite = research.icon;
    }

    private void Update()
    {
        bool canUnlock = researchManager.CanUnlock(research);
        button.interactable = canUnlock;

        priceText.color = canUnlock ? Color.green : Color.red;
    }

    private void OnClick()
    {
        researchManager.Unlock(research);
    }
}
