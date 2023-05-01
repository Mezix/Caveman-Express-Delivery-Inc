using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using System;

public class AudioManager : MonoBehaviour
{
    public AudioMixer audioMixer;
    public AudioSource _menuMusic;
    public AudioSource _gameMusic;
    public static float masterVolume;
    public static float musicVolume;
    public static float SFXVolume;
    [HideInInspector] public float defaultVol;

    public Slider MasterVolumeSlider;
    public Slider MusicVolumeSlider;
    public Slider SFXVolumeSlider;

    public static bool GameWasFirstStarted;

    private void Start()
    {
        REF.audio = this;
        DontDestroyOnLoad(transform.gameObject);
        defaultVol = 0.75f;
        InitPrefs();
        InitSliders();

        if(GameWasFirstStarted == false)
        {
            Loader._currentScene = Loader.Scene.MainMenu;
            GameWasFirstStarted = true;
            _menuMusic.Play();
        }
    }
    

    private void InitPrefs()
    {
        if (!PlayerPrefs.HasKey(PREFS.MASTER)) PlayerPrefs.SetFloat(PREFS.MASTER, defaultVol);
        if (!PlayerPrefs.HasKey(PREFS.MUSIC)) PlayerPrefs.SetFloat(PREFS.MUSIC, defaultVol);
        if (!PlayerPrefs.HasKey(PREFS.SFX)) PlayerPrefs.SetFloat(PREFS.SFX, defaultVol);

        UpdateVolume();
    }
    private void UpdateVolume()
    {
        masterVolume =  PlayerPrefs.GetFloat(PREFS.MASTER);
        musicVolume =   PlayerPrefs.GetFloat(PREFS.MUSIC);
        SFXVolume =     PlayerPrefs.GetFloat(PREFS.SFX);

        SetMasterVolume(masterVolume);
        SetMusicVolume(musicVolume);
        SetSFXVolume(SFXVolume);
    }

    public void FadeFromGameToMenu()
    {
        StopAllCoroutines();
        StartCoroutine(FadeBetweenTracks(REF.audio._gameMusic, REF.audio._menuMusic, 100));
    }
    public void FadeFromMenuToGame()
    {
        StopAllCoroutines();
        StartCoroutine(FadeBetweenTracks(REF.audio._menuMusic, REF.audio._gameMusic, 100));
    }

    public IEnumerator FadeBetweenTracks(AudioSource fromMusic, AudioSource toMusic, int halfFadeLength)
    {
        for (int i = 0; i < halfFadeLength; i++)
        {
            fromMusic.volume = 1 - ((float) i / (halfFadeLength * 2));
            yield return new WaitForFixedUpdate();
        }

        toMusic.Play();

        for (int i = 0; i < halfFadeLength; i++)
        {
            fromMusic.volume = 0.5f - ((float)i / (halfFadeLength * 2));
            toMusic.volume = (float)i / (halfFadeLength);
            yield return new WaitForFixedUpdate();
        }
        fromMusic.volume = 0;
        fromMusic.Stop();
        yield return null;
    }

    private void InitSliders()
    {
        MasterVolumeSlider.onValueChanged.AddListener(delegate { SetMasterVolume(MasterVolumeSlider.value); });
        MusicVolumeSlider.onValueChanged.AddListener(delegate { SetMusicVolume(MusicVolumeSlider.value); });
        SFXVolumeSlider.onValueChanged.AddListener(delegate { SetSFXVolume(SFXVolumeSlider.value); });
    }

    public void SetMasterVolume(float Volume)
    {
        if (MasterVolumeSlider.value == 1)
        {
            audioMixer.SetFloat("MasterVolume", 0);
        }
        else if(MasterVolumeSlider.value == 0)
        {
            audioMixer.SetFloat("MasterVolume", -80);
        }
        else
        {
            audioMixer.SetFloat("MasterVolume", Mathf.Log10(Volume) * 20);
        }
        masterVolume = Volume;
        MasterVolumeSlider.value = Volume;
        PlayerPrefs.SetFloat(PREFS.MASTER, Volume);
    }
    public void SetMusicVolume(float Volume)
    {
        if (MusicVolumeSlider.value == 1)
        {
            audioMixer.SetFloat("MusicVolume", 0);
        }
        else if (MusicVolumeSlider.value == 0)
        {
            audioMixer.SetFloat("MusicVolume", -80);
        }
        else
        {
            audioMixer.SetFloat("MusicVolume", Mathf.Log10(Volume) * 20);
        }
        musicVolume = Volume;
        MusicVolumeSlider.value = Volume;
        PlayerPrefs.SetFloat(PREFS.MUSIC, Volume);
    }
    public void SetSFXVolume(float Volume)
    {
        if (SFXVolumeSlider.value == 1)
        {
            audioMixer.SetFloat("SFXVolume", 0);
        }
        else if (SFXVolumeSlider.value == 0)
        {
            audioMixer.SetFloat("SFXVolume", -80);
        }
        else
        {
            audioMixer.SetFloat("SFXVolume", Mathf.Log10(Volume) * 20);
        }
        SFXVolume = Volume;
        SFXVolumeSlider.value = Volume;
        PlayerPrefs.SetFloat(PREFS.SFX, Volume);
    }
}
