using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class CloudMover : MonoBehaviour
{
    private float speed;

    public void SetSpeed(float cloudSpeed)
    {
        speed = cloudSpeed;
    }

    private void Update()
    {
        // Move the cloud to the left
        transform.Translate(Vector2.left * speed * Time.deltaTime);

        // Destroy the cloud when it goes off screen
        if (transform.position.x < Camera.main.ScreenToWorldPoint(new Vector3(0, 0, 0)).x - 1)
        {
            Destroy(gameObject);
        }
    }
}