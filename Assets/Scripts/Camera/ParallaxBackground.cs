using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxBackground : MonoBehaviour
{
    private float length, startpos;
    public GameObject cam;
    public float parallaxEffect;
    public PlayerData playerLoc;

    // Store the initial Y position of the background
    private float startY; 

    void Start()
    {
        startpos = transform.position.x;
        startY = transform.position.y; // Store the initial Y
        length = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    void FixedUpdate()
    {
        float distance = cam.transform.position.x * parallaxEffect;
        float movement = cam.transform.position.x * (1 - parallaxEffect);

        // Maintain the original Y position when updating the background's position
        transform.position = new Vector3(startpos + distance, startY, transform.position.z);

        if (movement > startpos + length)
        {
            startpos += length;
        }
        else if (movement < startpos - length)
        {
            startpos -= length;
        }
    }
}