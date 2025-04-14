using UnityEngine;

public class TileClickHandler : MonoBehaviour
{
    private GameManager gameManager;
    private GlowingTile glowingTile;

    void Start()
    {
        gameManager = GameObject.FindFirstObjectByType<GameManager>();
        glowingTile = GetComponent<GlowingTile>();
        
        // Ensure this object has a collider set as trigger
        Collider col = GetComponent<Collider>();
        if (col != null && !col.isTrigger)
        {
            col.isTrigger = true;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        // Only respond to objects tagged as Hand or Controller
        if (other.CompareTag("Hand") || other.CompareTag("Controller"))
        {
            HandleVRInteraction();
        }
    }
    
    private void HandleVRInteraction()
    {
        if (glowingTile != null)
        {
            if (glowingTile.IsGlowing())
            {
                // Correct tile hit
                glowingTile.StopGlowing();
                
                // Play sound
                glowingTile.PlaySound();
                
                // Notify game manager
                if (gameManager != null)
                {
                    gameManager.OnTileClicked(gameObject);
                    Debug.Log("VR interaction with correct glowing tile!");
                }
            }
            else
            {
                // Wrong tile hit
                glowingTile.ShowError();
                
                // Trigger game over
                if (gameManager != null)
                {
                    gameManager.OnWrongTileClicked();
                    Debug.Log("VR interaction with wrong tile! Game over triggered.");
                }
            }
        }
    }
}

//     void OnMouseDown()
//     {
//         if (glowingTile != null)
//         {
//             if (glowingTile.IsGlowing())
//             {
//                 // Correct tile hit
//                 glowingTile.StopGlowing();
                
//                 // Play sound
//                 glowingTile.PlaySound();
                
//                 // Notify game manager
//                 if (gameManager != null)
//                 {
//                     gameManager.OnTileClicked(gameObject);
//                 }
//             }
//             else
//             {
//                 // Wrong tile hit
//                 glowingTile.ShowError();
                
//                 // Trigger game over
//                 if (gameManager != null)
//                 {
//                     gameManager.OnWrongTileClicked();
//                 }
//             }
//         }
//     }
// }

// using UnityEngine;

// public class TileClickHandler : MonoBehaviour
// {
//     private GameManager gameManager;

//     void Start()
//     {
//         gameManager = FindObjectOfType<GameManager>();
//     }

//     void OnMouseDown()
//     {
//         gameManager.OnTileClicked(gameObject);
//     }
// }
