using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance; // Singleton for global access

    [Header("Background Music")]
    public AudioSource musicSource;     // Source for background music

    [Header("Sound Effects")]
    public AudioClip jumpSound;
    public AudioClip shootSound;
    public AudioClip coinCollectSound;
    public AudioSource effectsSource;   // Source for sound effects

    private void Awake()
    {
        // Singleton pattern
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Make sure this persists across scenes
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlayJumpSound()
    {
        effectsSource.PlayOneShot(jumpSound);
    }

 

    public void PlayShootSound()
    {
        effectsSource.PlayOneShot(shootSound);
    }

    public void PlayCoinCollectSound()
    {
        effectsSource.PlayOneShot(coinCollectSound);
    }

    public void PlayBackgroundMusic()
    {
        if (!musicSource.isPlaying)
        {
            musicSource.loop = true;
            musicSource.Play();
        }
    }
}
