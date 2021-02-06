using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnlockingAnimations : MonoBehaviour
{
    public UnlockingSystem unlockingSystem;

    public IdleScript idleScript;

    private Animator unlockAnimation;

    private void Start()
    {
        unlockAnimation = GetComponent<Animator>();
    }


    void Update()
    {
        AnimationUnlock();
    }

    public void AnimationUnlock()
    {
        if (unlockingSystem.animationUnlockConfirm == false)
        {
            unlockAnimation.SetBool("unlockAnimation", false);
        }
        else if (unlockingSystem.animationUnlockConfirm == true)
        {
            unlockAnimation.SetBool("unlockAnimation", true);
        }

    }

    public void DisableUpgradeButtons(int id)
    {

        unlockingSystem.unlockButtons[id].SetActive(false);

    }

}
