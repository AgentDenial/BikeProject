using UnityEngine;
using UnityEngine.AI;

public class PoliceChase : MonoBehaviour
{
    public Transform player;
    public float stopDistance = 2f;

    private NavMeshAgent agent;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        if (player == null) return;

        float distance = Vector3.Distance(transform.position, player.position);

        if (distance <= stopDistance)
        {
            // stop
            agent.isStopped = true;
        }
        else
        {
            // start chasing again
            agent.isStopped = false;
            agent.SetDestination(player.position);
        }
    }

    //suggestion: reference to BikeController to check player's current max speed and adjust actor speed accordingly?
}