
using System;
using UnityEngine;

public class VerticalScrollingCamera : MonoBehaviour
{
    [SerializeField] private float scrollSpeed = 2.0f; // The speed at which the camera scrolls
    [SerializeField] private float acceleration = 0.1f; // Speed increase over time
    [SerializeField] private float minY = -100f; // The minimum Y position the camera can reach

    void Update()
    {
        if (transform.position.y > minY)
        {
            // Move the camera downward
            transform.position += new Vector3(0, -scrollSpeed * Time.deltaTime, 0);

            // Increase the scroll speed over time for difficulty
            scrollSpeed += acceleration * Time.deltaTime;
        }
    }
}
