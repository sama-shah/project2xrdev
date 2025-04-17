using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // For UI elements
using TMPro; // For TextMeshPro UI

public class GameManager : MonoBehaviour
{
    [Header("Game Settings")]
    public List<GameObject> tiles;
    public float timeBetweenPrompts = 2f;
    public float minTimeBetweenPrompts = 0.5f;
    public float difficultyIncreaseFactor = 0.95f;
    public float difficultyIncreaseInterval = 5f;
    
    [Header("Game State")]
    public int score = 0;
    public int streak = 0;
    public bool gameActive = true;
    
    [Header("UI References")]
    public TextMeshProUGUI scoreText;
    public GameObject gameOverPanel;
    public TextMeshProUGUI finalScoreText;
    
    [Header("Audio")]
    public AudioClip gameOverSound;
    
    private GameObject currentTile;
    private float _nextPromptTime;
    private float _timeSinceLastDifficultyIncrease = 0f;
    private AudioSource _audioSource;

    void Start()
    {

        // TEMPORARY: Force UI visibility for testing
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
            Debug.Log("Forcing game over panel to be visible for testing");
        }
        ///

        _nextPromptTime = Time.time + timeBetweenPrompts;
        
        // Get or add an AudioSource for game sounds
        _audioSource = GetComponent<AudioSource>();
        if (_audioSource == null)
        {
            _audioSource = gameObject.AddComponent<AudioSource>();
        }
        
        // Hide game over panel at start
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(false);
        }
        
        // Initialize score display
        UpdateScoreDisplay();
        
        // Start the game
        gameActive = true;
    }
    
    void Update()
    {
        if (!gameActive) return;
        
        // Increase difficulty over time
        _timeSinceLastDifficultyIncrease += Time.deltaTime;
        
        if (_timeSinceLastDifficultyIncrease >= difficultyIncreaseInterval)
        {
            _timeSinceLastDifficultyIncrease = 0f;
            timeBetweenPrompts = Mathf.Max(minTimeBetweenPrompts, timeBetweenPrompts * difficultyIncreaseFactor);
            Debug.Log("Difficulty increased! Time between prompts: " + timeBetweenPrompts);
        }
        
        // Check if it's time for a new prompt
        if (Time.time >= _nextPromptTime)
        {
            ActivateRandomTile();
            _nextPromptTime = Time.time + timeBetweenPrompts;
        }
    }
    
    void ActivateRandomTile()
    {
        // If there's already a glowing tile, consider it a miss
        if (currentTile != null)
        {
            GlowingTile glowingTileComponent = currentTile.GetComponent<GlowingTile>();
            if (glowingTileComponent != null && glowingTileComponent.IsGlowing())
            {
                glowingTileComponent.StopGlowing();
                streak = 0;
                Debug.Log("Missed tile! Streak reset.");
            }
        }

        // Pick a random tile
        int index = Random.Range(0, tiles.Count);
        currentTile = tiles[index];

        // Trigger its glow
        GlowingTile glowTile = currentTile.GetComponent<GlowingTile>();
        if (glowTile != null)
        {
            glowTile.TriggerGlow();
        }
        else
        {
            Debug.LogError("GlowingTile component not found on tile: " + currentTile.name);
        }
    }

    // public void OnTileClicked(GameObject clickedTile)
    // {
    //     if (!gameActive) return;
        
    //     if (clickedTile == currentTile)
    //     {
    //         Debug.Log("Correct tile clicked!");
            
    //         // Play sound using the GlowingTile's method
    //         GlowingTile glowTile = clickedTile.GetComponent<GlowingTile>();
    //         if (glowTile != null)
    //         {
    //             glowTile.PlaySound();
    //         }
            
    //         // Update score and streak
    //         score++;
    //         streak++;
    //         UpdateScoreDisplay();
            
    //         Debug.Log("Score: " + score + " | Streak: " + streak);
    //     }
    // }

    public void OnTileClicked(GameObject clickedTile)
{
    if (!gameActive) return;

    if (clickedTile == currentTile)
    {
        Debug.Log("Correct tile clicked!");

        // Play sound using the GlowingTile's method
        GlowingTile glowTile = clickedTile.GetComponent<GlowingTile>();
        if (glowTile != null)
        {
            glowTile.PlaySound();
        }

        // Update score and streak
        score++;
        streak++;
        UpdateScoreDisplay();

        Debug.Log("Score: " + score + " | Streak: " + streak);
    }
    else
    {
        // NEW: show red and trigger game over
        Renderer renderer = clickedTile.GetComponent<Renderer>();
        if (renderer != null)
        {
            renderer.material.color = Color.red;
        }

        OnWrongTileClicked(); // call game over
    }
}

    
    public void OnWrongTileClicked()
    {
        if (!gameActive) return;
        
        Debug.Log("Wrong tile clicked! Game Over.");
        gameActive = false;
        
        // Play game over sound
        if (_audioSource != null && gameOverSound != null)
        {
            _audioSource.clip = gameOverSound;
            _audioSource.Play();
        }
        
        // Stop any currently glowing tiles
        if (currentTile != null)
        {
            GlowingTile glowingTileComponent = currentTile.GetComponent<GlowingTile>();
            if (glowingTileComponent != null)
            {
                glowingTileComponent.StopGlowing();
            }
        }
        
        // Show game over UI
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
            
            if (finalScoreText != null)
            {
                finalScoreText.text = "Final Score: " + score;
            }
        }
    }
    
    public void RestartGame()
    {

        // Stop any playing sounds globally
        AudioSource[] allAudio = FindObjectsOfType<AudioSource>();
        foreach (var audio in allAudio)
        {
            if (audio.isPlaying)
                audio.Stop();
        }


        // Reset game state
        score = 0;
        streak = 0;
        gameActive = true;
        _nextPromptTime = Time.time + timeBetweenPrompts;
        timeBetweenPrompts = 2f; // Reset to initial value
        
        // Hide game over UI
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(false);
        }
        
        // Update score display
        UpdateScoreDisplay();
        
        Debug.Log("Game restarted!");
    }
    
    private void UpdateScoreDisplay()
    {
        // if (scoreText != null)
        // {
        //     scoreText.text = "Score: " + score;
        // }
        Debug.Log("Updating score display to: " + score);
        if (scoreText != null)
        {
            scoreText.text = "Score: " + score;
            Debug.Log("Score text updated to: " + scoreText.text);
        }
        else
        {
            Debug.LogError("Score text reference is null!");
        }
    }
}

// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// public class GameManager : MonoBehaviour
// {
//     public List<GameObject> tiles;         // Assign tiles manually in inspector
//     public float timeBetweenPrompts = 2f;

//     private GameObject currentTile;

//     void Start()
//     {
//         StartCoroutine(GlowSequence());
//     }

//     IEnumerator GlowSequence()
//     {
//         while (true)
//         {
//             yield return new WaitForSeconds(timeBetweenPrompts);

//             // pick a random tile
//             int index = Random.Range(0, tiles.Count);
//             currentTile = tiles[index];

//             // trigger its glow
//             currentTile.GetComponent<GlowingTile>().TriggerGlow();
//         }
//     }

//     public void OnTileClicked(GameObject clickedTile)
//     {
//         if (clickedTile == currentTile)
//         {
//             Debug.Log("Correct tile clicked!");
//             AudioSource audio = clickedTile.GetComponent<AudioSource>();
//             if (audio != null)
//                 audio.Play();
//         }
//         else
//         {
//             Debug.Log("Wrong tile.");
//         }
//     }
// }
