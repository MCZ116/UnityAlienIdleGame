using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AstronautBehaviour : MonoBehaviour
{
    [SerializeField]
    private GameManager alienScript;

    [SerializeField]
    private int alien1;

    private Animator animationIdle;


    void Start()
    {   
        animationIdle = GetComponent<Animator>();
    }

    void Update()
    {
        
        AnimationIdle();
    }

    // Change script here cuz its activating only when first creature evolved... or maybe it should be like this ?
    public void AnimationIdle()
    {

        if (alienScript.AlienLevel[0] < alien1)
        {
            animationIdle.SetBool("idleAlien", false);
        }
        else if (alienScript.AlienLevel[0] >= alien1)
        {
            animationIdle.SetBool("idleAlien", true);
        }

    }

}
