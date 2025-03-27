using UnityEngine;

public class Boss : MonoBehaviour
{
    


    public float speed = 10f;
    public float aggroRange = 10f;
    public Transform player;
    public int damage = 50;
    public float attackCooldown = 2f; // Cooldown time in seconds

    private int direction;
    private bool isChasing = false;
    private bool isReturning = false; // New state for returning
    private BossVisual bossVisual;
    private bool isMoving = true;
    private Vector3 startPosition;
    private float lastAttackTime = 0f;
    public AudioSource audioSource;
    public AudioClip[] soundClips;

    void Start()
    {
        
        startPosition = transform.position; // Store initial position
        bossVisual = GetComponentInChildren<BossVisual>(); // Reference EnemyVisual
        player = null;
        audioSource = GetComponent<AudioSource>();
        audioSource.loop = true;
    }

    void Update()
    {
        HandleAttack();
        if (isMoving) HandleMove();
        PlaySound();
    }

    private void HandleMove()
    {
        Debug.Log("Is moving");
        CheckAggro();

        if (isChasing)
            ChasePlayer();
        else if (isReturning)
            ReturnToStart(); // Move back to start position
        else 
            bossVisual.StopWalk(); // Stop walk animation
    }

    void CheckAggro()
    {
        Collider2D hit = Physics2D.OverlapCircle(transform.position, aggroRange, LayerMask.GetMask("Player"));

        if (hit != null && hit.CompareTag("Player"))
        {
            Debug.Log("Chasing player");
            isChasing = true;
            isReturning = false; // Stop returning if player is found
            player = hit.transform;
        }
        else if (isChasing) // Player left aggro range
        {
            Debug.Log("Lost aggro, returning to start");
            isChasing = false;
            isReturning = true; // Start returning to original position
            player = null;
        }
    }

    void ReturnToStart()
    {
        bossVisual.PlayWalk(); // Play walk animation

        float moveDirection = Mathf.Sign(startPosition.x - transform.position.x); // Determine direction to move
        bossVisual.Flip(-moveDirection); // Flip sprite to face start position

        transform.position = Vector2.MoveTowards(transform.position, startPosition, speed * Time.deltaTime);

        if (Vector2.Distance(transform.position, startPosition) < 0.1f) // If reached start position
        {
            isReturning = false;
            direction = 1;
        }
    }

    void ChasePlayer()
    {
        bossVisual.PlayWalk(); // Call visual script

        float moveDirection = Mathf.Sign(player.position.x - transform.position.x); // Get direction towards player
        bossVisual.Flip(-moveDirection); // Flip sprite

        // Move only on X-axis, keep Y position the same
        Vector2 targetPosition = new Vector2(player.position.x, transform.position.y);
        transform.position = Vector2.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
    }

    private void HandleAttack()
    {
        if (Time.time - lastAttackTime < attackCooldown)
        {
            return; // Exit if still on cooldown
        }

        Collider2D hit = Physics2D.OverlapCircle(transform.position, 3f, LayerMask.GetMask("Player"));

        if (hit != null && hit.CompareTag("Player"))
        {
            isMoving = false;
            Debug.Log("Attacking player!");
            bossVisual.PlayAttack();
            
            // Get the PlayerController component from the hit collider and apply damage.
            PlayerController playerController = hit.GetComponent<PlayerController>();
            if (playerController != null)
            {
                playerController.TakeDamage(damage);
                lastAttackTime = Time.time; // Reset cooldown
            }
        }
        else
        {
            Debug.Log("Stop attacking");
            isMoving = true;
        }
    }

    private void PlaySound()
    {
        if (audioSource.isPlaying) return;
        if (isChasing) audioSource.PlayOneShot(soundClips[0]);
    }
}
