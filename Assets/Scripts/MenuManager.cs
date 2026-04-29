using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public void MainMenu()
    {
        // Loads the scene at index 1 in Build Settings
        SceneManager.LoadScene(0);
    }
    public void GameOver()
    {
        // Loads the scene at index 1 in Build Settings
        SceneManager.LoadScene(3);
    }
    public void HowToPlay()
    {
        // Loads the scene at index 1 in Build Settings
        SceneManager.LoadScene(1);
    }
    public void Play()
    {
        // Loads the scene at index 1 in Build Settings
        SceneManager.LoadScene(2);
    }
    public void Credits()
    {
        // Loads the scene at index 1 in Build Settings
        SceneManager.LoadScene(4);
    }
    public void Quit()
    {
        // Loads the scene at index 1 in Build Settings
        Application.Quit();
    }
}

