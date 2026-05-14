
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

/*=============================================================================
Script Name:    <Options Manager>
Last Edited:    <2026-05-13>
Contributors:   <Khidany Ruiz>
Description:    <Handles all Settings in the Options Menu.>
Notes:          <WE NEED TO MESS WITH THE RESOLUTIONS SETTINGS INSIDE PROJECT SETTINGS -> QUALITY cause RN they are all the same. Default is Medium>
=============================================================================*/
public class OptionsManager : MonoBehaviour
{
    public AudioMixer audioMixer;

    public TMP_Dropdown resolutionDropdown;

    Resolution[] resolutions;

    private void Start()
    {
        resolutions = Screen.resolutions;


        resolutionDropdown.ClearOptions();

        List<string> options = new List<string>();

        int currentResolutionIndex = 0;

        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height;
            options.Add(option);

            if (resolutions[i].width == Screen.width &&
                resolutions[i].height == Screen.height)
            {
                currentResolutionIndex = i;
            }
        }

        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();
    }

    public void SetResolution (int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

    public void SetFullScreen (bool isfullScreen)
    {
        Screen.fullScreen = isfullScreen;
    }
    public void SetMasterVolume (float volume)
    {
        audioMixer.SetFloat("MasterVolume", volume);
    }
    public void SetSFXVolume(float volume)
    {
        audioMixer.SetFloat("SFXVolume", volume);
    }
    public void SetMusicVolume(float volume)
    {
        audioMixer.SetFloat("MusicVolume", volume);
    }

    /* KR - <Sets the Quality Settings based on the ones in the Project Settings> */
    public void SetQuality (int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
        string qualityName = QualitySettings.names[QualitySettings.GetQualityLevel()];
        Debug.Log("Current Quality: " + qualityName);
    }
}
