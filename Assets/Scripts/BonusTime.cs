using System;
using UnityEngine;
using UnityEngine.UI;

public class BonusTime : MonoBehaviour
{
    private const int BUFF_DURATION_MINUTES = 120;
    private const int MAX_DURATION_MINUTES = 480;
    private const float maxBuffSeconds = 8f * 60f * 60f;
    private DateTime buffEndTime;

    public bool IsBuffActive => DateTime.UtcNow < buffEndTime;

    [SerializeField] GameObject boostMenu;
    [SerializeField] GameObject boostArea;
    public Text bonusTimeText;
    public Image progressBar;

    bool activeTab = false;

    void Start()
    {
        LoadBuff();
        Debug.Log("Loaded buff");
    }

    void Update()
    {
        HideIfClickedOutside(boostArea);
        UpdateBuffTimeText();
    }

    private void UpdateBuffTimeText()
    {
        DateTime now = DateTime.UtcNow;
        TimeSpan remaining = buffEndTime - now;

        if (IsBuffActive && remaining.TotalSeconds > 0)
        {
            bonusTimeText.text = $"{remaining:hh\\:mm\\:ss}";
        }
        else
        {
            bonusTimeText.text = "00:00:00";
        }

        UpdateProgressBar((float)remaining.TotalSeconds);
    }

    private void UpdateProgressBar(float remainingSeconds)
    {
        float clampedSeconds = Mathf.Clamp(remainingSeconds, 0f, maxBuffSeconds);
        float fillAmount = clampedSeconds / maxBuffSeconds;

        progressBar.fillAmount = fillAmount;
    }

    public void ClickedBonusBtn()
    {
        ExtendBuff();
    }

    public void ExtendBuff()
    {
        DateTime now = DateTime.UtcNow;

        DateTime newBuffEnd = buffEndTime > now
            ? buffEndTime.AddMinutes(BUFF_DURATION_MINUTES)
            : now.AddMinutes(BUFF_DURATION_MINUTES);

        TimeSpan totalBuff = newBuffEnd - now;
        if (totalBuff.TotalMinutes > MAX_DURATION_MINUTES)
        {
            newBuffEnd = now.AddMinutes(MAX_DURATION_MINUTES);
        }

        buffEndTime = newBuffEnd;
        Debug.Log(buffEndTime);
        SaveBuff();
    }

    public float GetIncomeMultiplier()
    {
        return IsBuffActive ? 2f : 1f;
    }

    public void LoadBuff()
    {
        if (PlayerPrefs.HasKey("BuffEnd"))
        {
            long binary = Convert.ToInt64(PlayerPrefs.GetString("BuffEnd"));
            buffEndTime = DateTime.FromBinary(binary);
        }
        else
        {
            buffEndTime = DateTime.UtcNow;
        }
    }

    public void SaveBuff()
    {
        PlayerPrefs.SetString("BuffEnd", buffEndTime.ToBinary().ToString());
        PlayerPrefs.Save();
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
        if (Input.GetMouseButton(0) && panel.activeSelf &&
            !RectTransformUtility.RectangleContainsScreenPoint(panel.GetComponent<RectTransform>(),
            Input.mousePosition, Camera.main))
        {
            boostMenu.SetActive(false);
            activeTab = false;
        }
    }

}
