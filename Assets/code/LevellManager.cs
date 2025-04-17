using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public int scoreToNextLevel = 3;
    public string nextSceneName;

    private GameManager gameManager;

    void Start()
    {
        gameManager = GameObject.FindFirstObjectByType<GameManager>();
    }

    void Update()
    {
        if (gameManager != null && gameManager.score >= scoreToNextLevel)
        {
            SceneManager.LoadScene(nextSceneName);
        }
    }
}
