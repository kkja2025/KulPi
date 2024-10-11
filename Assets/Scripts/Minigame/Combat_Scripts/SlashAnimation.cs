using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SlashAnimation : MonoBehaviour
{
    [SerializeField] private Image overlayImage;  // Reference to the overlay image component
    public Sprite[] animationFrames;  // Array of sprites for the animation
    public float animationSpeed = 0.01f;  // Time between frames
    public bool loop = false;  // Set to true if you want the animation to loop

    private int currentFrame = 0;
    private bool isAnimating = false;

    void Start()
    {
        // Ensure overlay image is hidden or set to the first frame initially
        if (overlayImage != null && animationFrames.Length > 0)
        {
            overlayImage.sprite = animationFrames[0];
            overlayImage.enabled = false;  // Initially hide the overlay image
        }
    }

    public void StartOverlayAnimation()
    {
        if (!isAnimating)
        {
            overlayImage.enabled = true;  // Show the overlay image
            StartCoroutine(AnimateOverlay());
        }
    }

    public void StopOverlayAnimation()
    {
        isAnimating = false;
        StopAllCoroutines();
        overlayImage.enabled = false;  // Hide the overlay image
    }

    private IEnumerator AnimateOverlay()
    {
        isAnimating = true;

        while (isAnimating)
        {
            // Change the overlay sprite to the next frame
            overlayImage.sprite = animationFrames[currentFrame];
            currentFrame = (currentFrame + 1) % animationFrames.Length;

            // Wait for the next frame
            yield return new WaitForSeconds(animationSpeed);

            // Exit the loop if not set to loop
            if (!loop && currentFrame == 0)
            {
                StopOverlayAnimation();
            }
        }
    }
}
