using UnityEngine;

public class PlayerDangerSystem : MonoBehaviour
{
    public PlayerController playerController;
    public float dangerRadius = 8f;
    public float safeSpeed = 10f;
    public float baseCaptureRate = 1f;
    public float captureDecayRate = 0.75f;
    public float maxCaptureProgress = 5f;

    public string policeTag = "Police";
    public float currentSpeed;
    public int policeInRange;
    public float captureProgress;

    private bool isGameOver = false;
    private Vector3 lastPosition;

    void Start()
    {
        lastPosition = transform.position;
    }

    void Update()
    {
        if (isGameOver) return;

        UpdateSpeed();
        CountPoliceInRange();
        UpdateCaptureProgress();

        if (captureProgress >= maxCaptureProgress)
        {
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

    void CountPoliceInRange()
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
    }

    void UpdateCaptureProgress()
    {
        bool inDanger = policeInRange > 0;
        bool isSlow = currentSpeed < safeSpeed;

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
    }

    void TriggerGameOver()
    {
        isGameOver = true;
        Debug.Log("GAME OVER");

        // Game over
        Time.timeScale = 0f;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, dangerRadius);
    }
}