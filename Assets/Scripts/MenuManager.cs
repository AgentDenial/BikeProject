using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public static MenuManager Instance;
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
    public void MainMenu()
    {
        // Loads the scene at index 1 in Build Settings
        SceneManager.LoadScene(0);
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

    public void YouLose()
    {
        SceneManager.LoadScene(3);
    }
    public void GameOver()
    {
        // Loads the scene at index 1 in Build Settings
        SceneManager.LoadScene(4);
    }
    public void Credits()
    {
        // Loads the scene at index 1 in Build Settings
        SceneManager.LoadScene(5);
    }

    public void Quit()
    {
        // Loads the scene at index 1 in Build Settings
        Application.Quit();
    }

    private void LoadScene(int buildIndex)
    {
        if (Instance == null)
        {
            Instance = FindAnyObjectByType<MenuManager>();
            if (Instance == null)
            {
                Debug.LogError("Menu Manager not found");
                return;
            }
        }
        Debug.Log($"Loading Scene Index: {buildIndex}");
        SceneManager.LoadScene(buildIndex);
    }
}

