using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedBoostBuff : MonoBehaviour
{
    public float speedMultiplier = 2.0f;
    public float duration = 5.0f;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerController playerController = other.GetComponent<PlayerController>();
            if (playerController != null)
            {
                playerController.ActivateSpeedBoost(speedMultiplier, duration);
                Destroy(gameObject); // Clean up the buff object
            }
        }
    }
}