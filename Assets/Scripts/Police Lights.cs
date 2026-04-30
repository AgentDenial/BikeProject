using UnityEngine;

public class PoliceLights : MonoBehaviour
{
    
    [SerializeField]
    private GameObject[] BlueLights;
    
    [SerializeField]
    private GameObject[] RedLights;
    
    [SerializeField]
    private GameObject[] WhiteLights;

   // private bool lightsAreOn = false;
    private float cycleInterval;

    [SerializeField]
    private float lightCooldown;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SetLights(false);
        cycleInterval = Time.time;
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time >= cycleInterval)
        {
            StartLightCycle();
            cycleInterval = Time.time + lightCooldown;
        }

    }

    public void StartLightCycle()
    {
        SetLights(false);

        /*if (Time.time >= cycleInterval > lightCooldown)
        {
            lightsAreOn = false;
        }*/
        int randomIndex = Random.Range(0, 3);
        //selected objects = TopLights[randomIndex];

        switch (randomIndex)
        {
            case 0:
                TurnOnLights(BlueLights);
                break;
            case 1:
                TurnOnLights(RedLights);
                break;
            case 2:
                TurnOnLights(WhiteLights);
                break;

        }
        /*if (randomIndex == 0 && lightsAreOn == false)
        {
            gameObject.BlueLights.SetActive(true);
            gameObject.RedLights.SetActive(false);
            gameObject.WhiteLights.SetActive(false);
            lightsAreOn = true;
}
        if (randomIndex == 1 && lightsAreOn == false)
        {
            gameObject.BlueLights.SetActive(false);
            gameObject.RedLights.SetActive(true);
            gameObject.WhiteLights.SetActive(false);
            lightsAreOn = true;
        }
        if (randomIndex == 2 && lightsAreOn == false)
        {
            gameObject.BlueLights.SetActive(false);
            gameObject.RedLights.SetActive(false);
            gameObject.WhiteLights.SetActive(true);
            lightsAreOn = true;
        }*/


    }

    private void SetLights(bool state)
    {
        TurnOnLights(BlueLights, state);
        TurnOnLights(RedLights, state);
        TurnOnLights(WhiteLights, state);
    }

    void TurnOnLights(GameObject[] lights, bool state = true)
    {
        foreach (GameObject lightObj in lights)
        {
            if (lightObj != null)
            {
                lightObj.SetActive(state);
            }
        }
    }
}
