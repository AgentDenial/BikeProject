using UnityEngine;

public class CollisionProxy : MonoBehaviour
{
    public BikeController playerBikeGroup; 

    private void OnCollisionEnter(Collision collision)// It will handle de collision that is in the player bike script
    {

        playerBikeGroup.HandleCollision(collision);
    }

}
