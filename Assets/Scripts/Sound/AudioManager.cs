using UnityEngine;
using System.Collections;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

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
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }

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
        if (scene.name == "Login" || scene.name == "MainMenu")
        {
            PlayBackgroundMusic("bgm1");
        }
        else if (scene.name == "GameMenu")
        {
            PlayBackgroundMusic("bgm3");
        }
    }

    public void PlayBackgroundMusic(string clipName, float fadeDuration = 1.0f)
    {
        if (currentMusicClipName == clipName && backgroundMusicSource.isPlaying)
        {
            return; // Music is already playing
        }

        AudioClip clip = Resources.Load<AudioClip>("Sound/BGM/" + clipName);
        if (clip == null)
        {
            Debug.LogError($"AudioClip {clipName} not found in Resources/Sound/BGM/");
            return;
        }

        // Crossfade music if something is already playing
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

        // Fade out the current music
        while (backgroundMusicSource.volume > 0)
        {
            backgroundMusicSource.volume -= startVolume * Time.deltaTime / fadeDuration;
            yield return null;
        }

        backgroundMusicSource.Stop();
        backgroundMusicSource.clip = newClip;
        backgroundMusicSource.Play();

        // Fade in the new music
        while (backgroundMusicSource.volume < startVolume)
        {
            backgroundMusicSource.volume += startVolume * Time.deltaTime / fadeDuration;
            yield return null;
        }

        backgroundMusicSource.volume = startVolume;
        currentMusicClipName = newClip.name; // Update the current clip name
    }

    public void PlaySoundEffect(string clipName)
    {
        // Load the sound effect from Resources folder
        AudioClip clip = Resources.Load<AudioClip>("Sound/SFX/" + clipName);

        if (clip == null)
        {
            Debug.LogError($"AudioClip {clipName} not found in Resources/Sound/SFX/!");
            return;
        }

        // Play the sound effect once
        soundEffectsSource.PlayOneShot(clip);
    }

    public void PlayVoiceOver(string clipName)
    {
        // Load the voice over from Resources folder
        AudioClip clip = Resources.Load<AudioClip>("Sound/VoiceOver/" + clipName);

        if (clip == null)
        {
            Debug.LogError($"AudioClip {clipName} not found in Resources/Sound/VoiceOver/!");
            return;
        }

        // Stop the current voice over if playing
        if (voiceOverSource.isPlaying)
        {
            voiceOverSource.Stop();
        }

        // Play the new voice over clip
        voiceOverSource.clip = clip;
        voiceOverSource.Play();
    }


    public void SetMasterVolume(float volume)
    {
        if (masterMixer != null)
        {
            masterMixer.SetFloat("MasterVolume", Mathf.Log10(volume) * 20);
        }
    }
}
