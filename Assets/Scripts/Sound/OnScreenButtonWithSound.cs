using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class OnScreenButtonWithSound : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] protected AudioClip clickSound = null;
    protected AudioSource soundEffectsSource = null;

    protected virtual void Start()
    {
        soundEffectsSource = AudioManager.Singleton.GetSoundEffectsSource();
        soundEffectsSource.loop = true;
    }

    public virtual void OnPointerDown(PointerEventData eventData)
    {   
        soundEffectsSource.PlayOneShot(clickSound);
    }
}