using UnityEngine;

public class TileClickHandler : MonoBehaviour
{
    private GameManager gameManager;

    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
    }

    void OnMouseDown()
    {
        gameManager.OnTileClicked(gameObject);
    }
}
