using UnityEngine;
using System.Collections.Generic;

public class SpawnManager : MonoBehaviour
{
    [Header("Obstacles")]
    [SerializeField] private GameObject[] obstaclePrefabs;
    [SerializeField] private float obstacleSpawnInterval = 2f;

    [Header("Coins")]
    [SerializeField] private GameObject coinPrefab;
    [SerializeField] private float coinSpawnInterval = 3f;

    [Header("Power-Ups")]
    [SerializeField] private GameObject[] powerUpPrefabs;
    [SerializeField] private float powerUpSpawnInterval = 5f;

    [Header("Player Reference")]
    [SerializeField] private Transform player;

    [Header("Spawn Offsets")]
    [SerializeField] private float spawnDistanceFromPlayer = 10f;
    [SerializeField] private Vector3 spawnOffset = Vector3.forward; // Default forward offset

    private float obstacleSpawnTimer = 0f;
    private float coinSpawnTimer = 0f;
    private float powerUpSpawnTimer = 0f;

    // Patterns for spawning coins and obstacles
    private List<Vector3[]> coinPatterns;
    private List<Vector3[]> obstaclePatterns;

    private void Start()
    {
        ValidateInspectorInputs();
        InitializePatterns();
    }

    private void Update()
    {
        if (player == null)
        {
            Debug.LogWarning("SpawnManager: Player Transform is not assigned!");
            return;
        }

        // Increment timers
        obstacleSpawnTimer += Time.deltaTime;
        coinSpawnTimer += Time.deltaTime;
        powerUpSpawnTimer += Time.deltaTime;

        // Check if it's time to spawn obstacles
        if (obstacleSpawnTimer >= obstacleSpawnInterval)
        {
            SpawnObstaclePattern();
            obstacleSpawnTimer = 0f;
        }

        // Check if it's time to spawn coins
        if (coinSpawnTimer >= coinSpawnInterval)
        {
            SpawnCoinPattern();
            coinSpawnTimer = 0f;
        }

        // Check if it's time to spawn power-ups
        if (powerUpSpawnTimer >= powerUpSpawnInterval)
        {
            SpawnPowerUp();
            powerUpSpawnTimer = 0f;
        }
    }

    private void ValidateInspectorInputs()
    {
        if (obstaclePrefabs == null || obstaclePrefabs.Length == 0)
        {
            Debug.LogWarning("Obstacle prefabs array is empty or null.");
        }

        if (coinPrefab == null)
        {
            Debug.LogWarning("Coin prefab is null.");
        }

        if (powerUpPrefabs == null || powerUpPrefabs.Length == 0)
        {
            Debug.LogWarning("Power-up prefabs array is empty or null.");
        }

        if (player == null)
        {
            Debug.LogWarning("Player Transform is not assigned.");
        }
    }

    private void InitializePatterns()
    {
        // Initialize coin patterns
        coinPatterns = new List<Vector3[]>
        {
            new Vector3[] { Vector3.zero, Vector3.right, Vector3.right * 2, Vector3.right * 3, Vector3.right * 4 },
            new Vector3[] { Vector3.zero, Vector3.up, Vector3.up * 2, Vector3.up * 3, Vector3.up * 4 },
            new Vector3[] { Vector3.zero, Vector3.right + Vector3.up, (Vector3.right + Vector3.up) * 2, (Vector3.right + Vector3.up) * 3, (Vector3.right + Vector3.up) * 4 }
        };

        // Initialize obstacle patterns
        obstaclePatterns = new List<Vector3[]>
        {
            new Vector3[] { Vector3.zero, Vector3.right * 2, Vector3.right * 4 },
            new Vector3[] { Vector3.zero, Vector3.up * 2, Vector3.up * 4 },
            new Vector3[] { Vector3.zero, Vector3.right * 2, Vector3.up * 2 }
        };
    }

    /// <summary>
    /// Spawns a random obstacle pattern at a position relative to the player.
    /// </summary>
    private void SpawnObstaclePattern()
    {
        if (obstaclePrefabs == null || obstaclePrefabs.Length == 0)
        {
            return;
        }

        Vector3[] pattern = obstaclePatterns[Random.Range(0, obstaclePatterns.Count)];
        Vector3 spawnPosition = player.position + spawnOffset * spawnDistanceFromPlayer;

        foreach (Vector3 offset in pattern)
        {
            GameObject prefabToSpawn = obstaclePrefabs[Random.Range(0, obstaclePrefabs.Length)];
            Instantiate(prefabToSpawn, spawnPosition + offset, Quaternion.identity);
        }
    }

    /// <summary>
    /// Spawns a random coin pattern at a position relative to the player.
    /// </summary>
    private void SpawnCoinPattern()
    {
        if (coinPrefab == null)
        {
            return;
        }

        Vector3[] pattern = coinPatterns[Random.Range(0, coinPatterns.Count)];
        Vector3 spawnPosition = player.position + spawnOffset * spawnDistanceFromPlayer;

        foreach (Vector3 offset in pattern)
        {
            Instantiate(coinPrefab, spawnPosition + offset, Quaternion.identity);
        }
    }

    /// <summary>
    /// Spawns a random power-up from the power-up prefabs at a position relative to the player.
    /// </summary>
    private void SpawnPowerUp()
    {
        if (powerUpPrefabs == null || powerUpPrefabs.Length == 0)
        {
            return;
        }

        GameObject prefabToSpawn = powerUpPrefabs[Random.Range(0, powerUpPrefabs.Length)];
        Vector3 spawnPosition = player.position + spawnOffset * spawnDistanceFromPlayer;

        Instantiate(prefabToSpawn, spawnPosition, Quaternion.identity);
    }
}