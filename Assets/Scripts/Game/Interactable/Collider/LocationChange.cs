using UnityEngine;
 
public class LocationChange : Interactable
{
    [SerializeField] private float x;
    [SerializeField] private float y;
    [SerializeField] private float z;

    protected override void OnInteractButtonClicked()
    {
        PlayerMovement playerMovement = GameObject.FindWithTag("Player").GetComponent<PlayerMovement>();
        if(playerMovement.isGrounded && !playerMovement.isMoving)
        {
            if (isPlayerInRange)
            {
                Vector3 newPosition = new Vector3(x, y, z);
                GameObject player = GameObject.FindWithTag("Player");
                PanelManager.Singleton.StartLoading(2f, 
                () => GameManager.Singleton.SetPlayerPosition(newPosition), 
                () => PanelManager.GetSingleton("hud").Open());     
            }
        }
    }
}