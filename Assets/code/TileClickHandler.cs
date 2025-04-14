using UnityEngine;

public class TileClickHandler : MonoBehaviour
{
    private GameManager gameManager;
    private GlowingTile glowingTile;

    void Start()
    {

        // Replace the obsolete FindObjectOfType - debugging
        gameManager = GameObject.FindFirstObjectByType<GameManager>();
        glowingTile = GetComponent<GlowingTile>();

    }

    void OnMouseDown()
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
                }
            }
        }
    }
}

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
