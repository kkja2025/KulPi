using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;

public class WalkSound : OnScreenButtonWithSound, IPointerUpHandler
{
    [SerializeField] private AudioClip[] clickSounds;
    private int currentClipIndex = 0;
    private bool isPointerDown = false;

    protected override void Start()
    {
        soundEffectsSource = AudioManager.Singleton.GetSoundEffectsSource();
        soundEffectsSource.loop = false;
    }

    private void Update()
    {
        if (isPointerDown && !soundEffectsSource.isPlaying)
        {
            PlayNextClip();
        }
    }
    
    private void PlayNextClip()
    {
        if (clickSounds.Length > 0)
        {
            soundEffectsSource.clip = clickSounds[currentClipIndex];
            soundEffectsSource.Play();
            currentClipIndex = (currentClipIndex + 1) % clickSounds.Length;
        }
    }

    private void OnDisable()
    {
        soundEffectsSource.Stop();
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        isPointerDown = true;
        if (!soundEffectsSource.isPlaying)
        {
            PlayNextClip();
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isPointerDown = false;
        soundEffectsSource.Stop();
    }
}
