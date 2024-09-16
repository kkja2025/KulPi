using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class PlayerMovement : MonoBehaviour
{
    PlayerInput controls;

    public Rigidbody2D rb;
    private BoxCollider2D boxCollider;
    public Animator anim;
    private SpriteRenderer spriteRenderer;
    bool isGrounded;
    public Transform groundCheck;

    private float direction = 0f;

    [SerializeField]
    public LayerMask groundLayer;

    [SerializeField]
    private float moveSpeed = 5f;

    [SerializeField]
    private float jumpForce = 8f;

    private enum State
    {
        idle,
        running,
        jumping
    }

    void Awake()
    {
        controls = new PlayerInput();
        controls.Enable();

        controls.Land.Move.performed += ctx => 
        {
            direction = ctx.ReadValue<float>();
        };

        controls.Land.Jump.performed += ctx => Jump();
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

    }

    void Update()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.1f, groundLayer);
        rb.velocity = new Vector2(direction * moveSpeed, rb.velocity.y);

        State state;

        if (direction > 0f)
        {

            state = State.running;
            spriteRenderer.flipX = false;
        }
        else if (direction < 0f)
        {
            state = State.running;
            spriteRenderer.flipX = true;
        }
        else
        {
            state = State.idle;
    } 

        if (rb.velocity.y > 0.1f)
        {
            state = State.jumping;
        }
        else if (rb.velocity.y < -0.1f)
        {
            state = State.jumping;
        }

        anim.SetInteger("state", (int)state);
    }

    void Jump()
    {     
        if (isGrounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }
            
    }


    private void OnEnable()
    {
        controls.Enable();
    }

    private void OnDisable()
    {
        controls.Disable();
    } 
}

