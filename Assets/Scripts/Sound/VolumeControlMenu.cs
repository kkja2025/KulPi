using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class VolumeControlMenu : Panel
{
    [SerializeField] private Button VolumeUpButton = null;
    [SerializeField] private Button VolumeDownButton = null;
    [SerializeField] private TMP_InputField VolumeInputField = null;
    [SerializeField] private string volumeType;
    private const int minVolume = 0;
    private const int maxVolume = 4;

    public override void Initialize()
    {
        if (IsInitialized)
        {
            return;
        }
        VolumeDownButton.onClick.AddListener((VolumeDown));
        VolumeUpButton.onClick.AddListener((VolumeUp));
        SetInputFieldsReadOnly(true);
        base.Initialize();
    }

    private void ApplyVolumeChange(int volume)
    {
        switch (volumeType)
        {
            case "Master":
                AudioManager.Singleton.SetMasterVolume(volume);
                PlayerPrefs.SetInt("MasterVolume", volume);
                PlayerPrefs.Save();
                break;
            case "BackgroundMusic":
                AudioManager.Singleton.SetBackgroundMusicVolume(volume);
                PlayerPrefs.SetInt("BackgroundMusicVolume", volume);
                PlayerPrefs.Save();
                break;
            case "SoundEffects":
                AudioManager.Singleton.SetSoundEffectsVolume(volume);
                PlayerPrefs.SetInt("SoundEffectsVolume", volume);
                PlayerPrefs.Save();
                break;
            case "VoiceOver":
                AudioManager.Singleton.SetVoiceOverVolume(volume);
                PlayerPrefs.SetInt("VoiceOverVolume", volume);
                PlayerPrefs.Save();
                break;
            default:
                break;
        }
    }

    private int ApplyDefaultVolume(string volumeType)
    {
        switch (volumeType)
        {
            case "Master":
                return PlayerPrefs.GetInt("MasterVolume");
            case "BackgroundMusic":
                return PlayerPrefs.GetInt("BackgroundMusicVolume");
            case "SoundEffects":
                return PlayerPrefs.GetInt("SoundEffectsVolume");
            case "VoiceOver":
                return PlayerPrefs.GetInt("VoiceOverVolume");
            default:
                return 3;
        }
    }

    public override void Open()
    {
        VolumeInputField.text = ApplyDefaultVolume(volumeType).ToString();
        base.Open();
    }

    private void SetInputFieldsReadOnly(bool isReadOnly)
    {
        VolumeInputField.readOnly = isReadOnly;
        VolumeInputField.interactable = !isReadOnly;
    }

    public void VolumeDown()
    {
        UpdateVolume(VolumeInputField, -1);
    }

    public void VolumeUp()
    {
        UpdateVolume(VolumeInputField, 1);
    }

    private void UpdateVolume(TMP_InputField inputField, int delta)
    {
        if (int.TryParse(inputField.text, out int volume))
        {
            volume = Mathf.Clamp(volume + delta, minVolume, maxVolume);
            inputField.text = volume.ToString();
            ApplyVolumeChange(volume);
        }
    }
}
