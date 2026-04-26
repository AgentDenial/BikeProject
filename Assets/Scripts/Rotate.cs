using UnityEngine;

public class Rotate : MonoBehaviour
{
    
    public float speed = 10f; // Speed of rotation in degrees per second
    void Update()
    { 
        // Rotate around the Y-axis
        transform.Rotate(Vector3.up, speed * Time.deltaTime); 
    } 
}
