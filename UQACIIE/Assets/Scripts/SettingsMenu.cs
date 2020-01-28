using System.Collections;
using System.Collections.Generic;
using System;

using UnityEngine;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    private AudioManager audioManager;
    [SerializeField]
    private Slider generalVolumeSlider;
    [SerializeField]
    private Slider musicVolumeSlider;
    [SerializeField]
    private Slider effectsVolumeSlider;
    public Dropdown resolutionDropDown;

    Resolution[] resolutions;


    private void Start()
    {
        audioManager = FindObjectOfType<AudioManager>();
        generalVolumeSlider.value = 0.5f;
        musicVolumeSlider.value = Array.Find(audioManager.sounds, item => item.name == "GameMusic").volume;
        effectsVolumeSlider.value = Array.Find(audioManager.sounds, item => item.name == "PutTrap").volume;

        resolutions = Screen.resolutions;
        resolutionDropDown.ClearOptions();
        List<string> options = new List<string>();

        int currentResolutionIndex = 0;
        for(int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + "x" + resolutions[i].height;
            options.Add(option);

            if(resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = i;
            }
        }

        resolutionDropDown.AddOptions(options);
        resolutionDropDown.value = currentResolutionIndex;
        resolutionDropDown.RefreshShownValue();

    }


    public void SetGeneralVolume(float volume)
    {
        foreach (Sound s in audioManager.sounds)
        {
            if (s == null)
            {
                Debug.LogWarning("Le son n'a pas été trouvé..");
                return;
            }
            s.source.volume = volume;
        }
    }

    public void SetMusicVolume(float volume)
    {
        Sound s = Array.Find(audioManager.sounds, item => item.name == "GameMusic");
        if (s == null)
        {
            Debug.LogWarning("Le son n'a pas été trouvé..");
            return;
        }
        s.source.volume = volume;
    }

    public void SetEffectsVolume(float volume)
    {
        foreach (Sound s in audioManager.sounds)
        {
            if (s == null)
            {
                Debug.LogWarning("Le son n'a pas été trouvé..");
                return;
            }
            else if (s.name != "GameMusic")
            s.source.volume = volume;
        }
    }


    public void SetQuality (int quality)
    {
        QualitySettings.SetQualityLevel(quality);
    }



    public void SetFullScreen(bool isFullScreen)
    {
        Screen.fullScreen = isFullScreen;
    }


    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }


}
