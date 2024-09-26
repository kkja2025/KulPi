using UnityEngine;
using UnityEngine.UI;

public class OnScreenButtonWithSound : MonoBehaviour
{
    [SerializeField] private AudioClip clickSound;

    private void Start()
    {
        Button button = GetComponent<Button>();
        if (button != null)
        {
            button.onClick.AddListener(PlaySoundEffect);
        }
    }

    public void PlaySoundEffect()
    {
        AudioManager.Singleton.soundEffectsSource.PlayOneShot(clickSound);
    }
}
