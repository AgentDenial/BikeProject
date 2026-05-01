using UnityEngine;

public class PlayerDangerSystem : MonoBehaviour
{
    //public PlayerController playerController;
    public BikeController bikeController;
    public PoliceMegaphone policeMegaphone;
    public BGMManager bgmManager;
    public MenuManager menuManager;

    public float dangerRadius = 8f;
    //public float safeSpeed = 10f;
    public float baseCaptureRate = 1f;
    public float captureDecayRate = 0.75f;
    public float maxCaptureProgress = 5f;

    private float safeSpeed;
    private float slowDivison = 2f / 5f;
    

    //public string policeTag = "Police";
    public float currentSpeed;
    public int policeInRange;
    public float captureProgress;

    //private float lastMegaphoneTime;
    //private float megaphoneCooldown = 10f;
    private int previousPoliceInRange = 0;


    private bool isGameOver = false;
    private Vector3 lastPosition;

    void Start()
    {
        lastPosition = transform.position;
        policeMegaphone = FindAnyObjectByType<PoliceMegaphone>();
    }

    void Update()
    {
        if (isGameOver) return;

        UpdateSpeed();
        CountPoliceInRange();
        UpdateCaptureProgress();
        CheckNewPoliceInRange();

        if (captureProgress >= maxCaptureProgress)
        {
            isGameOver = true;
            TriggerGameOver();
        }
    }

    void UpdateSpeed()
    {
        Vector3 movement = transform.position - lastPosition;
        // currentSpeed = playerController.currentSpeed;
        currentSpeed = movement.magnitude / Time.deltaTime;
        lastPosition = transform.position;
    }

    /*void CountPoliceInRange()
    {
        policeInRange = 0;

        GameObject[] policeObjects = GameObject.FindGameObjectsWithTag(policeTag);

        foreach (GameObject police in policeObjects)
        {
            float distance = Vector3.Distance(transform.position, police.transform.position);

            if (distance <= dangerRadius)
            {
                policeInRange++;
            }
        }
    }*/
    void CountPoliceInRange()
    {
        policeInRange = 0;

        GameObject[] policeObjects = GameObject.FindGameObjectsWithTag("Police");

        foreach (GameObject police in policeObjects)
        {
            float distance = Vector3.Distance(transform.position, police.transform.position);

            if (distance <= dangerRadius)
            {
                policeInRange++;
                
            }
        } 
    }

    void CheckNewPoliceInRange()
    {
        if (policeInRange > previousPoliceInRange)
        {
            policeMegaphone.PlayRandomMegaphone();
        }

        previousPoliceInRange = policeInRange;
    }
    
    void UpdateCaptureProgress()
    {
        bool inDanger = policeInRange > 0;
        //added line
        safeSpeed = bikeController.maxSpeed * slowDivison;
        bool isSlow = currentSpeed < safeSpeed;

        //added for testing
        if (isSlow)
        {
            //Debug.Log($"Too slow, current speed {currentSpeed:F1}, target {safeSpeed:F1}");
        }
        else
        {
            //Debug.Log($"Fast Enough, current speed {currentSpeed:F1}");
        }

        if (inDanger && isSlow)
        {
            float increaseAmount = baseCaptureRate * policeInRange * Time.deltaTime;
            captureProgress += increaseAmount;
        }
        else
        {
            captureProgress -= captureDecayRate * Time.deltaTime;
        }

        captureProgress = Mathf.Clamp(captureProgress, 0f, maxCaptureProgress);
        //Debug.Log($"{captureProgress}");
    }

    /*void TriggerGameOver()
    {
        isGameOver = true;
        Debug.Log("GAME OVER");

        // Game over
        Time.timeScale = 0f;
    }*/
    void TriggerGameOver()
    {
        if (isGameOver == true)
        {
            
            Debug.Log("GAME OVER");

            menuManager.GameOver();
            bgmManager.PlayGameOverMusic();

        }
       
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, dangerRadius);
    }
}