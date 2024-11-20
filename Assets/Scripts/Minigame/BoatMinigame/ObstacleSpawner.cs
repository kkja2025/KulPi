using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    public GameObject obstaclePrefab;    // The obstacle prefab
    public float initialSpawnInterval = 2f;  // Base interval between spawns
    public float randomIntervalOffset = 1f;  // Randomness in spawn timing
    public float initialObstacleSpeed = 3f;  // Starting speed of obstacles
    public float speedIncreaseInterval = 10f; // Time in seconds to increase speed
    public float speedIncrement = 0.5f;       // Amount to increase speed

    private float currentSpawnInterval;
    private float currentObstacleSpeed;
    private float spawnTimer = 0f;
    private float speedTimer = 0f;

    private void Start()
    {
        // Initialize speed and spawn interval
        currentSpawnInterval = initialSpawnInterval;
        currentObstacleSpeed = initialObstacleSpeed;
    }

    private void Update()
    {
        spawnTimer += Time.deltaTime;
        speedTimer += Time.deltaTime;

        // Spawn obstacles at intervals
        if (spawnTimer >= currentSpawnInterval + Random.Range(0f, randomIntervalOffset))
        {
            SpawnObstacle();
            spawnTimer = 0f;
        }

        // Increase speed periodically
        if (speedTimer >= speedIncreaseInterval)
        {
            currentObstacleSpeed += speedIncrement;
            speedTimer = 0f;
            Debug.Log($"Obstacle speed increased to: {currentObstacleSpeed}");
        }
    }

    void SpawnObstacle()
    {
        // Spawn the obstacle at the spawner's position
        GameObject obstacle = Instantiate(obstaclePrefab, transform.position, Quaternion.identity);

        // Add a downward velocity to the obstacle
        Rigidbody2D rb = obstacle.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.velocity = Vector2.down * currentObstacleSpeed;
        }
    }
}
