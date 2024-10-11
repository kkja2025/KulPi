
using System;
using UnityEngine;

public class VerticalScrollingCamera : MonoBehaviour
{
    [SerializeField] private float scrollSpeed = 2.0f;
    [SerializeField] private float acceleration = 0.1f; 
    [SerializeField] private float minY = -100f; 
    private bool isScrolling = false;

    private void Update()
    {
        if(isScrolling)
        {
            if (transform.position.y > minY)
            {
                transform.position += new Vector3(0, -scrollSpeed * Time.deltaTime, 0);

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
}
