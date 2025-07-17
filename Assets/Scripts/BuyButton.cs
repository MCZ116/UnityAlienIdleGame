using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class BuyButton : MonoBehaviour
{
    public Image icon;
    public Text label;
    public Button button;

    public void Init(int itemId, string labelText, Sprite iconSprite, Action<int> onClick)
    {
        label.text = labelText;
        icon.sprite = iconSprite;

        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(() => onClick?.Invoke(itemId));
    }

    public void UpdateLabel(string newLabel)
    {
        label.text = newLabel;
    }

    public void Enable()
    {
        button.interactable = true;
    }

    public void Disable()
    {
        button.interactable = false;
    }

    public void UpdateTextColor(Color color)
    {
        label.color = color;
    }
}
