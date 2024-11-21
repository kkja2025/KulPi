using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // For scene reloading

public class BoatController : MonoBehaviour
{
    public float moveSpeed = 5f;           // Speed of the boat
    private Vector2 movementInput;        // Input for movement
    private PlayerInput controls;         // Player input reference
    private Rigidbody2D rb;               // Rigidbody for movement

    public int lives = 3;                 // Player lives
    private float survivalTime = 0f;      // Time survived by the player
    private bool isGameActive = true;     // Controls game state

    private void Awake()
    {
        controls = new PlayerInput();
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnEnable()
    {
        controls.Gameplay.Enable();
        controls.Gameplay.Move.performed += OnMovePerformed;
        controls.Gameplay.Move.canceled += OnMoveCanceled;
    }

    private void OnDisable()
    {
        controls.Gameplay.Move.performed -= OnMovePerformed;
        controls.Gameplay.Move.canceled -= OnMoveCanceled;
        controls.Gameplay.Disable();
    }

    private void Update()
    {
        if (isGameActive)
        {
            // Increment survival time
            survivalTime += Time.deltaTime;

            // Check win condition
            if (survivalTime >= 60f) // 1 minute
            {
                WinGame();
            }
        }
    }

    private void FixedUpdate()
    {
        if (!isGameActive) return; // Prevent movement when game is over

        Vector2 newPosition = rb.position + movementInput * moveSpeed * Time.fixedDeltaTime;
        rb.MovePosition(newPosition);
    }

    private void OnMovePerformed(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        movementInput = context.ReadValue<Vector2>();
        Debug.Log("Move input: " + movementInput);
    }

    private void OnMoveCanceled(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        movementInput = Vector2.zero;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isGameActive && collision.gameObject.CompareTag("Log"))
        {
            HandleLogCollision(collision.gameObject);
        }
    }

    private void HandleLogCollision(GameObject log)
    {
        // Reduce player lives
        lives--;

        Debug.Log("Lives remaining: " + lives);

        // Destroy the log
        Destroy(log);

        // Check for game over
        if (lives <= 0)
        {
            Debug.Log("Game Over!");
            EndGame(false); // Player loses
        }
    }

    private void WinGame()
    {
        Debug.Log("You Win!");
        EndGame(true); // Player wins
    }

    private void EndGame(bool hasWon)
    {
        isGameActive = false; // Stop the game

        if (hasWon)
        {
            Debug.Log("Game stopped: You survived 1 minute!");
        }
        else
        {
            Debug.Log("Game stopped: You lost all your lives.");
        }

        // Restart the game after a delay
        StartCoroutine(RestartGame());
    }

    private IEnumerator RestartGame()
    {
        Debug.Log("Restarting game...");
        yield return new WaitForSeconds(2f); // Wait for 2 seconds before restarting
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); // Reload current scene
    }
}
