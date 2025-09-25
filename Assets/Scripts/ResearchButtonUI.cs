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
    private PlanetManager planetManager;
    private InfoWindow infoWindow;
    public Image planetIconImage;

    public void Initialize(ResearchData data, ResearchManager manager, PlanetManager planetManager, InfoWindow info)
    {
        research = data;
        researchManager = manager;
        infoWindow = info;
        this.planetManager = planetManager;

        button.onClick.AddListener(OnClick);
        button.onClick.AddListener(AudioControler.instance.ButtonClickSound);

        if (icon != null && research.icon != null)
            icon.sprite = research.icon;

        var planet = planetManager.GetPlanetUnlockedByResearch(research);

        if (planetIconImage != null)
        {
            if (planet != null && planet.Icon != null)
            {
                planetIconImage.sprite = planet.Icon;
                planetIconImage.gameObject.SetActive(true);
            }
            else
            {
                planetIconImage.gameObject.SetActive(false);
            }
        }

    }

    private void Update()
    {
        bool canUnlock = researchManager.CanUnlock(research);
        bool requiredResearchDone = researchManager.RequirementsMet(research);
        button.interactable = canUnlock;

        priceText.color = canUnlock ? Color.green : Color.red;
        priceText.text = requiredResearchDone ? GameManager.ExponentLetterSystem(research.GetPrice()) : "";
        SetActiveState();
    }

    private void OnClick()
    {
        researchManager.Unlock(research);
    }

    public void SetActiveState()
    {
        if (researchManager.IsUnlocked(research))
        {
            priceText.text = "Active" ;
            priceText.color = Color.green;
        }
    }
}
