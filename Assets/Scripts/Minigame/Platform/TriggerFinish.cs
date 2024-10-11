using UnityEngine;

public class TriggerFinish : MonoBehaviour
{
    [SerializeField] private string triggerType;

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Player"))
        {
            if(triggerType == "victory")
            {
                PlatformFallManager.Singleton.ShowVictoryMenu();
            } 
            else if(triggerType == "finish")
            {
                PlatformFallManager.Singleton.Finish();
            }

        }
    }
}
