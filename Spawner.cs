using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[System.Serializable]
public class WaveGroup
{
    [AssetList(Path = "/ScriptableObjects")]
    public BaseAnimal animalSO;
    public string name;
    [Range(0f, 10f)]
    [LabelText(SdfIconType.Clock)]
    public float delay = 1f;
    [Range(0f, 10f)]
    public int spawnCount = 1;
    public bool hasSpawned = false;
    //public string message;
}

[System.Serializable]
public class Wave
{
    public string name;
    public List<WaveGroup> group;
}

public class Spawner : MonoBehaviour
{
    [Header("Non-changables")]
    public Animal animalPrefab;
    public PathCreation.PathCreator path;
    [HideInInspector]
    public int currentWaveNr = 0;


    //[Header("General Settings")]
    //public int waveAmount = 5;
    //public float interval = 1;

    [Header("Wave Settings")]
    [SerializeField]
    [Range(0f, 10f)]
    private float waveDelay = 1;
    
    [Tooltip("This field only exsists as quick access and does nothing on its own")]
    public ScriptableObject[] animalQuickAccess;
    public List<Wave> waves;


    private Wave currentWave;
    private BaseAnimal animalData;
    private bool waveActive = true;
    //private float timer;
    //private float delayFactor = 1.0f;

    //public AudioSource animalSound;

    public delegate void OnGroupSpawnedAction();
    public static event OnGroupSpawnedAction OnGroupSpawned;

    public delegate void OnResetSpawnTimerAction();
    public static event OnResetSpawnTimerAction OnResetSpawnTime;

    public delegate void OnWavesOverAction();
    public static event OnWavesOverAction OnWavesOver;


    public Wave CurrentWave { get { return currentWave; } }
    public float WaveDelay { get { return waveDelay; } }
    public bool WaveActive { get { return waveActive; } }




    private void Awake()
    {
        if (path != null)
        {
            return;
        }
        path = GameObject.FindGameObjectWithTag("Road").GetComponent<PathCreation.PathCreator>();
    }

    //private void Start()
    //{
    //    StartCoroutine(SpawnLoop());
    //}


    void Update()
    {
        if (!waveActive)
        {
            return;
        }
        if (currentWaveNr - 1 == waves.Count)
        {
            Debug.Log("All waves over");
            waveActive = false;
            StopCoroutine(SpawnLoop());
            OnWavesOver();
        }
    }


    IEnumerator SpawnLoop()
    {
        /// each wave gets an according name and increases the current wave nr by 1
        foreach (Wave wave in waves)
        {
            SetWaveInfo(wave);

            ///for each group in a wave, we set the name and grab the SO data.
            ///then spawn groups after delay
            foreach (WaveGroup group in wave.group)
            {
                SetGroupInfo(group);

                ///if we have a delay, we wait
                if (group.delay > 0)
                    yield return new WaitForSeconds(group.delay);

                SpawnWaveGroup(group);
                group.hasSpawned = true;
            }
            
            OnGroupSpawned();
            //wait after wave is over to trigger next wave
            yield return new WaitForSeconds(waveDelay);
            OnResetSpawnTime();

            yield return null;  // prevents crash if all delays are 0
        }
    }

    ///for each number of spawncount we instantiate one animal
    private void SpawnWaveGroup(WaveGroup group)
    {
        if (animalPrefab == null || group.spawnCount <= 0)
        {
            return;
        }
        for (int i = 0; i < group.spawnCount; i++)
        {
            Instantiate(animalPrefab);
        }
    }

    private void SetWaveInfo(Wave wave)
    {
        currentWave = wave;
        wave.name = currentWaveNr + ". Wave";
        currentWaveNr++;
    }

    private void SetGroupInfo(WaveGroup group)
    {
        group.name = animalPrefab.name;
        animalData = group.animalSO;
        animalPrefab.animalSO = animalData;
    }

    public void ForceSpawn()
    {
        waveActive = true;
        StartCoroutine(SpawnLoop());
    }


}
