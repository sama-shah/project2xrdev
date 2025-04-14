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

        // Ensure button has a collider
        Collider col = GetComponent<Collider>();
        if (col == null)
        {
            BoxCollider boxCol = gameObject.AddComponent<BoxCollider>();
            boxCol.isTrigger = true;
            // Size the collider based on RectTransform if possible
            RectTransform rect = GetComponent<RectTransform>();
            if (rect != null)
            {
                boxCol.size = new Vector3(rect.rect.width, rect.rect.height, 0.1f);
            }
        }
        else if (!col.isTrigger)
        {
            col.isTrigger = true;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        // Only respond to objects tagged as Hand or Controller
        if (other.CompareTag("Hand") || other.CompareTag("Controller"))
        {
            Debug.Log("VR controller touched restart button!");
            
            // Invoke the button click
            if (uiButton != null)
            {
                uiButton.onClick.Invoke();
            }
            
            // Direct call to restart game as backup
            if (gameManager != null)
            {
                gameManager.RestartGame();
            }
        }
    }
}