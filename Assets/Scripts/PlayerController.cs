using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Config")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float jumpForce = 6.5f;
    [SerializeField] private float dampTime = 0.1f;
    [SerializeField] private float runMultiplier = 1.5f;

    [Header("Health Config")]
    [SerializeField] private int maxHealth = 100;
    private int currentHealth;
    private bool isDead = false;

    private Animator animator;
    private Rigidbody2D rb;
    private bool isGrounded = false;
    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        currentHealth = maxHealth;
    }

    private void Update()
    {
        if (isDead) return;

        HandleMovement();
        HandleJump();
        HandleDamageInput();
    }

    private void HandleMovement()
    {
        float moveInput = Input.GetAxis("Horizontal");
        bool isRunning = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
        float currentSpeed = isRunning ? moveSpeed * runMultiplier : moveSpeed;

        rb.linearVelocity = new Vector2(moveInput * currentSpeed, rb.linearVelocity.y);

        float animationSpeed = Mathf.Abs(moveInput) * (isRunning ? runMultiplier : 1f);
        animator.SetFloat("Speed", animationSpeed, dampTime, Time.deltaTime);
        animator.SetBool("isRunning", isRunning && Mathf.Abs(moveInput) > 0.1f);

        // Flip sprite based on direction
        spriteRenderer.flipX = moveInput < 0;
    }

    private void HandleJump()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow) && isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        }
    }

    private void HandleDamageInput()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            TakeDamage(10);
        }
        else if (Input.GetKeyDown(KeyCode.K))
        {
            TakeDamage(100);
        }
    }

    public void TakeDamage(int damage)
    {
        if (isDead) return;

        currentHealth -= damage;

        if (currentHealth > 0)
        {
            animator.SetTrigger("Hurt");
        }
        else
        {
            isDead = true;
            animator.SetTrigger("Dead");
            rb.linearVelocity = Vector2.zero;
            // Additional logic on death (e.g., disabling components) can be added here.
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }
}
