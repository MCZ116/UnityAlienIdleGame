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
        
        //render = GetComponent<SpriteRenderer>();
        //alien1egg = Resources.Load<Sprite>("alien1egg");
        //alien1phase3= Resources.Load<Sprite>("alien1phase3");
        //render.sprite = alien1egg;
        animationIdle = GetComponent<Animator>();
    }


    void Update()
    {
        Hunger.text = "Hunger: " + hunger;

        //ChangingSprite();
        SurvivalStats();
        AnimationIdle();
    }

    //public void ChangingSprite()
    //{
    //    if (alienScript.LightPower != 0)
    //    {
    //        if (alienScript.LightPower < alien1)
    //        {
    //            render.sprite = alien1egg;


    //        }
    //        else if (alienScript.LightPower >= alien1)
    //            render.sprite = alien1phase3;
    //    }
    //}

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
