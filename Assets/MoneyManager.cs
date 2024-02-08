using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using TMPro;

public class MoneyManager : MonoBehaviour
{

    [SerializeField]
    private TextMeshProUGUI profitAmountUI;



    public void setWeeklyProfit(int weeklyProfit)
    {
        profitAmountUI.text = parseProfitToString(weeklyProfit);
    }

    public int getCurrentWeeklyProfit()
    {
        return parseProfitToNumber(profitAmountUI.text);
    }

    private int parseProfitToNumber(string profit)
    {
        string s = profit.Substring(1);
        s = s.Replace(",", "");
        return int.Parse(s);
    }

    private string parseProfitToString(int profit)
    {
        string s = $"{profit:n0}";
        s = s.Insert(0, "$");
        return s;

    }
}
