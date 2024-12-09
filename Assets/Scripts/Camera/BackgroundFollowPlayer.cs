using UnityEngine;

public class BackgroundFollowPlayer : MonoBehaviour
{
    public Transform player; // Reference to the player's Transform
    public Vector2 offset;   // Offset to maintain distance between the player and the background

    void LateUpdate()
    {
        if (player != null)
        {
            // Match the background's position to the player's position with an offset
            transform.position = new Vector3(player.position.x + offset.x, player.position.y + offset.y, transform.position.z);
            
        }
    }
}
