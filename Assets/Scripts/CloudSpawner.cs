using System.Collections;

using UnityEngine;

public class CloudSpawner : MonoBehaviour
{
    public GameObject[] cloudPrefabs; // Array to hold different cloud prefabs
    public float spawnInterval = 5f; // Time interval between spawns
    public float minY = -2f; // Minimum y position for spawning
    public float maxY = 2f; // Maximum y position for spawning
    public float cloudSpeed = 2f; // Speed at which clouds move

    private void Start()
    {
        // Start spawning clouds
        StartCoroutine(SpawnClouds());
    }

    private IEnumerator SpawnClouds()
    {
        while (true)
        {
            // Spawn a cloud
            SpawnCloud();

            // Wait for the next spawn
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    private void SpawnCloud()
    {
        // Determine a random y position within the specified range
        float yPos = Random.Range(minY, maxY);

        // Pick a random cloud prefab
        int randomIndex = Random.Range(0, cloudPrefabs.Length);
        GameObject cloudPrefab = cloudPrefabs[randomIndex];

        // Instantiate the cloud at the right edge of the screen
        Vector2 spawnPosition = new Vector2(Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, 0, 0)).x + 1, yPos);
        GameObject cloud = Instantiate(cloudPrefab, spawnPosition, Quaternion.identity);

        // Set the cloud's speed
        CloudMover cloudMover = cloud.GetComponent<CloudMover>();
        if (cloudMover != null)
        {
            cloudMover.SetSpeed(cloudSpeed);
        }
    }

    public void UpdateCloudPositions(float playerSpeed)
    {
        // Update the speed of the clouds based on the player's speed
        cloudSpeed = playerSpeed / 2; // Adjust the divisor to control cloud speed relative to player speed
    }
}