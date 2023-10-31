using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextWaveTimer : MonoBehaviour
{
    [SerializeField]
    private Spawner spawner;

    private float spawnTimer;
    private bool timerReady;

    public float WaveSpawnTime { get { return spawnTimer; } }

    // Update is called once per frame
    void Update()
    {      
        if (!spawner.WaveActive)
        {
            spawnTimer = 0;
        }
        else
        {
            Timer();
        }
    }

    private void Timer()
    {
        if (timerReady)
        {
            spawnTimer -= Time.deltaTime;

            if (spawnTimer <= 0)
            {
                timerReady = false;
                spawnTimer = 0;
            }
        }
    }

    public void BeginWaveTimer()
    {
        if (spawner == null)
        {
            return;
        }
        timerReady = true;
    }

    private void ResetTimer()
    {
        spawnTimer = spawner.WaveDelay;
    }

    private void StopTimer()
    {
        spawnTimer = 0;
    }

    /// <summary>
    /// singals received, subscribe and unsubscribe
    /// </summary>
    private void OnEnable()
    {
        Spawner.OnGroupSpawned += BeginWaveTimer;
        spawnTimer = spawner.WaveDelay;
        Spawner.OnResetSpawnTime += ResetTimer;
        Spawner.OnWavesOver += StopTimer;
    }

    private void OnDisable()
    {        
        Spawner.OnGroupSpawned -= BeginWaveTimer;
        spawnTimer = 0;
        Spawner.OnResetSpawnTime -= ResetTimer;
        Spawner.OnWavesOver -= StopTimer;
    }
}
