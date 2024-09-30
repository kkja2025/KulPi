using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioMixer masterMixer = null;
    [SerializeField] private AudioSource backgroundMusicSource = null;
    [SerializeField] private AudioSource soundEffectsSource = null;
    [SerializeField] private AudioSource voiceOverSource = null;

    private bool initialized = false;
    private static AudioManager singleton = null;

    private readonly Dictionary<string, string> sceneMusicMap = new Dictionary<string, string>
    {
        { "Login", "Main_Theme" },
        { "MainMenu", "Main_Theme" },
        { "Chapter1", "Forest" },
        { "Chapter1BossDiwata", "Battle_Diwata" },
        { "Chapter1SigbinTikbalang", "Battle_Diwata" }
    };

    private string currentMusicClipName = "";
    private readonly Dictionary<string, AudioClip> audioClipCache = new Dictionary<string, AudioClip>();

    private const string MASTER_VOLUME_KEY = "MasterVolume";
    private const string BGM_VOLUME_KEY = "BackgroundMusicVolume";
    private const string SFX_VOLUME_KEY = "SoundEffectsVolume";
    private const string VOICEOVER_VOLUME_KEY = "VoiceOverVolume";

    public static AudioManager Singleton
    {
        get
        {
            if (singleton == null)
            {
                singleton = FindObjectOfType<AudioManager>();
                if (singleton == null)
                {
                    Debug.LogError("AudioManager instance not found in the scene!");
                }
                else
                {
                    singleton.Initialize();
                }
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

        singleton = this;
        DontDestroyOnLoad(gameObject);
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
        if (sceneMusicMap.TryGetValue(scene.name, out string musicClipName))
        {
            PlayBackgroundMusic(musicClipName);
        }
    }

    public AudioSource GetBackgroundMusicSource()
    {
        return backgroundMusicSource;
    }

    public AudioSource GetSoundEffectsSource()
    {
        return soundEffectsSource;
    }

    public AudioSource GetVoiceOverSource()
    {
        return voiceOverSource;
    }

    private AudioClip GetCachedClip(string path)
    {
        if (audioClipCache.TryGetValue(path, out AudioClip clip))
        {
            return clip;
        }

        clip = Resources.Load<AudioClip>(path);
        if (clip == null)
        {
            Debug.LogError($"AudioClip not found at {path}");
            return null;
        }

        audioClipCache[path] = clip;
        return clip;
    }

    public void PlayBackgroundMusic(string clipName, float fadeDuration = 1.0f)
    {
        if (currentMusicClipName == clipName && backgroundMusicSource.isPlaying)
        {
            return;
        }

        AudioClip clip = GetCachedClip("Sound/BGM/" + clipName);
        if (clip == null)
        {
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
        AudioClip clip = GetCachedClip("Sound/SFX/" + clipName);
        if (clip == null) return;

        soundEffectsSource.PlayOneShot(clip);
    }

    public void PlaySwordSoundEffect(int clickCount)
    {
        List<string> soundClipNames = new List<string> { "sword_swing_1", "sword_swing_2", "sword_swing_3", "sword_swing_4", "sword_swing_5" };

        string clipName = soundClipNames[clickCount % soundClipNames.Count];
        AudioClip clip = GetCachedClip("Sound/SFX/" + clipName);
        if (clip == null) return;

        soundEffectsSource.PlayOneShot(clip);
    }

    public void PlayVoiceOver(string clipName)
    {
        AudioClip clip = GetCachedClip("Sound/VoiceOver/" + clipName);
        if (clip == null) return;

        if (voiceOverSource.isPlaying)
        {
            voiceOverSource.Stop();
        }

        voiceOverSource.clip = clip;
        voiceOverSource.Play();
    }

    private float MapVolumeToDb(int volume, float maxDb)
    {
        volume = Mathf.Clamp(volume, 0, 5);

        float minDb = -80f;

        return Mathf.Lerp(minDb, maxDb, volume / 5f);
    }

    private void SetVolume(string mixerParam, int volume, float maxDb)
    {
        float volumeDb = MapVolumeToDb(volume, maxDb);
        masterMixer.SetFloat(mixerParam, volumeDb);
    }

    public void SetMasterVolume(int volume) => SetVolume("Master", volume, 20f);
    public void SetBackgroundMusicVolume(int volume) => SetVolume("Background", volume, 0f);
    public void SetSoundEffectsVolume(int volume) => SetVolume("SoundEffects", volume, 0f);
    public void SetVoiceOverVolume(int volume) => SetVolume("VoiceOver", volume, 0f);

    public void LoadSoundSettings()
    {
        int masterVolume = PlayerPrefs.GetInt(MASTER_VOLUME_KEY, 4);
        int bgmVolume = PlayerPrefs.GetInt(BGM_VOLUME_KEY, 4);
        int sfxVolume = PlayerPrefs.GetInt(SFX_VOLUME_KEY, 4);
        int voiceOverVolume = PlayerPrefs.GetInt(VOICEOVER_VOLUME_KEY, 4);

        SetMasterVolume(masterVolume);
        SetBackgroundMusicVolume(bgmVolume);
        SetSoundEffectsVolume(sfxVolume);
        SetVoiceOverVolume(voiceOverVolume);
    }
}
