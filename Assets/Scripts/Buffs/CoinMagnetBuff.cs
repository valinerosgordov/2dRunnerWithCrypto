using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinMagnetBuff : MonoBehaviour
{
    public float duration = 10.0f;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerController playerController = other.GetComponent<PlayerController>();
            if (playerController != null)
            {
                playerController.ActivateCoinMagnet(duration);
                Destroy(gameObject); // Clean up the buff object
            }
        }
    }
}