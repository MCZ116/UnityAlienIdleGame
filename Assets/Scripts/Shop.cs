using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop : MonoBehaviour
{

    public IdleScript alienScript;

    public EggBehaviour eggScript;

    private double foodPrice = 2500;

    void Update()
    {
        //foodPrice *= alienScript.LightPower;
    }

    public void FeedingAlien()
    {
        if (alienScript.mainCurrency > foodPrice)
        {
            alienScript.mainCurrency -= foodPrice;
            eggScript.hunger += 50;

            if (eggScript.hunger > 100)
            {
                eggScript.hunger = 100;
            }

        }
    }
}
