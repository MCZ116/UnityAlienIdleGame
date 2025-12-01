using UnityEngine;

[CreateAssetMenu(fileName = "WheelConfig", menuName = "ScriptableObjects/Wheel Config")]
public class WheelConfig : ScriptableObject
{
    public WheelSlot[] slots;   // 12 slots (30° each)
}