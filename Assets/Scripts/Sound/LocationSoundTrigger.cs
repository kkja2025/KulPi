using UnityEngine;

public class LocationSoundTrigger : MonoBehaviour
{
    [SerializeField] private AudioClip locationMusic;
    private bool hasTriggered = false;

    private void Update()
    {
        if (hasTriggered && locationMusic != null)
        {
            AudioManager.Singleton.PlayBackgroundSound(locationMusic, true);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !hasTriggered)
        {
            hasTriggered = true;
            if (locationMusic != null)
            {
                AudioManager.Singleton.PlayBackgroundSound(locationMusic, true);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            hasTriggered = false;
        }
    }
}
