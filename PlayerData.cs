using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerData : MonoBehaviour
{
    public TMP_Text moneyTM;
    public int currentCheatMoney;

    public TMP_Text hpTM;
    public int hp = 100;

    public TMP_Text waveTM;

    [SerializeField]
    private NextWaveTimer nextWaveTimer;
    public TMP_Text timeTM;
    private bool firstPressOver = false;

    private void Start()
    {
        timeTM.text = "Press to trigger first wave";
    }

    private void Update()
    {
        Money();
        HP();
        WaveTimer();
    }

    private void WaveTimer()
    {
        waveTM.text = $"{PlayerPrefs.GetInt("CurrentWave")} / {PlayerPrefs.GetInt("TotalWaves")}";

        if (firstPressOver)
        {
            if (nextWaveTimer != null)
                DisplayTime(nextWaveTimer.WaveSpawnTime);
        }
    }

    private void HP()
    {
        if (PlayerPrefs.GetInt("HP") < 0)
        {
            PlayerPrefs.SetInt("HP", 0);
        }
        hpTM.text = $"{PlayerPrefs.GetInt("HP")}";
    }

    private void Money()
    {
        if (PlayerPrefs.GetInt("Wealth") < 0)
        {
            PlayerPrefs.SetInt("Wealth", 0);
        }
        moneyTM.text = $"{PlayerPrefs.GetInt("Wealth")}";
    }

    public void CheatMoney(int Amount)
    {
        currentCheatMoney += Amount;
        PlayerPrefs.SetInt("Wealth", currentCheatMoney);        
    }

    public void FirstPressOver()
    {
        firstPressOver = true;
    }

    void DisplayTime(float timeToDisplay)
    {
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);
        float milliSeconds = (timeToDisplay % 1) * 1000;
        timeTM.text = "Next Wave in: " + string.Format("{0:00}:{1:000}", seconds, milliSeconds);
    }

}
