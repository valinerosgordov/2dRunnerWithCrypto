using System.Collections;

using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float horizontalSpeed = 5f; // Speed of automatic horizontal movement
    [SerializeField] private float jumpForce = 10f;      // Force applied for jumping
    [SerializeField] private float rollSpeed = 7f;       // Speed during rolling
    [SerializeField] private float rollDuration = 0.5f;  // Duration of the roll
    [SerializeField] private Transform groundCheck;      // Transform for ground check position
    [SerializeField] private float groundCheckRadius = 0.2f; // Radius for ground check circle
    [SerializeField] private LayerMask groundLayer;      // Layer for ground detection

    private Rigidbody2D rb2D;
    private bool isGrounded;
    private bool isRolling;
    private float rollTimer;
    private Vector2 rollDirection;
    private Animator animator;

    // Buff states
    public bool isShielded = false;
    public bool hasGun = false;
    public bool isInvincible = false;
    public bool isMagnetActive = false;

    // SpeedBoost variables
    private float originalSpeed;

    // Gun variables
    public GameObject bulletPrefab;
    public Transform bulletSpawnPoint;
    public float bulletSpeed = 10.0f;

    // Coin magnet variables
    public float magnetRadius = 5.0f;

    private void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
        if (rb2D == null)
        {
            Debug.LogError("PlayerController: Rigidbody2D component is missing!");
        }

        animator = GetComponent<Animator>();
        if (animator == null)
        {
            Debug.LogWarning("PlayerController: Animator component is missing!");
        }

        originalSpeed = horizontalSpeed;
    }

    private void Update()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        // Handle player jump
        if (isGrounded && Input.GetButtonDown("Jump"))
        {
            Jump();
        }

        // Handle player roll
        if (isGrounded && Input.GetButtonDown("Roll") && !isRolling)
        {
            StartRoll(Vector2.right); // Roll to the right
        }

        // Continue rolling
        if (isRolling)
        {
            ContinueRoll();
        }

        // Handle shooting
        if (hasGun && Input.GetButtonDown("Fire1"))
        {
            Shoot();
        }

        // Handle coin magnet
        if (isMagnetActive)
        {
            AttractCoins();
        }

        // Update animator parameters
        if (animator != null)
        {
            animator.SetBool("IsGrounded", isGrounded);
            animator.SetBool("IsRolling", isRolling);
            animator.SetFloat("HorizontalSpeed", Mathf.Abs(rb2D.velocity.x));

            // Only set IsRunning to true if the player is grounded and not rolling or jumping
            if (isGrounded && !isRolling)
            {
                animator.SetBool("IsRunning", true);
            }
            else
            {
                animator.SetBool("IsRunning", false);
            }
        }
    }

    private void FixedUpdate()
    {
        if (isRolling)
        {
            rb2D.velocity = rollDirection * rollSpeed;
        }
        else
        {
            // Automatic horizontal movement
            rb2D.velocity = new Vector2(horizontalSpeed, rb2D.velocity.y);
        }
    }

    private void Jump()
    {
        rb2D.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        isGrounded = false;

        // Trigger jump animation
        if (animator != null)
        {
            animator.SetTrigger("Jump");
            animator.SetBool("IsRunning", false); // Stop running animation during jump
        }
    }

    private void ContinueRoll()
    {
        rollTimer -= Time.deltaTime;
        if (rollTimer <= 0)
        {
            isRolling = false;

            // Exit roll animation
            if (animator != null)
            {
                animator.SetBool("IsRolling", false);
                animator.SetBool("IsRunning", true); // Resume running animation after roll
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Ground"))
        {
            isGrounded = true;

            // Trigger landing animation
            if (animator != null)
            {
                animator.ResetTrigger("Jump"); // Ensure the jump trigger is reset
                animator.SetBool("IsGrounded", true);
            }
        }

        if (collision.collider.CompareTag("Obstacle"))
        {
            if (isShielded || isInvincible)
            {
                // Prevent damage
                return;
            }
            else
            {
                // Handle player damage or death
                Debug.Log("Player hit an obstacle!");
                // Trigger any death or damage animation if available
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if player collides with a coin
        if (other.CompareTag("Coin"))
        {
            UIManager.Instance?.AddCoin();
            Destroy(other.gameObject); // Destroy the coin object
        }
    }

    private void StartRoll(Vector2 direction)
    {
        isRolling = true;
        rollTimer = rollDuration;
        rollDirection = direction.normalized;

        // Trigger roll animation
        if (animator != null)
        {
            animator.SetBool("IsRolling", true);
            animator.SetBool("IsRunning", false); // Stop running animation during roll
        }
    }

    private void Shoot()
    {
        GameObject bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, bulletSpawnPoint.rotation);
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        rb.velocity = bulletSpawnPoint.up * bulletSpeed;
    }

    private void AttractCoins()
    {
        Collider2D[] coins = Physics2D.OverlapCircleAll(transform.position, magnetRadius);
        foreach (Collider2D coin in coins)
        {
            if (coin.CompareTag("Coin"))
            {
                // Attract coin to player
                coin.transform.position = Vector2.MoveTowards(coin.transform.position, transform.position, horizontalSpeed * Time.deltaTime);
            }
        }
    }

    public void ActivateSpeedBoost(float multiplier, float duration)
    {
        StartCoroutine(SpeedBoost(multiplier, duration));
    }

    private IEnumerator SpeedBoost(float multiplier, float duration)
    {
        horizontalSpeed *= multiplier;
        yield return new WaitForSeconds(duration);
        horizontalSpeed = originalSpeed;
    }

    public void ActivateShield(float duration)
    {
        StartCoroutine(Shield(duration));
    }

    private IEnumerator Shield(float duration)
    {
        isShielded = true;
        yield return new WaitForSeconds(duration);
        isShielded = false;
    }

    public void ActivateGun(float duration)
    {
        StartCoroutine(Gun(duration));
    }

    private IEnumerator Gun(float duration)
    {
        hasGun = true;
        yield return new WaitForSeconds(duration);
        hasGun = false;
    }

    public void ActivateInvincibility(float duration)
    {
        StartCoroutine(Invincibility(duration));
    }

    private IEnumerator Invincibility(float duration)
    {
        isInvincible = true;
        yield return new WaitForSeconds(duration);
        isInvincible = false;
    }

    public void ActivateCoinMagnet(float duration)
    {
        StartCoroutine(CoinMagnet(duration));
    }

    private IEnumerator CoinMagnet(float duration)
    {
        isMagnetActive = true;
        yield return new WaitForSeconds(duration);
        isMagnetActive = false;
    }
}
