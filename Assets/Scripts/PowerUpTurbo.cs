using UnityEngine;

public class PowerUpTurbo : MonoBehaviour
{
    public float speedBoost = 20f;
    //public float duration = 3f;
    private void OnTriggerEnter(Collider other)
    {
        BikeController bike = other.GetComponentInParent<BikeController>();

        GameObject mainBike = GameObject.Find("PlayerBike_GRP");

        if (mainBike != null)
        {
            bike = mainBike.GetComponent<BikeController>();
        }

        if (bike != null)
        {
            EndgameController endGameController = Object.FindFirstObjectByType<EndgameController>();

            if (endGameController != null)
            {
                endGameController.AddPowerUp(); // Le sumamos 1 al contador global
            }

            bike.ApplyTurbo(speedBoost);
            Destroy(gameObject);
        }
    }
}
