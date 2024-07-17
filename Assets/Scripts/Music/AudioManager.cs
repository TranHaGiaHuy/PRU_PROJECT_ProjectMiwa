using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    public Sound[] musicSounds, sfxSound;
    public AudioSource musicSource,sfxSource;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else { Destroy(gameObject); }
    }
    private void Start()
    {
        //  PlayerMusic(SceneManager.GetActiveScene().name);
        PlayerMusic("Menu");
    }
    public void PlayerMusic(string name)
    {
        Sound s = Array.Find(musicSounds, x => x.name == name);
        if (s==null)
        {
            Debug.LogWarning("Sound Not Found");         
        }
        else
        {
            {
                musicSource.clip = s.clip;
                musicSource.Play();
            }
        }

    }
    public void PlayerSFX(string name)
    {
        Sound s = Array.Find(sfxSound, x => x.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound Not Found");
        }
        else
        {
            {
                sfxSource.PlayOneShot(s.clip);
            }
        }

    }

    public void ToggleMusic()
    {
        musicSource.mute = !musicSource.mute;
    }
    public void ToggleSFX()
    {
        sfxSource.mute = !sfxSource.mute;
    }
    public void MusicVolume(float volume)
    {
        musicSource.volume = volume;
    }
    public void SFXVolume(float volume)
    {
        sfxSource.volume = volume;
    }
}
