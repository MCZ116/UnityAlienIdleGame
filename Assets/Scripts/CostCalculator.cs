using UnityEngine;

public abstract class CostCalculator : MonoBehaviour
{

    public double CostCalc(int id, double costs, double level, double mainCurrency)
    {
        var h = costs;
        var c = mainCurrency;
        var r = 1.07;
        var u = level;
        double n = 1;
        var calculatedCost = h * (System.Math.Pow(r, u) * (System.Math.Pow(r, n) - 1) / (r - 1));

        return calculatedCost;
    }

}
