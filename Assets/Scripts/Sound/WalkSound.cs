using UnityEngine;
using UnityEngine.EventSystems;

public class WalkSound : OnScreenButtonWithSound, IPointerUpHandler
{
    public override void Start()
    {
        soundEffectsSource = AudioManager.Singleton.GetSoundEffectsSource();
        soundEffectsSource.clip = clickSound;
        soundEffectsSource.loop = true;
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        soundEffectsSource.Play();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
       soundEffectsSource.Stop();
    }
}
