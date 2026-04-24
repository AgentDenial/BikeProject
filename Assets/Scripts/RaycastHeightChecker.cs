using UnityEngine;

public class RaycastHeightChecker : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        HeightCheck();
    }

    void HeightCheck()
    {
        Ray ray = new Ray(transform.position, Vector3.down);
        RaycastHit info;
        if(Physics.Raycast(ray, out info))
        {
            Debug.Log($"Hit {info.transform.name}: Y = {info.point.y}");
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawRay(transform.position, Vector3.down);
    }
}
