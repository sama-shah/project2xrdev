using System.Collections;
using UnityEngine;

public class GlowingTile : MonoBehaviour
{
    [Header("Materials")]
    public Material defaultMaterial;   // Assign in inspector
    public Material glowMaterial;      // Assign in inspector
    public Material errorMaterial;     // Red material for wrong hits
    
    [Header("Settings")]
    public float glowDuration = 50.0f;
    
    private MeshRenderer _renderer;
    private AudioSource _audioSource;
    private Coroutine _glowCoroutine;
    private bool _isGlowing = false;
    
    // Static reference to the currently playing audio source
    private static AudioSource currentlyPlayingAudio = null;
    
    void Start()
    {
        _renderer = GetComponent<MeshRenderer>();
        _audioSource = GetComponent<AudioSource>();
        
        if (_renderer == null)
        {
            Debug.LogError("No MeshRenderer found on " + gameObject.name);
        }
        else if (defaultMaterial != null)
        {
            // Set default material at start
            _renderer.material = defaultMaterial;
        }
    }

    public void TriggerGlow()
    {
        // Stop existing glow coroutine if it's running
        if (_glowCoroutine != null)
        {
            StopCoroutine(_glowCoroutine);
        }
        
        // Start new glow coroutine
        _glowCoroutine = StartCoroutine(GlowRoutine());
    }
    
    IEnumerator GlowRoutine()
    {
        // Apply glow material
        if (_renderer != null && glowMaterial != null)
        {
            _renderer.material = glowMaterial;
            _isGlowing = true;
        }
        
        // Wait for glow duration
        yield return new WaitForSeconds(glowDuration);
        
        // Return to default material (if the tile hasn't been hit)
        StopGlowing();
    }
    
    public void StopGlowing()
    {
        if (_renderer != null && defaultMaterial != null)
        {
            _renderer.material = defaultMaterial;
            _isGlowing = false;
        }
    }
    
    public void ShowError()
    {
        if (_renderer != null && errorMaterial != null)
        {
            _renderer.material = errorMaterial;
            StartCoroutine(ResetErrorAfterDelay());
        }
    }
    
    IEnumerator ResetErrorAfterDelay()
    {
        yield return new WaitForSeconds(1.0f);
        if (!_isGlowing && _renderer != null && defaultMaterial != null)
        {
            _renderer.material = defaultMaterial;
        }
    }
    
    public bool IsGlowing()
    {
        return _isGlowing;
    }
    
    public void PlaySound()
    {
        if (_audioSource != null)
        {
            Debug.Log("Attempting to play sound on " + gameObject.name);
            
            // Check if we have an audio clip
            if (_audioSource.clip == null)
            {
                Debug.LogError("No audio clip assigned to AudioSource on " + gameObject.name);
                return;
            }
            
            // Stop the currently playing audio if there is one
            if (currentlyPlayingAudio != null && currentlyPlayingAudio != _audioSource)
            {
                currentlyPlayingAudio.Stop();
            }
            
            // Set volume to make sure it's audible
            _audioSource.volume = 1.0f;
            
            // Play this tile's audio
            _audioSource.Play();
            Debug.Log("Sound played successfully");
            
            // Update the reference to the currently playing audio
            currentlyPlayingAudio = _audioSource;
        }
        else
        {
            Debug.LogError("No AudioSource component found on " + gameObject.name);
        }
    }

    
// public void PlaySound()
    // {
    //     if (_audioSource != null)
    //     {
    //         // Stop the currently playing audio if there is one
    //         if (currentlyPlayingAudio != null && currentlyPlayingAudio != _audioSource)
    //         {
    //             currentlyPlayingAudio.Stop();
    //         }
            
    //         // Play this tile's audio
    //         _audioSource.Play();
            
    //         // Update the reference to the currently playing audio
    //         currentlyPlayingAudio = _audioSource;
    //     }
    // }
    
    void OnTriggerEnter(Collider other)
{
    // Check if the collider is the player's hand/controller
    if (other.CompareTag("Hand") || other.CompareTag("Controller"))
    {
        // Replace the obsolete FindObjectOfType
        GameManager gameManager = GameObject.FindFirstObjectByType<GameManager>();
        
        if (_isGlowing)
        {
            StopGlowing();
            
            // Notify game manager of correct hit
            if (gameManager != null)
            {
                gameManager.OnTileClicked(gameObject);
            }
        }
        else
        {
            // Wrong tile hit - show error and trigger game over
            ShowError();
            
            if (gameManager != null)
            {
                gameManager.OnWrongTileClicked();
            }
        }
    }
}
}

// using UnityEngine;

// public class GlowingTile : MonoBehaviour
// {
//     public Color glowColor = Color.cyan;
//     public float glowDuration = 0.5f;
//     public float fadeDuration = 0.5f;

//     private Material tileMaterial;
//     private Color originalColor;
//     private bool isGlowing = false;

//     void Start()
//     {
//         tileMaterial = GetComponent<Renderer>().material;
//         originalColor = tileMaterial.color;
//     }

//     public void TriggerGlow()
//     {
//         if (!isGlowing)
//             StartCoroutine(GlowEffect());
//     }

//     private System.Collections.IEnumerator GlowEffect()
//     {
//         isGlowing = true;
//         tileMaterial.color = glowColor;

//         yield return new WaitForSeconds(glowDuration);

//         float t = 0;
//         while (t < fadeDuration)
//         {
//             tileMaterial.color = Color.Lerp(glowColor, originalColor, t / fadeDuration);
//             t += Time.deltaTime;
//             yield return null;
//         }

//         tileMaterial.color = originalColor;
//         isGlowing = false;
//     }
// }
