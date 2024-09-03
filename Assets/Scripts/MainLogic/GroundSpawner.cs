using UnityEngine;
using System.Collections.Generic;

public class GroundSpawner : MonoBehaviour
{
    [SerializeField] private GameObject groundTilePrefab;
    [SerializeField] private float spawnDistanceThreshold = 10f;
    [SerializeField] private int groundTilePoolSize = 10;

    private Queue<GameObject> groundTilePool;
    private GameObject lastSpawnedTile;
    private float groundTileWidth;

    private void Start()
    {
        if (groundTilePrefab == null)
        {
            Debug.LogError("GroundSpawner: Please assign groundTilePrefab in the Inspector!");
            return;
        }

        InitializePool();

        // Calculate the width of a ground tile based on its sprite bounds
        groundTileWidth = groundTilePrefab.GetComponent<SpriteRenderer>().bounds.size.x;

        lastSpawnedTile = GetTileFromPool();
        lastSpawnedTile.transform.position = transform.position;

        for (int i = 0; i < groundTilePoolSize - 1; i++)
        {
            SpawnGroundTile();
        }
    }

    private void Update()
    {
        if (lastSpawnedTile.transform.position.x - transform.position.x < spawnDistanceThreshold)
        {
            SpawnGroundTile();
        }
    }

    private void InitializePool()
    {
        groundTilePool = new Queue<GameObject>();

        for (int i = 0; i < groundTilePoolSize; i++)
        {
            GameObject newTile = Instantiate(groundTilePrefab);
            newTile.SetActive(false);
            groundTilePool.Enqueue(newTile);
        }
    }

    private GameObject GetTileFromPool()
    {
        if (groundTilePool.Count == 0)
        {
            GameObject newTile = Instantiate(groundTilePrefab);
            return newTile;
        }
        else
        {
            GameObject tile = groundTilePool.Dequeue();
            tile.SetActive(true);
            return tile;
        }
    }

    private void SpawnGroundTile()
    {
        Vector3 spawnPos = lastSpawnedTile.transform.position + new Vector3(groundTileWidth, 0f, 0f);
        lastSpawnedTile = GetTileFromPool();
        lastSpawnedTile.transform.position = spawnPos;

        // Return the previous tile to the pool
        if (groundTilePool.Count < groundTilePoolSize)
        {
            groundTilePool.Enqueue(lastSpawnedTile);
        }
        else
        {
            // If the pool is full, destroy the extra tile (optional)
            Destroy(lastSpawnedTile);
        }
    }
}