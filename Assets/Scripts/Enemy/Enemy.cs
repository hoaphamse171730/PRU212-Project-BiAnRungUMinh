using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed = 2f;
    public float roamDistance = 3f;
    public float aggroRange = 5f;
    public Transform player;
    public int damage = 35;
    public float attackCooldown = 0.5f; // Cooldown time in seconds

    private float startX;
    private int direction = 1;
    private bool isChasing = false;
    private bool isReturning = false; // New state for returning
    private EnemyVisual enemyVisual;
    private bool isMoving = true;
    private Vector3 startPosition;
    private float lastAttackTime = 0f;
    

    void Start()
    {
        startX = transform.position.x;
        startPosition = transform.position; // Store initial position
        enemyVisual = GetComponentInChildren<EnemyVisual>(); // Reference EnemyVisual
        player = null;
    }

    void Update()
    {
        HandleAttack();
        if (isMoving) HandleMove();
    }

    private void HandleMove()
    {
        CheckAggro();

        if (isChasing)
        {
            speed = 5f; //run speed
            ChasePlayer();
        }
            
        else if (isReturning)
            ReturnToStart(); // Move back to start position
        else
        {
            speed = 2f;//roam speed
            Roam();
        }
            
    }

    void Roam()
    {
        enemyVisual.PlayWalk(); // Call visual script
        enemyVisual.Flip(direction); // Flip sprite based on direction

        transform.position += new Vector3(speed * direction * Time.deltaTime, 0, 0);

        if (Mathf.Abs(transform.position.x - startX) > roamDistance)
        {
            direction *= -1;
        }
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
        enemyVisual.PlayWalk(); // Play walk animation

        float moveDirection = Mathf.Sign(startPosition.x - transform.position.x); // Determine direction to move
        enemyVisual.Flip(moveDirection); // Flip sprite to face start position

        transform.position = Vector2.MoveTowards(transform.position, startPosition, speed * Time.deltaTime);

        if (Vector2.Distance(transform.position, startPosition) < 0.1f) // If reached start position
        {
            isReturning = false;
            direction = 1; // Reset direction for roaming
        }
    }

    void ChasePlayer()
    {
        enemyVisual.PlayRun(); // Call visual script

        float moveDirection = Mathf.Sign(player.position.x - transform.position.x); // Get direction towards player
        enemyVisual.Flip(moveDirection); // Flip sprite

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

        Collider2D hit = Physics2D.OverlapCircle(transform.position, 1f, LayerMask.GetMask("Player"));

        if (hit != null && hit.CompareTag("Player"))
        {
            isMoving = false;
            Debug.Log("Attacking player!");
            enemyVisual.PlayAttack();

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

}
