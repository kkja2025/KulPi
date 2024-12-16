using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class RhythmController : MonoBehaviour
{
    [Header("Prefabs and References")]
    public GameObject blueNotePrefab;
    public GameObject pinkNotePrefab;
    public Transform spawnPoint;
    public Transform hitZone;

    [Header("Drum Sounds")]
    public AudioSource blueDrumSound;
    public AudioSource pinkDrumSound;

    [Header("Gameplay Settings")]
    public float perfectThreshold = 0.1f;
    public float goodThreshold = 0.3f;
    public float noteSpeed = 5.0f;
    public float spawnInterval = 1.0f;

    private float spawnTimer = 0f;

    private PlayerInput controls;

    [Header("Game End Settings")]
    public int perfectsToWin = 30; // Number of perfect hits required to end the game
    private int perfectHitCount = 0; // Counter for perfect hits
    [SerializeField] private TMP_Text perfectHitCounterText;

    private void Awake()
    {
        controls = new PlayerInput();
    }

    private void UpdateCounterDisplay()
    {
        if (perfectHitCounterText != null)
        {
            perfectHitCounterText.text = $"Perfect Hits: {perfectHitCount} / {perfectsToWin}";
        }
    }

    private void OnEnable()
    {
        // Subscribe to input actions
        controls.Land.BlueButton.performed += _ => HitNote("BlueNote");
        controls.Land.PinkButton.performed += _ => HitNote("PinkNote");
        controls.Enable();
    }

    private void OnDisable()
    {
        // Unsubscribe from input actions to avoid errors
        controls.Land.BlueButton.performed -= _ => HitNote("BlueNote");
        controls.Land.PinkButton.performed -= _ => HitNote("PinkNote");
        controls.Disable();
    }

    private void Update()
    {
        UpdateCounterDisplay();
        // Spawn notes at regular intervals
        spawnTimer += Time.deltaTime;
        if (spawnTimer >= spawnInterval)
        {
            SpawnNote();
            spawnTimer = 0f;
        }
    }

    void SpawnNote()
    {
        // Randomly decide which note to spawn
        bool isBlue = Random.Range(0, 2) == 0;
        GameObject note = Instantiate(
            isBlue ? blueNotePrefab : pinkNotePrefab,
            spawnPoint.position,
            Quaternion.identity
        );

        // Set note's velocity
        Rigidbody2D rb = note.GetComponent<Rigidbody2D>();
        rb.velocity = Vector2.left * noteSpeed;

        // Assign tag for easy detection
        note.tag = isBlue ? "BlueNote" : "PinkNote";
    }

    void HitNote(string noteTag)
    {
        // Play drum sound based on note type
        if (noteTag == "BlueNote")
            blueDrumSound.Play();
        else if (noteTag == "PinkNote")
            pinkDrumSound.Play();

        // Find the closest note of the corresponding tag
        GameObject closestNote = FindClosestNoteWithTag(noteTag);

        if (closestNote != null)
        {
            float distance = Mathf.Abs(closestNote.transform.position.x - hitZone.position.x);

            if (distance < perfectThreshold)
            {
                Debug.Log("Perfect!");
                perfectHitCount++; // Increment the perfect hit counter
                CheckForGameEnd(); // Check if the game should end
                Destroy(closestNote);
            }
            else if (distance < goodThreshold)
            {
                Debug.Log("Good!");
                Destroy(closestNote);
            }
            else
            {
                Debug.Log("Miss!");
            }
        }
        else
        {
            Debug.Log("Miss!");
        }
    }

    GameObject FindClosestNoteWithTag(string noteTag)
    {
        GameObject[] notes = GameObject.FindGameObjectsWithTag(noteTag);
        GameObject closestNote = null;
        float closestDistance = Mathf.Infinity;

        foreach (GameObject note in notes)
        {
            float distance = Mathf.Abs(note.transform.position.x - hitZone.position.x);

            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestNote = note;
            }
        }

        if (closestDistance <= goodThreshold)
        {
            return closestNote;
        }

        return null;
    }

    void CheckForGameEnd()
    {
        if (perfectHitCount >= perfectsToWin)
        {
            Debug.Log("Game Over! You Win!");
            UpdateCounterDisplay();
            EndGame();
        }
    }

    void EndGame()
    {
        
        Debug.Log("Stopping game...");
        enabled = false; 
        RhythmManager.Singleton.ShowVictoryMenu();
    }
}
