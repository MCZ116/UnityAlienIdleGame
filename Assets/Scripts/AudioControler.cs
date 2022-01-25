using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioControler : MonoBehaviour
{
    public AudioMixer audioMixer;
    public AudioSource buttonClickSound;
    public Slider musicSlider;
    public Slider sfxSlider;

    void Start()
    {
        musicSlider.value = PlayerPrefs.GetFloat("MusicVolume", 0.75f);
        sfxSlider.value = PlayerPrefs.GetFloat("SFXVolume", 0.75f);
    }

    public void MusicVolume(float musiclvl)
    {
        float sliderValue = musicSlider.value;
        audioMixer.SetFloat("MusicVol", Mathf.Log10(sliderValue) * 20);
        PlayerPrefs.SetFloat("MusicVolume", sliderValue);
    }

    public void SFXVolume(float sfxlvl)
    {
        float sliderValue = sfxSlider.value;
        audioMixer.SetFloat("SFXVol", Mathf.Log10(sliderValue) * 20);
        PlayerPrefs.SetFloat("SFXVolume", sliderValue);
    }

    public void ButtonClickSound()
    {
        buttonClickSound.Play();
    }


}
