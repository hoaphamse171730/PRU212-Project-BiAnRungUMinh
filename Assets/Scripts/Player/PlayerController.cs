using UnityEngine;
using System.Collections;

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

    [Header("Respawn Settings")]
    [Tooltip("Assign a spawn point (a Transform in the scene with tag 'SpawnPoint')")]
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private float respawnDelay = 5f;

    private Animator animator;
    private Rigidbody2D rb;
    private bool isGrounded = false;
    private SpriteRenderer spriteRenderer;

    // Input variables
    private float moveInput;
    private bool jumpInput;
    private bool isRunning;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        currentHealth = maxHealth;

        // Find spawn point by tag if not assigned in the Inspector.
        if (spawnPoint == null)
        {
            GameObject sp = GameObject.FindGameObjectWithTag("SpawnPoint");
            if (sp != null)
            {
                spawnPoint = sp.transform;
            }
            else
            {
                Debug.LogWarning("Spawn point not found in scene. Please assign spawnPoint manually.");
            }
        }
    }

    private void Update()
    {
        if (isDead) return;

        // Capture movement input
        moveInput = Input.GetAxis("Horizontal");
        isRunning = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);

        if (Input.GetKeyDown(KeyCode.UpArrow) && isGrounded)
        {
            jumpInput = true;
        }

        // Testing damage: press H to take 10 damage or K to take fatal damage.
        if (Input.GetKeyDown(KeyCode.H))
        {
            TakeDamage(10);
        }
        else if (Input.GetKeyDown(KeyCode.K))
        {
            TakeDamage(100);
        }

        // Update animations (idle if no movement).
        float animationSpeed = Mathf.Abs(moveInput) * (isRunning ? runMultiplier : 1f);
        animator.SetFloat("Speed", animationSpeed, dampTime, Time.deltaTime);
        animator.SetBool("isRunning", isRunning && Mathf.Abs(moveInput) > 0.1f);

        // Flip sprite based on movement direction.
        spriteRenderer.flipX = moveInput < 0;
    }

    private void FixedUpdate()
    {
        if (isDead) return;

        float currentSpeed = isRunning ? moveSpeed * runMultiplier : moveSpeed;
        rb.linearVelocity = new Vector2(moveInput * currentSpeed, rb.linearVelocity.y);

        if (jumpInput)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            jumpInput = false;
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
            StartCoroutine(RespawnAfterDelay(respawnDelay));
        }
    }

    private IEnumerator RespawnAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        Respawn();
    }

    private void Respawn()
    {
        // Reposition the player at the spawn point if it exists.
        if (spawnPoint != null)
        {
            transform.position = spawnPoint.position;
        }
        else
        {
            Debug.LogWarning("No spawn point set! Player remains at current position.");
        }

        // Reset player's health and state.
        currentHealth = maxHealth;
        isDead = false;
        rb.linearVelocity = Vector2.zero;
        animator.ResetTrigger("Dead");
        animator.Play("idle");
        // Reset DarknessController light.
        DarknessController dc = FindObjectOfType<DarknessController>();
        if (dc != null)
        {
            dc.ResetLight();
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

    public bool IsDead => isDead;
}
