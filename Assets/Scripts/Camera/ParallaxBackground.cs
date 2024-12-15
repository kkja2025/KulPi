using UnityEngine;

public class ParallaxBackground : MonoBehaviour
{
    private float length, startPositionX, startPositionY;
    private Transform cam; 
    private Vector3 previousCamPos;

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

        Vector3 playerPosition = player.transform.position;

        if (IsPlayerWithinTrigger(playerPosition))
        {
            float distanceX = playerPosition.x * parallaxEffectX;
            float distanceY = playerPosition.y * parallaxEffectY;

            transform.position = new Vector3(startPositionX + distanceX, startPositionY + distanceY, transform.position.z);

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
        float parallaxX = (previousCamPos.x - cam.position.x) * parallaxEffectX;
        float parallaxY = (previousCamPos.y - cam.position.y) * parallaxEffectY;

        Vector3 backgroundTargetPos = new Vector3(
            transform.position.x + parallaxX,
            transform.position.y + parallaxY,
            transform.position.z
        );

        transform.position = Vector3.Lerp(transform.position, backgroundTargetPos, Mathf.Max(smoothingX, smoothingY) * Time.deltaTime);

        previousCamPos = cam.position;
    }

    private bool IsPlayerWithinTrigger(Vector3 playerPosition)
    {
        return playerPosition.x >= minTriggerPointX &&
               playerPosition.x <= maxTriggerPointX &&
               playerPosition.y >= limitY;
    }
}
