using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{   
    public void StartGame() {
        SceneManager.LoadSceneAsync(1);
    }

    public void ShowTutorial()
    {
        SceneManager.LoadSceneAsync(2);
    }
}
