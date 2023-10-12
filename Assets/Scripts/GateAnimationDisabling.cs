using UnityEngine;

public class GateAnimationDisabling : MonoBehaviour
{
    public AnimationUnlockSystem animationUnlockSys;

    private void Start()
    {
        animationUnlockSys = GameObject.Find("GateAnimationManager").GetComponent<AnimationUnlockSystem>();
    }

    public void DisableUpgradeButtons()
    {
        for (int id = 0; id < animationUnlockSys.unlockingSystem.gateUnlockButtonObject.Length; id++)
        {
            if (animationUnlockSys.gameManager.upgradesActivated[id] == true)
            {
                animationUnlockSys.unlockingSystem.gateUnlockButtonObject[id].SetActive(false);
            }
        }

    }

}
