using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Config")]
    public float moveSpeed = 5f;
    public float jumpForce = 6.5f;
    public float dampTime = 0.1f; // Smoothing factor for animation transitions
    public float runMultiplier = 1.5f; // Multiplier for running speed
    
    [Header("Health Config")]
    public int maxHealth = 100;
    private int currentHealth;
    private bool isDead = false;

    private Animator animator;
    private Rigidbody2D rb;
    private bool isGrounded = false;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>(); // Get the SpriteRenderer component
        currentHealth = maxHealth;
    }

    void Update()
    {
        // Prevent any movement if the character is dead.
        if (isDead)
            return;

        // Get horizontal input
        float moveInput = Input.GetAxis("Horizontal");

        // Check if the Shift key is held down to run
        bool isRunning = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);

        // Set the current speed based on whether running or walking
        float currentSpeed = isRunning ? moveSpeed * runMultiplier : moveSpeed;

        // Update the Rigidbody's velocity for movement
        rb.linearVelocity = new Vector2(moveInput * currentSpeed, rb.linearVelocity.y);

        // Calculate animation speed (this can be used to blend animations)
        float animationSpeed = Mathf.Abs(moveInput) * (isRunning ? runMultiplier : 1f);
        animator.SetFloat("Speed", animationSpeed, dampTime, Time.deltaTime);

        // Set the running boolean parameter for your Animator
        animator.SetBool("isRunning", isRunning && Mathf.Abs(moveInput) > 0.1f);

        // Flip the sprite based on movement direction
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

        if (Input.GetKeyDown(KeyCode.H))
        {
            TakeDamage(10);
        }

        if (Input.GetKeyDown(KeyCode.K))
        {
            TakeDamage(100);
        }

    }

    // Call this method to apply damage to the character.
    public void TakeDamage(int damage)
    {
        if (isDead)
            return;

        currentHealth -= damage;

        if (currentHealth > 0)
        {
            // Trigger the hurt animation
            animator.SetTrigger("Hurt");
        }
        else
        {
            // The character is dead.
            isDead = true;
            animator.SetTrigger("Dead");
            // Optionally, you can disable further physics/movement:
            rb.linearVelocity = Vector2.zero;
            // And disable the collider or any other components if necessary
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
