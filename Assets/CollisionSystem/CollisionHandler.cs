using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CollisionHandler : MonoBehaviour
{
    [Header("Game Over UI")]
    public GameObject gameOverPanel;
    public Button restartButton;
    [SerializeField]
    public Rigidbody car;

    [Header("Collision Settings")]
    public string obstacleTag = "obstacle";

    private bool isGameOver = false;
    private MonoBehaviour[] carControlScripts;
    void Start()
    {
        carControlScripts = GetComponents<MonoBehaviour>();

        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);

        if (restartButton != null)
            restartButton.onClick.AddListener(RestartGame);
    }

    void OnCollisionEnter(Collision collision)
    {
        // Check if collided with an obstacle
        if (!isGameOver && collision.gameObject.CompareTag(obstacleTag))
        {
            GameOver();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (!isGameOver && other.CompareTag(obstacleTag))
        {
            Debug.Log("Trigger collision with obstacle: " + other.gameObject.name);
            GameOver();
        }
    }

    void GameOver()
    {
        if (isGameOver) return;  // Prevent multiple calls

        isGameOver = true;

        // Stop the car
        if (car != null)
        {
            car.linearVelocity = Vector3.zero;
            car.angularVelocity = Vector3.zero;
        }

        // Disable all car control scripts
        foreach (MonoBehaviour script in carControlScripts)
        {
            if (script != this && script.enabled)
                script.enabled = false;
        }

        // display gameover panel
        if (gameOverPanel != null)
            gameOverPanel.SetActive(true);
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}