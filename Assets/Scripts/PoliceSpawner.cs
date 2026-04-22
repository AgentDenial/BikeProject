using UnityEngine;
using UnityEngine.AI;

public class PoliceSpawner : MonoBehaviour
{
    public GameObject policePrefab;
    public Transform player;
    public Camera mainCamera;

    public float minDistance = 25f;
    public float maxDistance = 40f;
    public float navMeshSearchRadius = 10f;

    public float spawnInterval = 5f;

    void Start()
    {
        InvokeRepeating(nameof(SpawnPolice), 2f, spawnInterval);
    }

    void SpawnPolice()
    {
        for (int i = 0; i < 20; i++) // try multiple times
        {
            // pick random direction around player
            Vector2 randomDir = Random.insideUnitCircle.normalized;
            float distance = Random.Range(minDistance, maxDistance);

            Vector3 candidate = player.position + new Vector3(randomDir.x, 0, randomDir.y) * distance;

            // check if inside camera view
            Vector3 viewport = mainCamera.WorldToViewportPoint(candidate);

            bool isVisible =
                viewport.z > 0 &&
                viewport.x > 0 && viewport.x < 1 &&
                viewport.y > 0 && viewport.y < 1;

            if (isVisible)
                continue; // try again

            // snap to NavMesh
            if (NavMesh.SamplePosition(candidate, out NavMeshHit hit, navMeshSearchRadius, NavMesh.AllAreas))
            {
                GameObject police = Instantiate(policePrefab, hit.position, Quaternion.identity);

                // assign player target
                PoliceChase chase = police.GetComponent<PoliceChase>();
                if (chase != null)
                {
                    chase.player = player;
                }

                return;
            }
        }

        Debug.Log("Failed to find spawn position");
    }
}