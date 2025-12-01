using UnityEngine;

[CreateAssetMenu(fileName = "WheelSlot", menuName = "ScriptableObjects/Wheel Slot")]
public class WheelSlot : ScriptableObject
{
    public float centerAngle;
    public float angleSize = 30f;
    public bool isCrystal;
    public double baseAmount;
}