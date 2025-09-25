using UnityEngine;
using UnityEngine.UI;

public class GlowEffectUI : MonoBehaviour
{
    [SerializeField] private Image glowImage;
    [SerializeField] private float pulseSpeed = 2f;
    [SerializeField] private float minAlpha = 0.3f;
    [SerializeField] private float maxAlpha = 0.7f;

    private bool isActive;

    void Update()
    {
        if (!isActive || glowImage == null) return;

        // Pulse alpha between min and max
        float alpha = Mathf.Lerp(minAlpha, maxAlpha, (Mathf.Sin(Time.time * pulseSpeed) + 1f) / 2f);
        Color c = glowImage.color;
        c.a = alpha;
        glowImage.color = c;
    }

    public void SetGlow(bool active)
    {
        isActive = active;
        if (glowImage != null)
            glowImage.enabled = active;
    }
}
