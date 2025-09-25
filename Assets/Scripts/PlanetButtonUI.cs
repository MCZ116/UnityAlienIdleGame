using UnityEngine;
using UnityEngine.UI;

public class PlanetButtonUI : MonoBehaviour
{
    public Button button;
    public Text priceText;
    public Image icon;
    public Text requirements;
    public Text description;

    public PlanetData planet;
    public PlanetManager planetManager;

    void Start()
    {
        priceText.text = GameManager.ExponentLetterSystem(planet.GetPrice());
        requirements.text = planet.GetResearchRequirement();
        description.text = planet.description;

        // Assign button listeners
        button.onClick.AddListener(OnClick);
        button.onClick.AddListener(() => GameManager.instance.ChangePlanetTab(planet.planetId));            

        if (icon != null && planet.Icon != null)
            icon.sprite = planet.Icon;
    }

    private void Update()
    {
        bool hasEnoughCurrency = planetManager.HasEnoughCurrency(planet);
        bool requiredResearch= planetManager.RequirementsMet(planet);

        priceText.color = hasEnoughCurrency ? Color.green : Color.red;
        requirements.color = requiredResearch ? Color.green : Color.red;

        if (planetManager.IsUnlocked(planet))
        {
            priceText.gameObject.SetActive(false);
            requirements.gameObject.SetActive(false);
        }
        else
        {
            priceText.gameObject.SetActive(true);
            priceText.text = GameManager.ExponentLetterSystem(planet.GetPrice());
            requirements.gameObject.SetActive(true);
        }
    }

    private void OnClick()
    {
        planetManager.Unlock(planet);
    }
}
