using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerMovement : MonoBehaviour
{
    public Rigidbody2D rb;
    [Header ("Movement")]
    public float moveSpeed = 5f;
    float horizontalMovement;

    [Header ("Jumping")]
    public float jumpForce = 5f;

    [Header ("Ground Check")]
    public Transform groundCheckPosition;
    public Vector2 groundCheckSize = new Vector2 (0.5f, 0.05f);
    public LayerMask groundLayer;

 
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        rb.velocity = new Vector2(horizontalMovement * moveSpeed, rb.velocity.y);
    }


    public void Move(InputAction.CallbackContext context)
    {
        horizontalMovement = context.ReadValue<Vector2>().x;
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (isGrounded())
        {
            if (context.performed)
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            }
            else if (context.canceled)
            {
                rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
            }
        }
    }

    private bool isGrounded()
    {
        if (Physics2D.OverlapBox(groundCheckPosition.position, groundCheckSize, 0, groundLayer))
        { 
            return true;
        }
        return false;
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawCube(groundCheckPosition.position, groundCheckSize);
    }
}
