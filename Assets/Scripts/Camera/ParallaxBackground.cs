using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;

public class ParallaxBackground : MonoBehaviour
{
    private float length, startposX, startposY;
    public GameObject player;
    public float parallaxEffectX;
    public float parallaxEffectY; // New variable for Y-axis parallax effect

    public float minTriggerPointX;
    public float maxTriggerPointX;

    public float limitY;

    void Start()
    {
        startposX = transform.position.x;
        startposY = transform.position.y; // Store the initial Y position
        length = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    void FixedUpdate()
    {
        if (player != null)
        {
            float playerLocX = player.transform.position.x;
            float playerLocY = player.transform.position.y;

            if (playerLocX >= minTriggerPointX && playerLocX <= maxTriggerPointX && playerLocY >= limitY)
            {
                float distanceX = playerLocX * parallaxEffectX;
                float distanceY = playerLocY * parallaxEffectY;
                float movementX = playerLocX * (1 - parallaxEffectX);

                transform.position = new Vector3(startposX + distanceX, startposY + distanceY, transform.position.z);
                
                if (movementX > startposX + length)
                {
                    startposX += length;
                }
                else if (movementX < startposX - length)
                {
                    startposX -= length;
                }
            }
            else if (playerLocX >= maxTriggerPointX)
            {
                return;
            }
        }
    }
}
