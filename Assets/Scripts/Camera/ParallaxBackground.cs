using UnityEngine;

public class ParallaxBackground : MonoBehaviour
{
    private float length, startPositionX, startPositionY;
    public GameObject player;
    public float parallaxEffectX;
    public float parallaxEffectY; 

    public float minTriggerPointX;
    public float maxTriggerPointX;

    public float limitY;

    void Start()
    {
        startPositionX = transform.position.x;
        startPositionY = transform.position.y;  
        length = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    void FixedUpdate()
    {
        if (player != null)
        {
            float playerLocationX = player.transform.position.x;
            float playerLocationY = player.transform.position.y;

            if (playerLocationX >= minTriggerPointX && playerLocationX <= maxTriggerPointX && playerLocationY >= limitY)
            {
                float distanceX = playerLocationX * parallaxEffectX;
                float distanceY = playerLocationY * parallaxEffectY;
                float movementX = playerLocationX * (1 - parallaxEffectX);

                transform.position = new Vector3(startPositionX + distanceX, startPositionY + distanceY, transform.position.z);
                
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
    }
}
