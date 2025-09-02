using UnityEngine;
using UnityEngine.UI;

public class ResearchLine
{
    public RectTransform lineTransform;
    public ResearchData startResearch;
    public ResearchData endResearch;
    public Image lineImage;

    public ResearchLine(RectTransform lineTransform, ResearchData start, ResearchData end)
    {
        this.lineTransform = lineTransform;
        this.startResearch = start;
        this.endResearch = end;
        this.lineImage = lineTransform.GetComponent<Image>();
    }
}
