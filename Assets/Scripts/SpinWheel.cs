using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SpinWheel : MonoBehaviour
{
    [Header("Spin Wheel Settings")]
    public bool spinStarted = false;
    public bool activeSpinTab = false;
    public GameObject wheel;
    public GameObject spinWheelMenu;
    public GameObject spinArea;

    public Text[] wheelSlotTexts;

    public GameManager gameManager;

    public WheelConfig config;
    public float spinStartAngle = 0f;
    public float spinEndAngle;

    public float currentLerpRotation;
    public float maxLerpRotationTime;

    [Header("Reward Popup")]
    public RewardPopup rewardPopup;

    private void Start()
    {
        UpdateWheelTexts();
    }

    private void Update()
    {
        UpdateWheelTexts();
        HideIfClickedOutside(spinArea);
        if (!spinStarted)
            return;

        maxLerpRotationTime = 4f;
        currentLerpRotation += Time.deltaTime;

        if (currentLerpRotation > maxLerpRotationTime || wheel.transform.eulerAngles.z == spinEndAngle)
        {
            currentLerpRotation = maxLerpRotationTime;
            spinStarted = false;
            spinStartAngle = spinEndAngle % 360f;
            if (spinStartAngle < 0f) spinStartAngle += 360f;

            GiveRewardFromConfig();
        }

        float spinTime = currentLerpRotation / maxLerpRotationTime;
        spinTime = spinTime * spinTime * spinTime * (spinTime * (6f * spinTime - 15f) + 10f);

        float angle = Mathf.Lerp(spinStartAngle, spinEndAngle, spinTime);
        wheel.transform.eulerAngles = new Vector3(0, 0, angle);
    }

    public void UpdateWheelTexts()
    {
        for (int i = 0; i < config.slots.Length; i++)
        {
            var slot = config.slots[i];

            double rewardAmount = GetFinalRewardAmount(slot);
            wheelSlotTexts[i].text = GameManager.ExponentLetterSystem(rewardAmount,"F0");
        }
    }

    public void SpinWheelButton()
    {
        if (spinStarted) return;

        currentLerpRotation = 0f;

        // Pick a random slot from your config
        int slotIndex = Random.Range(0, config.slots.Length);
        WheelSlot selectedSlot = config.slots[slotIndex];

        int fullSpins = 5;
        spinEndAngle = -(fullSpins * 360f + selectedSlot.centerAngle); // negative for clockwise spin

        spinStarted = true;
    }

    private void GiveRewardFromConfig()
    {
        float currentAngle = spinStartAngle % 360f;
        if (currentAngle < 0) currentAngle += 360f;

        WheelSlot rewardSlot = FindSlotByAngle(currentAngle);

        double rewardAmount = GetFinalRewardAmount(rewardSlot);

        bool isCrystal = rewardSlot.isCrystal;
        rewardPopup.Show(rewardAmount, isCrystal);
    }

    public double GetFinalRewardAmount(WheelSlot slot)
    {
        double ips = gameManager.GetTotalIncomePerSecond();
        double reward = slot.isCrystal ? slot.baseAmount : ips * slot.baseAmount;
        return GameManager.AggressiveRound(reward);
    }

    private WheelSlot FindSlotByAngle(float angle)
    {
        foreach (var slot in config.slots)
        {
            float half = slot.angleSize / 2f;
            float min = NormalizeAngle(slot.centerAngle - half);
            float max = NormalizeAngle(slot.centerAngle + half);

            if (AngleInRange(angle, min, max))
                return slot;
        }

        return config.slots[0]; // fallback
    }

    private bool AngleInRange(float angle, float min, float max)
    {
        if (min < max)
            return angle >= min && angle <= max;
        else
            return angle >= min || angle <= max;
    }

    private float NormalizeAngle(float a)
    {
        a %= 360f;
        if (a < 0f) a += 360f;
        return a;
    }

    public void SpinWheelMenu()
    {
        if (!activeSpinTab)
        {
            spinWheelMenu.SetActive(true);
            activeSpinTab = true;
        }
    }

    private void HideIfClickedOutside(GameObject panel)
    {
        if (rewardPopup.gameObject.activeSelf)
            return;

        if (Input.GetMouseButton(0) && panel.activeSelf &&
            !RectTransformUtility.RectangleContainsScreenPoint(
                panel.GetComponent<RectTransform>(),
                Input.mousePosition,
                Camera.main))
        {
            spinWheelMenu.SetActive(false);
            activeSpinTab = false;
        }
    }
}
