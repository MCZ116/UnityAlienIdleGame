using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AnimationUnlockSystem : MonoBehaviour
{
    public UnlockingSystem unlockingSystem;

    public GameManager gameManager;

    public List<Animator> gateOpeningAnimation;

    private void Awake()
    {
        for (int id = 0; id < unlockingSystem.upgradeObjects.Length; id++)
        {
            gateOpeningAnimation[id] = unlockingSystem.gateUnlockButtonObject[id].transform.Find("unlockGate").GetComponent<Animator>();
        }
    }

    void Update()
    {
        for (int id = 0; id < unlockingSystem.upgradeObjects.Length; id++)
        {
            AnimationUnlock(id);
        }

    }

    public void AnimationUnlock(int id)
    {

            if (gameManager.upgradesActivated[id] == true)
            {
                gateOpeningAnimation[id].SetBool("unlockAnimation", true);
            }
            else if (gameManager.upgradesActivated[id] == false)
            {
                gateOpeningAnimation[id].SetBool("unlockAnimation", false);
            }

    }

}
