using UnityEngine;
using UnityEngine.UI;

public class EngineSounds : MonoBehaviour
{
    //script specifically for engine sounds

    AudioSource audioSource;
    
   //min, max and current pitch based on speed
    public float minRevPitch = .25f;
    public float maxRevPitch = 3f;
    private float currentPlayerSpeed;

    //reference to speedometer?
    private Speedometer speedometer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.pitch = minRevPitch;
    }

    // Update is called once per frame
    void Update()
    {
        speedometer.speed = currentPlayerSpeed; //reference to speedometer
        //caps min and max so audio does not get distorted

        if (currentPlayerSpeed < minRevPitch)
        {
            audioSource.pitch = minRevPitch;
        }
        else if (currentPlayerSpeed > maxRevPitch)
        {
            audioSource.pitch = maxRevPitch;
        }
        else
        {
            audioSource.pitch = currentPlayerSpeed;
        }
    }
}
