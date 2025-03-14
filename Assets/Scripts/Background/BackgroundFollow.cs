using UnityEngine;

public class BackgroundFollow : MonoBehaviour
{
    [SerializeField] private Transform player; // Reference to the player's transform
    [SerializeField] private float followSpeed = 0.1f; // How fast the background follows (0 to 1, lower is slower)
    [SerializeField] private Vector3 offset; // Offset between background and player

    private Vector3 initialPosition;

    void Start()
    {
        // Store the initial position of the background
        initialPosition = transform.position;

        // If the offset isn't set, calculate it based on the starting positions
        if (offset == Vector3.zero)
        {
            offset = transform.position - player.position;
        }
    }

    void LateUpdate()
    {
        // Calculate the target position (player's position + offset)
        Vector3 targetPosition = player.position + offset;

        // Keep the background's Z position unchanged (important for 2D sorting)
        targetPosition.z = transform.position.z;

        // Smoothly move the background toward the target position
        transform.position = Vector3.Lerp(transform.position, targetPosition, followSpeed * Time.deltaTime);
    }
}