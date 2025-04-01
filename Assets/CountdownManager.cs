using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CountdownManager : MonoBehaviour
{
    public Text countdownText;
    public Text objectiveText;
    public GameObject gameUI; // Reference to the GameUI object

    private string[] objectives = {
        "Don't Run Out of Fuel.",
        "Avoid Obstacles.",
        "Maximise Speed."
    };

    void Start()
    {
        gameUI.SetActive(false); // Hide the game UI at the start
        StartCoroutine(CountdownRoutine());
    }

    IEnumerator CountdownRoutine()
    {
        objectiveText.text = "";

        for (int i = 5; i > 0; i--)
        {
            countdownText.text = i.ToString();

            if (i >= 3)  
            {
                objectiveText.text += objectives[6 - i - 1] + "\n";
            }

            yield return new WaitForSeconds(1f);
        }

        objectiveText.text = "";
        countdownText.text = "GO!";

        yield return new WaitForSeconds(1f);
        countdownText.gameObject.SetActive(false);

        // Show the game UI after the countdown ends
        gameUI.SetActive(true);

        GameManager.Instance.StartGame();
    }
}