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
    private const int maxVolume = 10;
    private const int defaultVolume = 8;

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

    public override void Open()
    {
        VolumeInputField.text = defaultVolume.ToString();
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

    private void ApplyVolumeChange(int volume)
    {
        if (volumeType == "Master")
        {
            AudioManager.Singleton.SetMasterVolume(volume);
        }
        else if (volumeType == "BackgroundMusic")
        {
            AudioManager.Singleton.SetBackgroundMusicVolume(volume);
        }
        else if (volumeType == "SoundEffects")
        {
            AudioManager.Singleton.SetSoundEffectsVolume(volume);
        }
        else if (volumeType == "VoiceOver")
        {
            AudioManager.Singleton.SetVoiceOverVolume(volume);
        }
    }
}
