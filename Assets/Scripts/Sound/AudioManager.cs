using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEditor.Rendering.LookDev;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioMixer masterMixer;
    [SerializeField] private AudioSource backgroundMusicSource = null;
    [SerializeField] private AudioSource soundEffectsSource = null;
    [SerializeField] private AudioSource voiceOverSource = null;
    private bool initialized = false;
    private static AudioManager singleton = null;
    private string currentMusicClipName = "";

    public static AudioManager Singleton
    {
        get
        {
            if (singleton == null)
            {
                singleton = FindFirstObjectByType<AudioManager>();
                singleton.Initialize();
            }
            return singleton;
        }
    }

    private void Initialize()
    {
        if (initialized) { return; }
        initialized = true;
        DontDestroyOnLoad(gameObject);
    }

    private void Awake()
    {
        if (singleton != null && singleton != this)
        {
            Destroy(gameObject);
            return;
        }
        else
        {
            singleton = this;
            DontDestroyOnLoad(gameObject);
            
        }
        backgroundMusicSource.ignoreListenerPause = true;
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDestroy()
    {
        if (singleton == this)
        {
            singleton = null;
        }
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        LoadSoundSettings();
        if (scene.name == "Login" || scene.name == "MainMenu")
        {
            PlayBackgroundMusic("Main_Theme");
        }
        else if (scene.name == "Chapter1")
        {
            PlayBackgroundMusic("Forest");
        } 
        else if (scene.name == "Chapter1BossDiwata")
        {
            PlayBackgroundMusic("Battle_Diwata");
        }
        else if (scene.name == "Chapter1SigbinTikbalang")
        {
            PlayBackgroundMusic("Battle_Diwata");
        }
    }

    public void PlayBackgroundMusic(string clipName, float fadeDuration = 1.0f)
    {
        if (currentMusicClipName == clipName && backgroundMusicSource.isPlaying)
        {
            return; 
        }

        AudioClip clip = Resources.Load<AudioClip>("Sound/BGM/" + clipName);
        if (clip == null)
        {
            Debug.LogError($"AudioClip {clipName} not found in Resources/Sound/BGM/");
            return;
        }

        if (backgroundMusicSource.isPlaying)
        {
            StartCoroutine(CrossfadeMusic(clip, fadeDuration));
        }
        else
        {
            backgroundMusicSource.clip = clip;
            backgroundMusicSource.Play();
            currentMusicClipName = clipName;
        }
    }

    private IEnumerator CrossfadeMusic(AudioClip newClip, float fadeDuration)
    {
        float startVolume = backgroundMusicSource.volume;

        while (backgroundMusicSource.volume > 0)
        {
            backgroundMusicSource.volume -= startVolume * Time.deltaTime / fadeDuration;
            yield return null;
        }

        backgroundMusicSource.Stop();
        backgroundMusicSource.clip = newClip;
        backgroundMusicSource.Play();

        while (backgroundMusicSource.volume < startVolume)
        {
            backgroundMusicSource.volume += startVolume * Time.deltaTime / fadeDuration;
            yield return null;
        }

        backgroundMusicSource.volume = startVolume;
        currentMusicClipName = newClip.name; 
    }

    public void PlaySoundEffect(string clipName)
    {
        AudioClip clip = Resources.Load<AudioClip>("Sound/SFX/" + clipName);

        if (clip == null)
        {
            Debug.LogError($"AudioClip {clipName} not found in Resources/Sound/SFX/!");
            return;
        }

        soundEffectsSource.PlayOneShot(clip);
    }

    public void PlaySwordSoundEffect(int clickCount)
    {
        List<string> soundClipNames = new List<string> { "sword_swing_1", "sword_swing_2", "sword_swing_3", "sword_swing_4", "sword_swing_5" };

        string clipName = soundClipNames[clickCount % soundClipNames.Count];
        AudioClip clip = Resources.Load<AudioClip>("Sound/SFX/" + clipName);

        if (clip == null)
        {
            Debug.LogError($"AudioClip {clipName} not found in Resources/Sound/SFX/!");
            return;
        }

        soundEffectsSource.PlayOneShot(clip);
    }

    public void PlayVoiceOver(string clipName)
    {
        AudioClip clip = Resources.Load<AudioClip>("Sound/VoiceOver/" + clipName);

        if (clip == null)
        {
            Debug.LogError($"AudioClip {clipName} not found in Resources/Sound/VoiceOver/!");
            return;
        }

        if (voiceOverSource.isPlaying)
        {
            voiceOverSource.Stop();
        }

        voiceOverSource.clip = clip;
        voiceOverSource.Play();
    }

    private float MapVolumeToDb(int volume)
    {
        volume = Mathf.Clamp(volume, 0, 4);

        switch (volume)
        {
            case 0:
                return -80f;
            case 1:
                return Mathf.Lerp(-80f, 0f, 0.25f);
            case 2: 
                return Mathf.Lerp(-80f, 0f, 0.5f);
            case 3: 
                return Mathf.Lerp(-80f, 0f, 0.75f);
            case 4: 
                return 0f;
            default:
                return -80f; 
        }
    }

    public void SetMasterVolume(int volume)
    {
        float volumeDb = MapVolumeToDb(volume);
        masterMixer.SetFloat("Master", volumeDb);
    }

    public void SetBackgroundMusicVolume(int volume)
    {
        float volumeDb = MapVolumeToDb(volume);
        masterMixer.SetFloat("Background", volumeDb); 
    }

    public void SetSoundEffectsVolume(int volume)
    {
        float volumeDb = MapVolumeToDb(volume);
        masterMixer.SetFloat("SoundEffects", volumeDb); 
    }

    public void SetVoiceOverVolume(int volume)
    {
        float volumeDb = MapVolumeToDb(volume);
        masterMixer.SetFloat("VoiceOver", volumeDb); 
    }

    public void LoadSoundSettings()
    {
        try 
        {
            if (!PlayerPrefs.HasKey("MasterVolume"))
            {
                PlayerPrefs.SetInt("MasterVolume", 4);
                PlayerPrefs.SetInt("BackgroundMusicVolume", 4);
                PlayerPrefs.SetInt("SoundEffectsVolume", 4);
                PlayerPrefs.SetInt("VoiceOverVolume", 4);

                SetMasterVolume(4);
                SetBackgroundMusicVolume(4);
                SetSoundEffectsVolume(4);
                SetVoiceOverVolume(4);

                Debug.Log("Sound settings keys not found, using default value.");
            }
            else
            {
                int masterVolume = PlayerPrefs.GetInt("MasterVolume");
                int backgroundMusicVolume = PlayerPrefs.GetInt("BackgroundMusicVolume");
                int soundEffectsVolume = PlayerPrefs.GetInt("SoundEffectsVolume");
                int voiceOverVolume = PlayerPrefs.GetInt("VoiceOverVolume");

                SetMasterVolume(masterVolume);
                SetBackgroundMusicVolume(backgroundMusicVolume);
                SetSoundEffectsVolume(soundEffectsVolume);
                SetVoiceOverVolume(voiceOverVolume);
                Debug.Log("Sound settings found.");
            }
        }
        catch (PlayerPrefsException e)
        {
            Debug.LogError("Failed to load sound settings." + e.Message);
        }
    }
}
