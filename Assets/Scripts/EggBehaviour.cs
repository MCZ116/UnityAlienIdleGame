using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EggBehaviour : MonoBehaviour
{
    [SerializeField]
    private IdleScript alienScript;

    private SpriteRenderer render;
    private Sprite alien1egg, alien1phase3;
    [SerializeField]
    private int alien1;

    [SerializeField]
    public double hunger;

    [SerializeField]
    private double health;

    [SerializeField]
    private double changHungerPerSec;

    private Animator animationIdle;

    public Text Hunger;

    void Start()
    {   
        animationIdle = GetComponent<Animator>();
    }

    void Update()
    {
        Hunger.text = "Hunger: " + hunger;

        SurvivalStats();
        AnimationIdle();
    }

    public void SurvivalStats()
    {
        if(hunger != 0)
        {
            //alienScript.upgradeLevel1 *= Time.deltaTime;

        }
        if(changHungerPerSec>0)
        {
           changHungerPerSec-=Time.deltaTime;

            if (changHungerPerSec <= 0)
            {
                hunger--;
                changHungerPerSec = 50;
            }
           
        }
    }

    public void AnimationIdle()
    {

        if (alienScript.AlienLevel < alien1)
        {
            animationIdle.SetBool("idleAlien", false);
        }
        else if (alienScript.AlienLevel >= alien1)
        {
            animationIdle.SetBool("idleAlien", true);
        }

    }

}
