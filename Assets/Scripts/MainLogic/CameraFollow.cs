using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform player;       // Reference to the player's transform
    [SerializeField] private Vector3 offset = new Vector3(0, 0, -10); // Offset from the player
    [SerializeField] private float smoothSpeed = 0.125f; // Smoothing speed

    private void LateUpdate()
    {
        if (player != null)
        {
            Vector3 desiredPosition = player.position + offset;
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
            transform.position = smoothedPosition;
        }
    }
}