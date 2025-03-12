using UnityEngine;


public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private Vector3 offset;
    [SerializeField, Range(0, 1)] private float smoothSpeed = 0.125f;

    private void LateUpdate()
    {
        if (player == null) return; // Protect against null references

        Vector3 desiredPosition = player.position + offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPosition;
    }
}

