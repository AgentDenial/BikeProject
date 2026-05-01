using UnityEngine;

public class PowerUpTurbo : MonoBehaviour
{
    public float speedBoost = 20f;
    //public float duration = 3f;
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("A");
        BikeController bike = other.GetComponentInParent<BikeController>();
        //BikeController bike = other.gameObject.GetComponentInParent<BikeController>();

        GameObject mainBike = GameObject.Find("PlayerBike_GRP");

        if (mainBike != null)
        {
            bike = mainBike.GetComponent<BikeController>();
            Debug.Log("B");
        }

        if (bike != null)
        {
            bike.ApplyTurbo(speedBoost);
            Debug.Log("C");
            Destroy(gameObject); 
        }
    }
}
