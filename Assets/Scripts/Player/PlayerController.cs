using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

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

    [Header("Respawn Settings")]
    [Tooltip("Assign a spawn point (a Transform in the scene with tag 'SpawnPoint')")]
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private float respawnDelay = 5f;

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
    // Input variables
    private float moveInput;
    private bool jumpInput;
    private bool isRunning;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        footstepAudioSource = GetComponent<AudioSource>();
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
        if (IsDead) return;

        HandleMovement();
        HandleJump();
        HandleBreathing();
    }

    private void HandleMovement()
    {
        moveInput = Input.GetAxis("Horizontal");
        isRunning = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
        float currentSpeed = isRunning ? moveSpeed * runMultiplier : moveSpeed;

        float animationSpeed = Mathf.Abs(moveInput) * (isRunning ? runMultiplier : 1f);
        animator.SetFloat("Speed", animationSpeed, dampTime, Time.deltaTime);
        animator.SetBool("isRunning", isRunning && Mathf.Abs(moveInput) > 0.1f);

        if (Mathf.Abs(moveInput) > 0.1f && isGrounded)
        {
            if (!footstepAudioSource.isPlaying)
            {
                if (isRunning)
                    PlayRunningSound();
                else
                    PlayWalkingSound();
            }
        }
        else if (footstepAudioSource.isPlaying)
        {
            footstepAudioSource.Stop();
        }

        spriteRenderer.flipX = moveInput < 0;
    }



    private void HandleJump()
    {
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
            IsDead = true;
            animator.SetTrigger("Dead");

            GameSession.LastScene = SceneManager.GetActiveScene().name;

            ClearPersistentManagers();

            rb.linearVelocity = Vector2.zero;

            SceneManager.LoadScene("DeadScene");
        }
    }


    private IEnumerator RespawnAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        Respawn();
    }

    private void Respawn()
    {
        if (spawnPoint != null)
        {
            transform.position = spawnPoint.position;
        }
        else
        {
            Debug.LogWarning("No spawn point set! Player remains at current position.");
        }

        currentHealth = maxHealth;
        IsDead = false;
        rb.linearVelocity = Vector2.zero;
        animator.ResetTrigger("Dead");
        animator.Play("idle");
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

    private void ClearPersistentManagers()
    {
        var dialogueManager = FindObjectOfType<DialogueManager>();
        if (dialogueManager != null)
        {
            Destroy(dialogueManager.gameObject);
        }

        var decisionManager = FindObjectOfType<DecisionManager>();
        if (decisionManager != null)
        {
            Destroy(decisionManager.gameObject);
        }

        var uiManager = FindObjectOfType<NotesUI>();
        if (uiManager != null)
        {
            Destroy(uiManager.gameObject);
        }

        var noteManager = FindObjectOfType<NotesManager>();
        if (noteManager != null)
        {
            Destroy(noteManager.gameObject);
        }
    }


}
