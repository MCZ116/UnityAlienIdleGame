using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpinWheel : MonoBehaviour
{
    public bool spinStarted = false;
    public bool activeSpinTab = false;
    public GameObject wheel;
    public GameObject spinWheelMenu;
    public GameObject spinArea;
    public float[] sectorAngles;
    public float spinStartAngle = 0;
    public float spinEndAngle;
    public Text awardDisplay;
    public Text[] researchPointsOnSpin;
    public double[] rewardPoints;

    //changes
    public GameManager gameManager;

    public float currentLerpRotation;
    public float maxLerpRotationTime;

    private void Start()
    {
        rewardPoints = new double[6];
        for(int i = 0; i < researchPointsOnSpin.Length; i++)
        {
            rewardPoints[i] = (5000 * (i+1)) * gameManager.resetLevel;
            researchPointsOnSpin[i].text = GameManager.ExponentLetterSystem(rewardPoints[i], "F0");
        }
    }

    void Update()
    {

        HideIfClickedOutside(spinArea);
        if (!spinStarted)
            return;

        maxLerpRotationTime = 4f;
        currentLerpRotation += Time.deltaTime;
        if (currentLerpRotation > maxLerpRotationTime || wheel.transform.eulerAngles.z == spinEndAngle)
        {
            currentLerpRotation = maxLerpRotationTime;
            spinStarted = false;
            spinStartAngle = spinEndAngle % 360;


            GiveAwardByAngle();
            StartCoroutine("HideAwardDisplay");
        }

        float spinTime = currentLerpRotation / maxLerpRotationTime;

        spinTime = spinTime * spinTime * spinTime * (spinTime * (6f * spinTime - 15f) + 10f);

        float angle = Mathf.Lerp(spinStartAngle, spinEndAngle, spinTime);
        wheel.transform.eulerAngles = new Vector3(0, 0, angle);

    }

    public void SpinWheelButton(){

        currentLerpRotation = 0f;

        sectorAngles = new float[] { 30, 60, 90, 120, 150, 180, 210, 240, 270, 300, 330, 360 };

        int fullspins = 5;

        float randomAngle = sectorAngles[Random.Range(0,sectorAngles.Length)];
        spinEndAngle = -(fullspins * 360 + randomAngle);
        spinStarted = true;
    }

    private void GiveAwardByAngle()
    {
        switch ((int)spinStartAngle)
        {
            case 0:
                Rewards(300, "crystal");
                break;
            case -330:
                Rewards(rewardPoints[5], "points");
                break;
            case -300:
                Rewards(150, "crystal");
                break;
            case -270:
                Rewards(rewardPoints[4], "points");
                break;
            case -240:
                Rewards(100, "crystal");
                break;
            case -210:
                Rewards(rewardPoints[3], "points");
                break;
            case -180:
                Rewards(50, "crystal");
                break;
            case -150:
                Rewards(rewardPoints[2], "points");
                break;
            case -120:
                Rewards(25, "crystal");
                break;
            case -90:
                Rewards(rewardPoints[1], "points");
                break;
            case -60:
                Rewards(10, "crystal");
                break;
            case -30:
                Rewards(rewardPoints[0], "points");
                break;
        }
    }

    private void Rewards(double award, string currency)
    {
        switch (currency)
        {
            case "crystal":
                gameManager.crystalCurrency += award;
                awardDisplay.text = award.ToString("F0");

                break;

            case "points":
                gameManager.mainCurrency += award;
                awardDisplay.text = GameManager.ExponentLetterSystem(award, "F0");

                break;
        }
        awardDisplay.gameObject.SetActive(true);
    }

    private IEnumerator HideAwardDisplay()
    {
        yield return new WaitForSeconds(1f);
        awardDisplay.gameObject.SetActive(false);
    }

    public void SpinWheelMenu()
    {
        if (!activeSpinTab) {
            spinWheelMenu.gameObject.SetActive(true);
            activeSpinTab = true;
        }
    }

    private void HideIfClickedOutside(GameObject panel)
    {
        if (Input.GetMouseButton(0) && panel.activeSelf && !RectTransformUtility.RectangleContainsScreenPoint(panel.GetComponent<RectTransform>(),
            Input.mousePosition, Camera.main))
        {
            spinWheelMenu.SetActive(false);
            activeSpinTab = false;
        }
    }

}
