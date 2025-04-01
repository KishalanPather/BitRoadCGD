using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance; // Singleton instance
    private bool isGameActive = false;
     public GameObject gameOverPanel;
    public Text finalScoreText;
    public Button restartButton;
    public Button homeButton;

    public AudioSource backgroundMusicSource;
    public AudioClip gameOverMusicClip;
    public float musicFadeDuration = 2f;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Ensure it persists across scenes
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void StartGame()
    {
        isGameActive = true;
         if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(false);
        }

        // Ensure game time is running
        Time.timeScale = 1f;

        // Play background music if not already playing
        if (backgroundMusicSource != null && !backgroundMusicSource.isPlaying)
            backgroundMusicSource.Play();
        Debug.Log("Game Started!");
        // Activate player controls, enable UI elements, etc.
    }

    public void EndGame()
    {
        isGameActive = false;
        Debug.Log("Game Over!");
        StartCoroutine(GameOver());
    }

    private IEnumerator GameOver()
    {
        if (backgroundMusicSource != null)
        {
            float startVolume = backgroundMusicSource.volume;
            float timer = 0f;
            while (timer < musicFadeDuration)
            {
                timer += Time.unscaledDeltaTime;
                backgroundMusicSource.volume = Mathf.Lerp(startVolume, 0, timer / musicFadeDuration);
                yield return null;
            }
            backgroundMusicSource.Stop();
            backgroundMusicSource.volume = startVolume; // Reset for future use
        }

        // Optionally, play game over music
        if (backgroundMusicSource != null && gameOverMusicClip != null)
        {
            backgroundMusicSource.clip = gameOverMusicClip;
            backgroundMusicSource.loop = false;
            backgroundMusicSource.Play();
        }

        // Display final score on the game over panel
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
            if (finalScoreText != null && ScoreManager.Instance != null)
            {
                finalScoreText.text = "Final Score: " + Mathf.FloorToInt(ScoreManager.Instance.GetTotalScore()).ToString();
            }
        }

        // Pause the game completely
        Time.timeScale = 0f;
    }

    public void RestartGame()
    {
         Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

        public void GoHome()
    {
        Time.timeScale = 1f;
        SceneManager.LoadSceneAsync(0); // Make sure your home scene is named correctly
    }

    public bool IsGameActive()
    {
        return isGameActive;
    }
}
