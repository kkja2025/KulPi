
using System;
using UnityEngine;

public class VerticalScrollingCamera : MonoBehaviour
{
    [SerializeField] private float scrollSpeed = 2.0f; // The speed at which the camera scrolls
    [SerializeField] private float acceleration = 0.1f; // Speed increase over time
    [SerializeField] private float minY = -100f; // The minimum Y position the camera can reach
    private bool isScrolling = false;
    private Camera mainCamera;

    private void Awake()
    {
        mainCamera = Camera.main;
        AdjustCameraSize();
    }

    private void Update()
    {
        if(isScrolling)
        {
            if (transform.position.y > minY)
            {
                // Move the camera downward
                transform.position += new Vector3(0, -scrollSpeed * Time.deltaTime, 0);

                // Increase the scroll speed over time for difficulty
                scrollSpeed += acceleration * Time.deltaTime;
            } else
            {
                isScrolling = false;
            }
        }
        
    }

    public void StartScrolling()
    {
        isScrolling = true;
    }

    private void AdjustCameraSize()
    {
        float aspectRatio = (float)Screen.width / Screen.height;

        // Define your target aspect ratio
        float targetAspect = 16f / 9f; // For example, 16:9

        if (aspectRatio >= targetAspect)
        {
            // Wider than target aspect ratio
            mainCamera.orthographicSize = 5f; // Set to your desired size for wider screens
        }
        else
        {
            // Taller than target aspect ratio
            mainCamera.orthographicSize = 5f * (targetAspect / aspectRatio);
        }
    }
}
