using UnityEngine;

public class GateAnimationDisabling : MonoBehaviour
{
    public UnlockingSystem unlocking;
    public UnlockingAnimations unlockingAnimations;

    public void DisableUpgradeButtons()
    {
        for (int id = 0; id < unlockingAnimations.animationUnlockObject.Length; id++)
        {
            if (unlockingAnimations.unlockingSystem.animationUnlockConfirm[id] == true)
            {
                unlockingAnimations.unlockingSystem.unlockButtons[id].SetActive(false);
            }
        }

    }

}
