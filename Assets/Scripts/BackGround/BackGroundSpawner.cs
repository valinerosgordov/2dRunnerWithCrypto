using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class BuildingSpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] buildingPrefabs; // Array of building prefabs to spawn
    [SerializeField] private float spawnInterval = 5.0f;   // Time between spawns
    [SerializeField] private float buildingSpeed = 3.0f;   // Speed at which buildings move
    [SerializeField] private Transform spawnPoint;         // Spawn position (for X coordinate)
    [SerializeField] private float minY = -1f;             // Minimum Y position for random spawning
    [SerializeField] private float maxY = 1f;              // Maximum Y position for random spawning
    [SerializeField] private float despawnX = -10f;        // X position at which buildings are destroyed (off-screen)

    private float timer;

    private void Start()
    {
        timer = spawnInterval;
    }

    private void Update()
    {
        timer -= Time.deltaTime;

        if (timer <= 0)
        {
            SpawnBuilding();
            timer = spawnInterval;
        }
    }

    private void SpawnBuilding()
    {
        // Select a random building prefab to spawn
        int randomIndex = Random.Range(0, buildingPrefabs.Length);
        GameObject building = Instantiate(buildingPrefabs[randomIndex]);

        // Randomize Y position between minY and maxY
        float randomY = Random.Range(minY, maxY);
        Vector3 spawnPosition = new Vector3(spawnPoint.position.x, randomY, spawnPoint.position.z);

        // Set building's position
        building.transform.position = spawnPosition;

        // Assign a speed to the building
        Rigidbody2D rb = building.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.velocity = new Vector2(-buildingSpeed, 0); // Move the building left
        }
        else
        {
            Debug.LogWarning("Building prefab is missing a Rigidbody2D component.");
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Building"))
        {
            if (other.transform.position.x < despawnX)
            {
                Destroy(other.gameObject);
            }
        }
    }
}
