using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;

public class WalkSound : OnScreenButtonWithSound, IPointerUpHandler
{
    protected override void Start()
    {
        soundEffectsSource = AudioManager.Singleton.GetSoundEffectsSource();
        soundEffectsSource.clip = clickSound;
        soundEffectsSource.loop = true;
    }
    
    private void OnDisable()
    {
        soundEffectsSource.Stop();
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
