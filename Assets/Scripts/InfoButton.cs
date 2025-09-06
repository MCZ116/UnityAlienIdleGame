using UnityEngine;
using UnityEngine.UI;

public class InfoButton : MonoBehaviour
{
    public InfoWindow infoWindow;
    private IDescribable target;
    public Button button;

    public void Start()
    {
        button.onClick.AddListener(ShowHideInfo);
    }

    public void SetTarget(IDescribable targetData)
    {
        target = targetData;
    }

    private void ShowHideInfo()
    {
        if (target == null) return;

        if (infoWindow.IsVisible && infoWindow.IsShowing(target))
        {
            // Same target is shown — hide it
            infoWindow.Hide();
        }
        else
        {
            // Show new or different target
            infoWindow.Show(target);
        }
    }
}
