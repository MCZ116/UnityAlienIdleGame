using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonusTime : MonoBehaviour
{
    double timeLeft;
    double timeAdded = 7200.0f;
    [SerializeField] GameObject boostMenu;
    [SerializeField] GameObject boostArea;
    //[SerializeField] GameObject boostMenuArea;
    bool activeTab = false;
    [SerializeField] GameManager gameManager;

    void Update()
    {
        if (timeLeft != 0)
        {
            timeLeft -= Time.deltaTime;
            BonusAdded();
        }

        HideIfClickedOutside(boostArea);
    }

    public void ClickedBonusBtn()
    {
        timeLeft += timeAdded;
    }

    public void BonusAdded()
    {
        if (timeLeft > 0)
        {
            gameManager.mainCurrency += gameManager.TotalIncome();
        }
    }

    public void BoostMenu()
    {
        if (!activeTab)
        {
            boostMenu.gameObject.SetActive(true);
            activeTab = true;
        }
    }

    private void HideIfClickedOutside(GameObject panel)
    {
        if (Input.GetMouseButton(0) && panel.activeSelf && !RectTransformUtility.RectangleContainsScreenPoint(panel.GetComponent<RectTransform>(),
            Input.mousePosition, Camera.main))
        {
            boostMenu.SetActive(false);
            activeTab = false;
        }
    }

}
