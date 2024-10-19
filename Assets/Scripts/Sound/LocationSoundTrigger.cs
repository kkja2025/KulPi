using UnityEngine;

public class LocationSoundTrigger : MonoBehaviour
{
    [SerializeField] private AudioClip locationMusic;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (locationMusic != null)
            {
                AudioManager.Singleton.PlayBackgroundSound(locationMusic, true);
            }
        }
    }
}
