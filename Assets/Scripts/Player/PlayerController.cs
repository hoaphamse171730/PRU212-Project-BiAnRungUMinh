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
    public bool IsDead = false;

    public bool isDead
    {
        get { return IsDead; }
    }

    private Animator animator;
    private Rigidbody2D rb;
    private bool isGrounded = false;
    private SpriteRenderer spriteRenderer;

    private AudioSource footstepAudioSource;
    [Header("Audio Config")]
    [SerializeField] private AudioClip walkingClip;
    [SerializeField] private AudioClip runningClip;
    [SerializeField] private float stepInterval = 0.5f;
    private float stepTimer;

    [Header("Breathing Config")]
    [SerializeField] private AudioClip idleBreathClip;
    [SerializeField] private AudioClip heavyBreathClip;
    [SerializeField] private float breathInterval = 3f;
    private float breathTimer;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        footstepAudioSource = GetComponent<AudioSource>();
        currentHealth = maxHealth;
    }

    private void Update()
    {
        if (IsDead) return;

        HandleMovement();
        HandleJump();
        HandleDamageInput();
        HandleBreathing();
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

        if (Mathf.Abs(moveInput) > 0.1f && isGrounded)
        {
            if (!footstepAudioSource.isPlaying)
            {
                if (isRunning)
                {
                    PlayRunningSound();
                }
                else
                {
                    PlayWalkingSound();
                }
            }
        }
        else
        {
            if (footstepAudioSource.isPlaying)
            {
                footstepAudioSource.Stop();
            }
        }

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
            IsDead = true;
            animator.SetTrigger("Dead");
            rb.linearVelocity = Vector2.zero;
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

    private void PlayWalkingSound()
    {
        if (walkingClip != null && footstepAudioSource != null)
        {
            footstepAudioSource.clip = walkingClip;
            footstepAudioSource.Play();
        }
    }

    private void PlayRunningSound()
    {
        if (runningClip != null && footstepAudioSource != null)
        {
            footstepAudioSource.clip = runningClip;
            footstepAudioSource.Play();
        }
    }

    private void HandleBreathing()
    {
        breathTimer += Time.deltaTime;

        bool isMoving = Mathf.Abs(rb.linearVelocity.x) > 0.1f;
        bool isRunning = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);

        if (!isMoving)
        {
            if (breathTimer >= breathInterval && idleBreathClip != null)
            {
                footstepAudioSource.clip = idleBreathClip;
                footstepAudioSource.Play();
                breathTimer = 0f;
            }
        }
        else if (isRunning)
        {
            if (breathTimer >= breathInterval / 2 && heavyBreathClip != null)
            {
                footstepAudioSource.clip = heavyBreathClip;
                footstepAudioSource.Play();
                breathTimer = 0f;
            }
        }
    }
}