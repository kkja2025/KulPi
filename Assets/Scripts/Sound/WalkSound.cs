using UnityEngine;
using UnityEngine.EventSystems;

public class WalkSound : OnScreenButtonWithSound, IPointerUpHandler
{
    public AudioClip walkClip;

    public void Start()
    {
        AudioManager.Singleton.soundEffectsSource.clip = walkClip;
        AudioManager.Singleton.soundEffectsSource.loop = true;
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        AudioManager.Singleton.soundEffectsSource.Play();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        AudioManager.Singleton.soundEffectsSource.Stop();
    }
}
