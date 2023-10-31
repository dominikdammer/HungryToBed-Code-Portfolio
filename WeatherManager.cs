using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class WeatherManager : MonoBehaviour
{
    [SerializeField]
    [BoxGroup]
    [LabelText(SdfIconType.Percent)]
    [SuffixLabel(SdfIconType.Percent)]
    [Range(0,100)]
    private int spawnChance = 20;

    [SerializeField]
    private GameObject[] weather;

    // Start is called before the first frame update
    void Start()
    {
        ///spawns random weather effect
        var randomNr = Mathf.RoundToInt(Random.Range(1, 100));

        if(randomNr <= spawnChance)
        {
            Instantiate(weather[Random.Range(0,weather.Length-1)]);
        }
    }

}
