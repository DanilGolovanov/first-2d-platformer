using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;


public class OptionMenu : MonoBehaviour
{
    public Resolution[] resolutions;
    public Dropdown resolusion;
    public Dropdown qualityDropdown;
    public Toggle fullscreenToggle;
    public Slider musicSlider;
    public Slider SFXSlider;
    public AudioMixer mixer;

    public void Awake()
    {
        LoadPlayerPrefs();
    }

    // Start is called before the first frame update
    void Start()
    {
        resolutions = Screen.resolutions;
        resolusion.ClearOptions();
        List<string> options = new List<string>();

        int currentRseolutionIndex = 0;
        for (int i = 0; i < resolutions.Length; i++)//go through every resolution
        {
            //build a string for displaying the resolution
            string option = resolutions[i].width + "x" + resolutions[i].height;
            options.Add(option);
            if (resolutions[i].width == Screen.currentResolution.width &&
                resolutions[i].height == Screen.currentResolution.height)
            {
                //we have found the current screen resolution, save that number
                currentRseolutionIndex = i;
            }
        }
        //set up our dropdown
        resolusion.AddOptions(options);
        resolusion.value = currentRseolutionIndex;
        resolusion.RefreshShownValue();

        if (!PlayerPrefs.HasKey("fullscreen"))
        {
            PlayerPrefs.SetInt("fullscreen", 0);
            Screen.fullScreen = false;
        }
        else
        {
            if (PlayerPrefs.GetInt("fullscreen") == 0)
            {
                Screen.fullScreen = false;
            }
            else
            {
                Screen.fullScreen = true;
            }
        }
        if (!PlayerPrefs.HasKey("quality"))
        {
            PlayerPrefs.SetInt("quality", 5);
            QualitySettings.SetQualityLevel(5);
        }
        else
        {
            QualitySettings.SetQualityLevel(PlayerPrefs.GetInt("quality"));
        }
        PlayerPrefs.Save();

    }
    public void SetMusicVolume(float value)
    {
        mixer.SetFloat("MusicVol", value);
    }
    public void SetSFXVolume(float value)
    {
        mixer.SetFloat("SFXVol", value);
    }
    public void SetResolution(int resolutionIndex)
    {
        Resolution res = resolutions[resolutionIndex];
        Screen.SetResolution(res.width, res.height, false);
    }
    public void SetFullScreen(bool fullscreen)
    {
        Screen.fullScreen = fullscreen;
    }

    public void ChangeQuality(int index)
    {
        QualitySettings.SetQualityLevel(index);
    }

    public void SavePlayerPrefs()
    {
        PlayerPrefs.SetInt("quality", QualitySettings.GetQualityLevel());
        if (fullscreenToggle.isOn)
        {
            PlayerPrefs.SetInt("fullscreen", 1);
        }
        else
        {
            PlayerPrefs.SetInt("fullscreen", 0);
        }
        PlayerPrefs.Save();

        float musicVol;
        if (mixer.GetFloat("MusicVol", out musicVol))
        {
            PlayerPrefs.SetFloat("MusicVol", musicVol);
        }

        float SFXVol;
        if (mixer.GetFloat("SFXVol", out SFXVol))
        {
            PlayerPrefs.SetFloat("SFXVol", SFXVol);
        }

    }
    public void LoadPlayerPrefs()
    {
        if (PlayerPrefs.HasKey("quality"))
        {
            int quality = PlayerPrefs.GetInt("quality");
            qualityDropdown.value = quality;
            if (QualitySettings.GetQualityLevel() != quality)
            {
                ChangeQuality(quality);
            }
        }
        if (PlayerPrefs.HasKey("fullscreen"))
        {
            if (PlayerPrefs.GetInt("fullscreen") == 0)
            {
                fullscreenToggle.isOn = false;
            }
            else
            {
                fullscreenToggle.isOn = true;
            }
        }
        if (PlayerPrefs.HasKey("MusicVol"))
        {
            float musicVol = PlayerPrefs.GetFloat("MusicVol");
            musicSlider.value = musicVol;
            mixer.SetFloat("MusicVol", musicVol);
        }

        if (PlayerPrefs.HasKey("SFXVol"))
        {
            float SFXVol = PlayerPrefs.GetFloat("SFXVol");
            SFXSlider.value = SFXVol;
            mixer.SetFloat("SFXVol", SFXVol);
        }

    }
}
