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
    private const int maxVolume = 5;
    private const int defaultVolume = 5;

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
                break;
            case "BackgroundMusic":
                AudioManager.Singleton.SetBackgroundMusicVolume(volume);
                break;
            case "SoundEffects":
                AudioManager.Singleton.SetSoundEffectsVolume(volume);
                break;
            case "VoiceOver":
                AudioManager.Singleton.SetVoiceOverVolume(volume);
                break;
            default:
                return;
        }

        PlayerPrefs.SetInt($"{volumeType}Volume", volume);
        PlayerPrefs.Save();
    }

    private int GetSavedOrDefaultVolume()
    {
        return PlayerPrefs.GetInt($"{volumeType}Volume", defaultVolume);
    }

    public override void Open()
    {
        VolumeInputField.text = GetSavedOrDefaultVolume().ToString();
        base.Open();
    }

    private void SetInputFieldsReadOnly(bool isReadOnly)
    {
        VolumeInputField.readOnly = isReadOnly;
        VolumeInputField.interactable = !isReadOnly;
    }

    private void VolumeDown()
    {
        UpdateVolume(-1);
    }

    private void VolumeUp()
    {
        UpdateVolume(1);
    }

    private void UpdateVolume(int delta)
    {
        if (int.TryParse(VolumeInputField.text, out int volume))
        {
            volume = Mathf.Clamp(volume + delta, minVolume, maxVolume);
            VolumeInputField.text = volume.ToString();
            ApplyVolumeChange(volume);
        }
    }
}
