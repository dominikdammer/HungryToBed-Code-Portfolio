using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class LevelSettings: MonoBehaviour
{
    [SerializeField]
    private GameObject gameOverScreen;

    public List<BaseTower> availableTowers;
    public int waveAmounts;

    private Spawner spawner;
    [SerializeField]
    private int startHP = 100;
    [SerializeField]
    private int startMoney = 10;

    private void Awake()
    {
        spawner = GetComponent<Spawner>();

        if (PlayerPrefs.GetInt("HP") <= startHP)
        {
            PlayerPrefs.SetInt("HP", startHP);
        }


        if (PlayerPrefs.GetInt("Wealth") <= startMoney)
        {
            PlayerPrefs.SetInt("Wealth", startMoney);
        }

        PlayerPrefs.SetInt("CurrentWave", 0);
        PlayerPrefs.SetInt("TotalWaves", spawner.waves.Count);

        if (gameOverScreen.activeInHierarchy && gameOverScreen != null)
        {
            gameOverScreen.SetActive(false);
        }
    }

    private void Update()
    {
        PlayerPrefs.SetInt("CurrentWave", spawner.currentWaveNr);

        if(PlayerPrefs.GetInt("HP") <= 0)
        {
            OnLoose();
        }
    }

    private void OnLoose()
    {
        if(gameOverScreen != null)
        gameOverScreen.SetActive(true);
    }


}
