using UnityEngine;
using UnityEngine.SceneManagement;

public class BGMManager : MonoBehaviour
{
    public static BGMManager Instance;

    [Header("Audio")]
    [SerializeField] public AudioSource bgmSource;

    [Header("Scene Music")]
    [SerializeField] public AudioClip menuMusic;
    [SerializeField] public AudioClip gameMusic;
    [SerializeField] public AudioClip gameOverMusic;
    [SerializeField] public AudioClip endMusic;


    void Awake()
    {
        // Singleton protection
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        SwitchMusic(scene.name);
    }

    public void SwitchMusic(string sceneName)
    {
        AudioClip newClip = null;

        switch (sceneName)
        {
            case "MainMenu":
                newClip = menuMusic;
                break;
            case "GameScene":
                newClip = gameMusic;
                break;
            case "EndScene":
                newClip = endMusic;
                break;
            /*case "GameOverScene":
                newClip = gameOverMusic;*/

        }

        if (newClip != null && bgmSource.clip != newClip)
        {
            bgmSource.clip = newClip;
            bgmSource.Play();
        }
    }

    public void PlayMainMenuMusic()
    {
        if (bgmSource.clip != menuMusic)
        {
            bgmSource.clip = menuMusic;
            bgmSource.Play();
        }
    }

    public void PlayGameSceneMusic()
    {
        if (bgmSource.clip != gameMusic)
        {
            bgmSource.clip = gameMusic;
            bgmSource.Play();
        }
    }
    public void PlayGameOverMusic()
    {
        if (bgmSource.clip != gameOverMusic)
        {
            bgmSource.clip = gameOverMusic;
            bgmSource.Play();
        }

    }

    public void PlayEndMusic()
    {
        if (bgmSource.clip != endMusic)
        {
            bgmSource.clip = endMusic;
            bgmSource.Play();
        }

    }
}
