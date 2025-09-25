using UnityEngine;
using UnityEngine.UI;

public class InfoWindow : MonoBehaviour
{
    public Text description;
    public Image icon;
    public GameObject infoWindow;

    private IDescribable currentTarget;

    public bool IsVisible => infoWindow.activeSelf;

    public bool IsShowing(IDescribable target)
    {
        return currentTarget == target;
    }

    public void Show(IDescribable target)
    {
        currentTarget = target;
        description.text = target.Description;
        icon.sprite = target.Icon;
        infoWindow.SetActive(true);
    }

    public void Hide()
    {
        infoWindow.SetActive(false);
        currentTarget = null;
    }
}
