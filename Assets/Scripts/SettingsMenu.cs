using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    public Slider masterVolumeSlider;
    public Slider musicSlider;
    public Slider SFXSlider;


    void Start()
    {
        masterVolumeSlider.value = PlayerPrefs.GetFloat("masterVolume", 0);
        musicSlider.value = PlayerPrefs.GetFloat("musicVolume", 0);
        SFXSlider.value = PlayerPrefs.GetFloat("SFXolume", 0);
    }

    public void SetMasterVolume(){
        PlayerPrefs.SetFloat("masterVolume" , masterVolumeSlider.value);
    }
    public void SetMusicVolume(){
        PlayerPrefs.SetFloat("musicVolume" , musicSlider.value);
    }
    public void SetSFXVolume(){
        PlayerPrefs.SetFloat("SFXVolume" , SFXSlider.value);
    }
    public void BackToGame(){
        Time.timeScale = 1.0f;
        gameObject.SetActive(false);
    }
}
