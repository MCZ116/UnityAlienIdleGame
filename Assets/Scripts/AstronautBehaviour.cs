using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AstronautBehaviour : MonoBehaviour
{
    [SerializeField]
    private GameManager alienScript;

    public GameObject[] upgradeAstronauts;

    private Animator animationIdle;

    private double[] astronautCost = { 50, 50, 50, 50 };

    public Text[] AstronautCostText;

    private int[] astronauts;

    void Start()
    {   
    
    }

    void Update()
    {
        for (int id = 0; id < AstronautCostText.Length; id++)
        {
            AstronautCostText[id].text = astronautCost[id].ToString("F0");
        }

    }

    public void AstronautsAppearing(int id)
    {
        var h = astronautCost[id];
        var c = alienScript.crystalCurrency;
        var r = 2;
        var u = id;
        double n = 1;
        
        if (astronauts[id] <= 3)
        {
            var astronautTempCost = h * (System.Math.Pow(r, u) * (System.Math.Pow(r, n) - 1) / (r - 1));
            if (alienScript.crystalCurrency >= astronautTempCost)
            {
                alienScript.crystalCurrency -= astronautTempCost;
                upgradeAstronauts[astronauts[id]].SetActive(true);
                astronauts[id]++;
                //AstronautsBoost();
            }
        } 
    }

    //public double AstronautsBoost()
    //{
    //    double asBoost = 0;

    //    for (int id = 0; id < upgradeAstronauts.Length; id++)
    //    {
    //        asBoost += 0.05 * astronauts[id] * 0.15;
    //    }

    //    return asBoost;
    //}

}
