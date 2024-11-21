using UnityEngine;
using UnityEngine.UI;

public class FrameAnimatorUI : MonoBehaviour
{
    [SerializeField] private Sprite[] frames; // Add your frames in the Inspector
    [SerializeField] private float frameRate = 0.1f; // Time per frame

    private Image imageComponent; // For UI Image
    private int currentFrame;
    private float timer;

    private void Start()
    {
        imageComponent = GetComponent<Image>(); // Get the Image component
    }

    private void Update()
    {
        timer += Time.deltaTime;

        if (timer >= frameRate)
        {
            timer -= frameRate;
            currentFrame = (currentFrame + 1) % frames.Length; // Loop the frames
            imageComponent.sprite = frames[currentFrame]; // Update the sprite in the Image component
        }
    }
}
