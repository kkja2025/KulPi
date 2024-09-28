using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class OnScreenButtonWithSound : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] private AudioClip clickSound;

    public virtual void OnPointerDown(PointerEventData eventData)
    {
        AudioManager.Singleton.soundEffectsSource.PlayOneShot(clickSound);
    }
}
