using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Classe gestionnaire des options de jeu
/// </summary>
public class SettingsMenu : MonoBehaviour
{
    private AudioManager audioManager;
    [SerializeField]
    private Slider generalVolumeSlider = null;
    [SerializeField]
    private Slider musicVolumeSlider = null;
    [SerializeField]
    private Slider effectsVolumeSlider = null;
    [SerializeField]
    private Dropdown resolutionDropDown = null;
    Resolution[] resolutions;


    private void Start()
    {
        audioManager = AudioManager.instance;
        generalVolumeSlider.value = 0.5f;
        musicVolumeSlider.value = Array.Find(audioManager.sounds, item => item.name == "GameMusic").volume;
        effectsVolumeSlider.value = Array.Find(audioManager.sounds, item => item.name == "PutKillTrap").volume;
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

    /// <summary>
    /// LEt a jour le volume general du jeu
    /// </summary>
    /// <param name="volume"> Le volume </param>
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

    /// <summary>
    /// Met a jour le volume de la musique
    /// </summary>
    /// <param name="volume"> Le volume </param>
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

    /// <summary>
    /// Met a jour le volume des effets sonores du jeu
    /// </summary>
    /// <param name="volume"> Le volume </param>
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

    /// <summary>
    /// Met a jour la qualite du jeu en fonction de ce que l'on a choisi
    /// </summary>
    /// <param name="quality"> Entier designant quelle qualite on souhaite </param>
    public void SetQuality (int quality)
    {
        QualitySettings.SetQualityLevel(quality);
    }


    /// <summary>
    /// Permet de mettre le jeu en plein ecran ou non.
    /// </summary>
    /// <param name="isFullScreen"> Booleen pour savoir si le jeu doit etre en fullscreen ou non </param>
    public void SetFullScreen(bool isFullScreen)
    {
        Screen.fullScreen = isFullScreen;
    }

    /// <summary>
    /// Permet de choisir la resolution du jeu
    /// </summary>
    /// <param name="resolutionIndex"> index de resolution d'ecran </param>
    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }


}
