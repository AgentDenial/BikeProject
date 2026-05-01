using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro; 

public class EndgameController : MonoBehaviour
{
    [Header("Powerup count")]
    public int totalPowerUpsNeeded = 6;
    public int currentPowerUps = 0;
    bool EndGameReady = false;

    [Header("PutPoweUp CountUI prefab")]
    public TextMeshProUGUI counterText; 

    [Header("BlockEndingReferences")]
    public GameObject blockPlayer;
    
    public MenuManager menuManager;

    void Start()
    {
        menuManager = MenuManager.Instance;
        if (menuManager == null)
        {
            menuManager = FindAnyObjectByType<MenuManager>();
        }
        UpdateUI(); 
    }

    public void AddPowerUp()
    {
        currentPowerUps++;
        UpdateUI(); 


        if (currentPowerUps >= totalPowerUpsNeeded)
        {
            EndGameReady = true;
            Debug.Log("EndGameReady");
            if (blockPlayer != null) blockPlayer.SetActive(false);
        }
    }

    void UpdateUI()
    {
        if (counterText != null)
        {
            counterText.text = $"{currentPowerUps} / {totalPowerUpsNeeded}";
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"trigger called. collided with {other.gameObject.name} with {other.tag} tag");
        if (other.CompareTag("Player") && EndGameReady == true)
        {
            Debug.Log("player entered, game is ready");
            if (menuManager != null)
            {
                menuManager.GameOver();
            }
            else
            {
                Debug.LogError("MenuManager is null");
                UnityEngine.SceneManagement.SceneManager.LoadScene("GameOver");
            }


        }
        else if (other.CompareTag("Player"))
        {
            Debug.Log("Player entered, endgame is still false");
        }
    }
}