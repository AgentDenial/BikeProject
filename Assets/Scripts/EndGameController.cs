using UnityEngine;
using UnityEngine.SceneManagement;

public class EndgameController : MonoBehaviour
{
    [Header("Victory setup")]
    public int totalPowerUpsNeeded = 6;
    public int currentPowerUps = 0;

    [Header("References")]
    public GameObject blockPlayer;
    public string winSceneName = "WinnerMenu";

    void Update()
    {
        //Counts how may power ups do we have 
        if (currentPowerUps >= totalPowerUpsNeeded)
        {
            if (blockPlayer.activeSelf)
            {
                blockPlayer.SetActive(false); //we lift up the blockade                
            }
        }
    }
    public void AddPowerUp()
    {
        currentPowerUps++;
    }

    private void OnTriggerEnter(Collider other)
    {
        //check sphere tag
        if (other.CompareTag("Player") && currentPowerUps >= totalPowerUpsNeeded)
        {
            SceneManager.LoadScene(winSceneName);
        }
    }
}