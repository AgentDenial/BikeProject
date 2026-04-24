using System;
using UnityEngine;

public class Speedometer : MonoBehaviour
{
    private const float MAX_SPEED_ANGLE = -142f;
    private const float ZERO_SPEED_ANGLE = 137f;

    private Transform needleTransform;

    private float speedMax;
    private float speed;

    private void Awake()
    {
        needleTransform = transform.Find("Needle");
        speed = 0f;
        speedMax = 200f;
    }

    private void Update()
    {
        HandlePayerInput();
        needleTransform.eulerAngles = new Vector3(0, 0, GetSpeedRotation());
    }

    private void HandlePayerInput()
    {
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            float acceleration = 50f;
            speed += acceleration * Time.deltaTime;
        }
        else
        {
            float deceleration = 200f;
            speed -= deceleration * Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.Space))
        {
            float breakSoeed = 100f;
            speed -= breakSoeed * Time.deltaTime;
        }

        speed = Math.Clamp(speed, 0f, speedMax);
    }

    private float GetSpeedRotation()
    {
        float totalAngleSize = ZERO_SPEED_ANGLE - MAX_SPEED_ANGLE;

        float speedNormalized = speed / speedMax;

        return ZERO_SPEED_ANGLE - speedNormalized * totalAngleSize;
    }
}
