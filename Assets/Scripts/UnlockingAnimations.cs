using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UnlockingAnimations : MonoBehaviour
{
    public UnlockingSystem unlockingSystem;

    public GameManager gameManager;

    //private Animator[] unlockAnimation;
    public List<Animator> unlockAnimation;
    public GameObject[] animationUnlockObject;

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
    // Need REMAKE
    //public void AnimationGateAssigning()
    //{
    //    animationUnlockObject = new GameObject[unlockingSystem.animationUnlockConfirm.Length];
    //    unlockAnimation = new List<Animator>();
    //    for (int id = 0; id < animationUnlockObject.Length; id++)
    //    {
    //        animationUnlockObject = GameObject.FindGameObjectsWithTag("unlockAnimation");
    //        unlockAnimation.Add(animationUnlockObject[id].GetComponent<Animator>());
    //        Debug.Log($"The index value of \"i\" is {id}");
    //        Debug.Log($"The collection \"ListaNaszychPrzedmiotow\" is {unlockAnimation.Count()}");
    //    }
    //}

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

}
