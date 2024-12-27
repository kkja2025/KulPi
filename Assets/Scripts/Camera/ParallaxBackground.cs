using UnityEngine;

public class ParallaxBackground : MonoBehaviour
{
    private float length, startPositionX, startPositionY;
    private Transform cam;

    public GameObject player; 
    public float parallaxEffectX = 0.5f; 
    public float parallaxEffectY = 0.5f; 

    public float minTriggerPointX = -10f;
    public float maxTriggerPointX = 10f;
    public float limitY = 0f;

    void Start()
    {
        startPositionX = transform.position.x;
        startPositionY = transform.position.y;
        length = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    void FixedUpdate()
    {
        ApplyPlayerParallax();
    }

    private void ApplyPlayerParallax()
    {
        if (player == null) return;

        Vector2 playerPosition = player.transform.position;

        if (IsPlayerWithinTrigger(playerPosition))
        {
            float distanceX = playerPosition.x * parallaxEffectX;
            float distanceY = playerPosition.y * parallaxEffectY;

            transform.position = new Vector2(startPositionX + distanceX, startPositionY + distanceY);

            float movementX = playerPosition.x * (1 - parallaxEffectX);

            if (movementX > startPositionX + length)
            {
                startPositionX += length;
            }
            else if (movementX < startPositionX - length)
            {
                startPositionX -= length;
            }
        }
    }

    private bool IsPlayerWithinTrigger(Vector2 playerPosition)
    {
        return playerPosition.x >= minTriggerPointX &&
               playerPosition.x <= maxTriggerPointX &&
               playerPosition.y >= limitY;
    }
}
