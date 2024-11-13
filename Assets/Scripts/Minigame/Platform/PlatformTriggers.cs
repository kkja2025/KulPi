using UnityEngine;

public class PlatformTriggers : MonoBehaviour
{
    [SerializeField] private string triggerType;
    [SerializeField] protected AudioClip onCollisionSound;

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Player"))
        {
            if(triggerType == "victory")
            {
                PlatformFallManager.Singleton.ShowVictoryMenu();
                Destroy(gameObject);
            } 
            else if(triggerType == "finish")
            {
                PlatformFallManager.Singleton.Finish();
            } 
            else if(triggerType == "enemy")
            {
                AudioManager.Singleton.PlaySoundEffectOneShot(onCollisionSound);
                PlatformFallManager.Singleton.GameOver();
            }
        
        }
    }
}
