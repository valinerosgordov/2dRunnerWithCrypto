using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [SerializeField] private TextMeshProUGUI distanceText;
    [SerializeField] private TextMeshProUGUI coinsText;

    private int coinsCollected = 0;
    private float distanceTraveled = 0f;
    private Transform player;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Optional: if you want the UIManager to persist across scenes
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        // Assuming the player has a tag "Player"
        player = GameObject.FindGameObjectWithTag("Player").transform;

        if (distanceText == null || coinsText == null)
        {
            Debug.LogError("UIManager: TextMeshProUGUI components are not assigned!");
        }

        // Initialize UI
        UpdateDistanceUI();
        UpdateCoinsUI();
    }

    private void Update()
    {
        if (player != null)
        {
            // Update distance traveled based on player's position
            distanceTraveled = player.position.x;
            UpdateDistanceUI();
        }
    }

    public void AddCoin()
    {
        coinsCollected++;
        UpdateCoinsUI();
    }

    private void UpdateDistanceUI()
    {
        distanceText.text = $"Distance: {distanceTraveled:F1}m";
    }

    private void UpdateCoinsUI()
    {
        coinsText.text = $"Coins: {coinsCollected}";
    }
}