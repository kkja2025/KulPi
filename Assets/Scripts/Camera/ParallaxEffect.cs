using UnityEngine;

public class ParallaxEffect : MonoBehaviour
{
    public Transform cameraTransform; // Reference to the main camera
    public Vector2 parallaxMultiplier; // Control the intensity of the parallax (X and Y)
    public bool loopHorizontally = true; // Enable/disable horizontal looping

    private Vector3 lastCameraPosition;
    private float textureUnitSizeX;

    private void Start()
    {
        if (cameraTransform == null)
        {
            cameraTransform = Camera.main.transform;
        }

        lastCameraPosition = cameraTransform.position;

        // Calculate the width of the texture in world units
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            textureUnitSizeX = spriteRenderer.bounds.size.x;
        }
    }

    private void LateUpdate()
    {
        Vector3 deltaMovement = cameraTransform.position - lastCameraPosition;

        // Apply parallax effect
        transform.position += new Vector3(
            deltaMovement.x * parallaxMultiplier.x,
            deltaMovement.y * parallaxMultiplier.y,
            0
        );

        lastCameraPosition = cameraTransform.position;

        // Handle horizontal looping
        if (loopHorizontally)
        {
            float cameraXPosition = cameraTransform.position.x;
            float objectXPosition = transform.position.x;

            if (Mathf.Abs(cameraXPosition - objectXPosition) >= textureUnitSizeX)
            {
                float offsetX = (cameraXPosition - objectXPosition) % textureUnitSizeX;
                transform.position = new Vector3(cameraXPosition + offsetX, transform.position.y, transform.position.z);
            }
        }
    }

    private void OnValidate()
    {
        if (parallaxMultiplier == Vector2.zero)
        {
            parallaxMultiplier = new Vector2(0.5f, 0.5f); // Default value
        }
    }
}
