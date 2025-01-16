using System;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioSettings : MonoBehaviour
{

    [SerializeField] private AudioMixer mixer;
    [SerializeField] private Slider musicVolumeSlider;
    [SerializeField] private Slider SFXVolumeSlider;
    [SerializeField] private Slider masterVolumeSlider;

    public void SetMusicVolume()
    {
        float volume = musicVolumeSlider.value;
        mixer.SetFloat("Music", (float)(Math.Log10(volume) * 20));
    }

    public void SetSFXVolume()
    {
        float volume = SFXVolumeSlider.value;
        mixer.SetFloat("SFX", (float)(Math.Log10(volume) * 20));
    }

    public void SetMasterVolume()
    {
        float volume = masterVolumeSlider.value;
        mixer.SetFloat("Master", (float)(Math.Log10(volume) * 20));
    }
}
