using UnityEngine;
using UnityEngine.UI;

public class VRButtonInteraction : MonoBehaviour
{
    private GameManager gameManager;
    private Button uiButton;

    void Start()
    {
        gameManager = GameObject.FindFirstObjectByType<GameManager>();
        uiButton = GetComponent<Button>();

    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Hand") || other.CompareTag("Controller"))
        {
            Debug.Log("VR controller touched restart button!");

            if (gameManager != null && !gameManager.gameActive)
            {
                Debug.Log("Restarting game...");

                if (uiButton != null)
                {
                    uiButton.onClick.Invoke();
                }

                gameManager.RestartGame();
            }
            else
            {
                Debug.Log("Game is still active. Not restarting.");
            }
        }
    }


//     void OnTriggerEnter(Collider other)
//     {
//         Debug.Log($"[RestartButton] Trigger entered by: {other.name}");
//         // Only respond to objects tagged as Hand or Controller
//         if (other.CompareTag("Hand") || other.CompareTag("Controller"))
//         {
//             Debug.Log("VR controller touched restart button!");
            
//             // Invoke the button click
//             if (uiButton != null)
//             {
//                 Debug.Log("Invoking UI button...");
//                 uiButton.onClick.Invoke();
//             }
//             else
//             {
//                 Debug.LogWarning("UI Button is null");
//             }
            
//             // Direct call to restart game as backup
//             if (gameManager != null)
//             {
//                 gameManager.RestartGame();
//             }
//             else
//             {
//                 Debug.LogWarning("GameManager is null");
//             }
//         }
//     }
}