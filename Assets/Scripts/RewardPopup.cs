using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RewardPopup : MonoBehaviour
{
    public Image icon;
    public TMP_Text amountText;

    [Header("Icons")]
    public Sprite coinsSprite;
    public Sprite crystalsSprite;

    [Header("Timing")]
    public float displayTime = 2f;
    public float fadeDuration = 0.5f;

    private CanvasGroup canvasGroup;

    private void Awake()
    {
        canvasGroup = gameObject.AddComponent<CanvasGroup>();
        canvasGroup.alpha = 0f;
    }

    public void Show(double amount, string type)
    {
        // Reset state in case it's currently fading
        StopAllCoroutines();
        canvasGroup.alpha = 1f;
        gameObject.SetActive(true);

        amountText.text = "You won " + GameManager.ExponentLetterSystem(amount,"F0");
        bool isCrystal = (type == "crystal");
        icon.sprite = isCrystal ? crystalsSprite : coinsSprite;

        // Auto-fade after displayTime
        StartCoroutine(FadeAndHide());
    }

    private System.Collections.IEnumerator FadeAndHide()
    {
        yield return new WaitForSeconds(displayTime);

        float elapsed = 0f;
        float startAlpha = canvasGroup.alpha;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(startAlpha, 0f, elapsed / fadeDuration);
            yield return null;
        }

        gameObject.SetActive(false);
    }
}
