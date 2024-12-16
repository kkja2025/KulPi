using UnityEngine;

public class BoatController : MonoBehaviour
{
    public float moveSpeed = 5f; // Speed of the boat
    private Vector2 movementInput; // Input for movement
    private PlayerInput controls; // Player input reference
    private Rigidbody2D rb; // Rigidbody for movement
    public int maxHP = 20; // Player lives
    public int damage = 0; // Damage taken
    private bool isGameActive = false; // Controls game state

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

    private void FixedUpdate()
    {
        if (!isGameActive) return;

        Vector2 newPosition = rb.position + movementInput * moveSpeed * Time.fixedDeltaTime;
        rb.MovePosition(newPosition);
    }

    public void StartGame()
    {
        isGameActive = true;
    }

    public void StopGame()
    {
        isGameActive = false;
        movementInput = Vector2.zero; // Stop movement
    }

    private void OnMovePerformed(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        movementInput = context.ReadValue<Vector2>();
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
        damage++;
    }
}
