using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMusicHandler : MonoBehaviour
{
    [SerializeField, ReadOnly]
    private AudioClip activeClip;
    [SerializeField, ReadOnly]
    private float time;

    [SerializeField]
    private AudioClip[] musicList;

    private AudioSource audioSource;
    private bool isPlaying;
    


    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.volume = 0.3f;

    }
    private void Update()
    {
        if (!isPlaying)
        {
            //Get a random Music/Sfx from the Array
            audioSource.clip = musicList[Random.Range(0, musicList.Length)];
            activeClip = audioSource.clip;

            //Play random Clip that has been added into the Array
            audioSource.Play();

            //Fire the IEnumerator/Coroutine that has been put in
            StartCoroutine(RandomTime(audioSource.clip.length));
        }
    }

    IEnumerator RandomTime(float TimeTillNextSFX)
    {
        //Stops the if statment from Starting a new Couroutine
        isPlaying = true;

        //Adding Time between how long the clip is and then when it plays again
        float WaitTime = TimeTillNextSFX + Random.Range(20f, 120f);
        time = WaitTime;

        //Shows u when the Music/Sfx are gonna fire again
        Debug.Log("Music will play again at: " + WaitTime + " seconds.");

        //Waiting now for the song to finish and then the extra time between Music/Sfx
        yield return new WaitForSeconds(WaitTime);

        //Make the if statement fire again
        isPlaying = false;

    }
}

