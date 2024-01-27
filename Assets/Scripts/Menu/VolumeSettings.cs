using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeSettings : MonoBehaviour
{
    [SerializeField]
    public SettingsData settings;

    [SerializeField] private AudioMixer myMixer;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider SFXSlider;

    private void Start()
    {
        if(settings)
        {
            LoadVolume();
        } else
        {
            SetMusicVolume();
        }    
    }
    public void SetMusicVolume()
    {
        float volume = musicSlider.value;
        myMixer.SetFloat("Music", Mathf.Log10(volume) * 20);
        settings.MusicVolume = volume;
    }

    public void SetSFXVolume()
    {
        float volume = SFXSlider.value;
        myMixer.SetFloat("SFX", Mathf.Log10(volume) * 20);
        settings.SFXVolume = volume;
    }

    public void LoadVolume()
    {
        musicSlider.value = settings.MusicVolume;
        SFXSlider.value = settings.SFXVolume;
        SetMusicVolume();
    }
}
