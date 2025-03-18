using UnityEngine;

public class BackgroundFollow : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private float parallaxFactor = 0.5f; // 0 to 1, lower = slower movement
    [SerializeField] private float followSpeed = 0.1f;

    private Vector3 initialPosition;

    void Start()
    {
        initialPosition = transform.position;
    }

    void LateUpdate()
    {
        Vector3 targetPosition = new Vector3(initialPosition.x + (player.position.x * parallaxFactor), initialPosition.y, transform.position.z);
        transform.position = Vector3.Lerp(transform.position, targetPosition, followSpeed * Time.deltaTime);
    }
}