using UnityEngine;

public class ParallaxBackground : MonoBehaviour
{
    private float length, startPositionX, startPositionY;
    private Vector2 previousCamPos;
    private Transform cam;

    public GameObject player; 
    public float parallaxEffectX = 0.5f; 
    public float parallaxEffectY = 0.5f; 

    public float smoothingX = 1f; 
    public float smoothingY = 1f;

    public float minTriggerPointX = -10f;
    public float maxTriggerPointX = 10f;
    public float limitY = 0f;

    void Awake()
    {
        cam = Camera.main.transform;
    }

    void Start()
    {
        startPositionX = transform.position.x;
        startPositionY = transform.position.y;
        length = GetComponent<SpriteRenderer>().bounds.size.x;
        previousCamPos = cam.position;
    }

    void FixedUpdate()
    {
        ApplyPlayerParallax();
        ApplyCameraParallax();
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

    private void ApplyCameraParallax()
    {
        Vector2 camPos = cam.position;
        float parallaxX = (previousCamPos.x - camPos.x) * parallaxEffectX;
        float parallaxY = (previousCamPos.y - camPos.y) * parallaxEffectY;

        Vector2 backgroundTargetPos = new Vector2(
            transform.position.x + parallaxX,
            transform.position.y + parallaxY
        );

        Vector2 currentPos = transform.position;
        transform.position = Vector2.Lerp(currentPos, backgroundTargetPos, Mathf.Max(smoothingX, smoothingY) * Time.deltaTime);

        previousCamPos = camPos;
    }

    private bool IsPlayerWithinTrigger(Vector2 playerPosition)
    {
        return playerPosition.x >= minTriggerPointX &&
               playerPosition.x <= maxTriggerPointX &&
               playerPosition.y >= limitY;
    }
}
