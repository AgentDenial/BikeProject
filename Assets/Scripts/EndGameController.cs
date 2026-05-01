using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro; 

public class EndgameController : MonoBehaviour
{
    [Header("Powerup count")]
    public int totalPowerUpsNeeded = 6;
    public int currentPowerUps = 0;

    [Header("PutPoweUp CountUI prefab")]
    public TextMeshProUGUI counterText; 

    [Header("BlockEndingReferences")]
    public GameObject blockPlayer;
    public string winSceneName = "WinnerMenu";

    void Start()
    {
        UpdateUI(); 
    }

    public void AddPowerUp()
    {
        currentPowerUps++;
        UpdateUI(); 


        if (currentPowerUps >= totalPowerUpsNeeded)
        {
            if (blockPlayer != null) blockPlayer.SetActive(false);
        }
    }

    void UpdateUI()
    {
        if (counterText != null)
        {
            counterText.text = currentPowerUps + " / " + totalPowerUpsNeeded;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && currentPowerUps >= totalPowerUpsNeeded)
        {
            SceneManager.LoadScene(winSceneName);
        }
    }
}