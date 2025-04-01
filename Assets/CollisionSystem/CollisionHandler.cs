using UnityEngine;
using UnityEngine.SceneManagement;

public class CollisionHandler : MonoBehaviour
{

    [SerializeField]
    public GameObject gameOverScreen;
   
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Obstacle"))
        {
            ShowGameOver();
        }
    }
    void ShowGameOver()
    {
        gameOverScreen.SetActive(true);
        Time.timeScale = 0f; // Pause the game
    }
}
