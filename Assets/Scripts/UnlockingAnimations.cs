using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UnlockingAnimations : MonoBehaviour
{
    public UnlockingSystem unlockingSystem;

    public GameManager idleScript;

    //private Animator[] unlockAnimation;
    private List<Animator> unlockAnimation;
    private GameObject[] animationUnlockObject;

    private void Awake()
    {
        AnimationGateAssigning();
    }

    void Update()
    {
        //Debug.Log("Unloking objects Update" + unlockingSystem.upgradeObjects.Length);
        //Debug.Log("Unloking objects" + unlockingSystem.upgradeObjects.Length);
        //Debug.Log("Animation Object " + animationUnlockObject.Length);
        //Debug.Log("UnlockText " + unlockingSystem.unlockText.Length);
        for (int id = 0; id < unlockingSystem.upgradeObjects.Length; id++)
        {
            AnimationUnlock(id);
        }


    }

    public void AnimationGateAssigning()
    {
        animationUnlockObject = new GameObject[unlockingSystem.upgradeObjects.Length];
        unlockAnimation = new List<Animator>();
        for (int id = 0; id < animationUnlockObject.Length; id++)
        {
            animationUnlockObject = GameObject.FindGameObjectsWithTag("unlockAnimation");
            unlockAnimation.Add(animationUnlockObject[id].GetComponent<Animator>());
            Debug.Log($"The index value of \"i\" is {id}");
            Debug.Log($"The collection \"ListaNaszychPrzedmiotow\" is {unlockAnimation.Count()}");
        }
    }

    public void AnimationUnlock(int id)
    {

            if (unlockingSystem.animationUnlockConfirm[id] == true)
            {
                unlockAnimation[id].SetBool("unlockAnimation", true);
            }
            else if (unlockingSystem.animationUnlockConfirm[id] == false)
            {
                unlockAnimation[id].SetBool("unlockAnimation", false);
            }

    }

    public void DisableUpgradeButtons()
    {
        for (int id = 0; id < animationUnlockObject.Length; id++)
        {
            if (unlockingSystem.animationUnlockConfirm[id] == true) {
                unlockingSystem.unlockButtons[id].SetActive(false);
            }
        }
        
    }

}
