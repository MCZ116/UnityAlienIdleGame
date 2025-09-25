using UnityEngine;
using UnityEngine.UI;

public class ResearchButtonUI : MonoBehaviour
{
    public Button button;
    public Text priceText;
    public Image icon;
    public GameObject priceContainer;
    public ResearchData research;
    private ResearchData lastShownResearch;
    private ResearchManager researchManager;
    private PlanetManager planetManager;
    private InfoWindow infoWindow;
    public Image planetIconImage;
    private GlowEffectUI glowEffect;

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
        glowEffect = button.GetComponent<GlowEffectUI>();
    }

    private void Update()
    {
        RefreshUI();
    }

    private void RefreshUI()
    {
        bool canUnlock = researchManager.CanUnlock(research);
        bool requiredResearchDone = researchManager.RequirementsMet(research);

        // Update price text
        priceContainer.SetActive(requiredResearchDone);
        if (requiredResearchDone)
            priceText.text = GameManager.ExponentLetterSystem(research.GetPrice());

        priceText.color = canUnlock ? Color.green : Color.red;

        // Update button state & glow
        UpdateVisuals();
    }


    private void OnClick()
    {
        researchManager.Unlock(research);
    }

    private void UpdateVisuals()
    {
        bool unlocked = researchManager.IsUnlocked(research);

        button.interactable = !unlocked;

        ColorBlock cb = button.colors;
        cb.disabledColor = cb.normalColor;
        button.colors = cb;

        glowEffect.SetGlow(unlocked);
    }

}
