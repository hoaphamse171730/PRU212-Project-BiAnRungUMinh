using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Config")]
    public float moveSpeed = 5f;
    public float jumpForce = 10f;
    public float dampTime = 0.1f; // Smoothing factor for animation transitions

    private Animator animator;
    private Rigidbody2D rb;
    private bool isGrounded = false;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>(); // Get the SpriteRenderer component

    }

    void Update()
    {
        // Get horizontal input
        float moveInput = Input.GetAxis("Horizontal");

        // Update the Rigidbody's velocity for movement
        rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);

        // Update the "Speed" parameter for animation smoothing
        animator.SetFloat("Speed", Mathf.Abs(moveInput), 0f, Time.deltaTime);

        // Flip the sprite based on movement direction:
        if (moveInput > 0)
        {
            spriteRenderer.flipX = false; // Facing right
        }
        else if (moveInput < 0)
        {
            spriteRenderer.flipX = true;  // Facing left
        }

        // Handle jump
        if (Input.GetKeyDown(KeyCode.UpArrow) && isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        }
    }

    // Detect collision with the ground
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }

    // Detect when leaving the ground
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }
}
