using UnityEngine;


public class PlayerMovement : MonoBehaviour
{
    PlayerInput controls;

    public Rigidbody2D rb;
    private BoxCollider2D boxCollider;
    public Animator anim;
    private SpriteRenderer spriteRenderer;
    public bool isGrounded;
    public bool isMoving;
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
        jumping,
        falling
    }
    private State state;

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
        isMoving = Mathf.Abs(direction) > 0f;
        rb.velocity = new Vector2(direction * moveSpeed, rb.velocity.y);

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
            state = State.falling;
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

    public void OnEnable()
    {
        controls.Enable();
    }

    public void OnDisable()
    {
        controls.Disable();
    }
}

