using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SaveData
{
    private static int currentMoney;

    public static event Action<int> OnWealthIncreased;
    public static event Action<int> OnWealthDecreased;
    public static event Action<int> OnHpIncreased;
    public static event Action<int> OnHpDecreased;

    public static void IncreaseWealth(int Amount)
    {
        currentMoney = PlayerPrefs.GetInt("Wealth");
        currentMoney += Amount ;
        PlayerPrefs.SetInt("Wealth", currentMoney);
        OnWealthIncreased?.Invoke(Amount);
    }
    /// <summary>
    /// decreases money to value (must be posivite)
    /// </summary>
    /// <param name="Amount"></param>
    public static void DecreaseWealth(int Amount)
    {
        currentMoney = PlayerPrefs.GetInt("Wealth");
        currentMoney -= Amount;
        PlayerPrefs.SetInt("Wealth", currentMoney);
        OnWealthDecreased?.Invoke(Amount);
    }

    public static int playerHP;
    public static void IncreasePlayerHP(int Amount)
    {
        playerHP = PlayerPrefs.GetInt("HP");
        playerHP += Amount;
        PlayerPrefs.SetInt("HP", playerHP);
        OnHpIncreased?.Invoke(Amount);
    }
    public static void DecreasePlayerHP(int Amount)
    {
        playerHP = PlayerPrefs.GetInt("HP");
        playerHP -= Amount;
        PlayerPrefs.SetInt("HP", playerHP);
        OnHpDecreased?.Invoke(Amount);
    }

}
