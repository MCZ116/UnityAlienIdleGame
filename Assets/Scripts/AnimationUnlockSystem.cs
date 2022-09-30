using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AnimationUnlockSystem : MonoBehaviour
{
    public UnlockingSystem unlockingSystem;

    public GameManager gameManager;

    public List<Animator> gateOpeningAnimation;

    void Update()
    {
        Debug.Log("Unlocking objects Update" + unlockingSystem.upgradeObjects.Length);
        Debug.Log("Unlocking objects" + unlockingSystem.upgradeObjects.Length);
        Debug.Log("UnlockText " + unlockingSystem.unlockText.Length);
        for (int id = 0; id < unlockingSystem.upgradeObjects.Length; id++)
        {
            AnimationUnlock(id);
        }

    }

    public void AnimationUnlock(int id)
    {

            if (unlockingSystem.animationUnlockConfirm[id] == true)
            {
                gateOpeningAnimation[id].SetBool("unlockAnimation", true);
            }
            else if (unlockingSystem.animationUnlockConfirm[id] == false)
            {
                gateOpeningAnimation[id].SetBool("unlockAnimation", false);
            }

    }

}
