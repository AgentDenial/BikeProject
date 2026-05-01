using System;
using System.Collections;
using UnityEngine;
using UnityEngine.ProBuilder.Shapes;

public class BikeController : MonoBehaviour
{
    // --- Private / Internal ---
    RaycastHit hit;
    float moveInput, steerInput, rayLength, currentVelocityOffset;
    [HideInInspector] public Vector3 velocity;

    /*[Header("Player Health")]
    public float maxHealth = 100f;
    public float currentHealth;
    public float damageReceived = 5f;*/

    [Header("Main Physics Settings")]
    public float maxSpeed;
    public float newMaxSpeed;
    public float acceleration;
    public float gravity;
    public float maxReverseSpeed = 20f;
    public LayerMask DrivableSurface;
    public LayerMask OutOfBounds;

    [Header("Steering & Handling")]
    public float steerStrength;
    public AnimationCurve turningCurve;
    public float handleRotVal = 30f;
    public float handleRotSpeed = .15f;

    [Header("Drift & Traction")]
    public float norDrag = 2f;
    public float driftDrag = 0.5f;
    [Range(1, 10)]
    public float breakingFactor;

    [Header("Turbo settings")]
    private float originalMaxSpeed;
    private bool isTurboActive = false; 

    [Header("Bike Tilt Settings")]
    //public float tiltAngle;
    public float zTiltAngle = 45f;
    [Tooltip("Smoooth Rx")]
    public float bikeXTiltIncrement = .9f;

    [Header("Visuals & Tires")]
    public float tireRotSpeed = 10000f;
    public float skidWidth = .062f;
    public float minSkidVelocity = 10f;
    public TrailRenderer skidMarks;

    [Header("Scene References")]
    public Rigidbody sphereRB;
    public Rigidbody BikeBody;
    public GameObject Handle;
    public GameObject FrontTire;
    public GameObject BackTire;

    private Vector3 airVelocity;

    
    public MenuManager menuManager;

    void Start()
    {
        originalMaxSpeed = maxSpeed;
        //currentHealth = maxHealth;

        sphereRB.transform.parent = null;
        BikeBody.transform.parent = null;
        rayLength = sphereRB.GetComponent<SphereCollider>().radius + .2f;

        //visuals
        skidMarks.startWidth = skidWidth;
        skidMarks.emitting = false;
        FrontTire.transform.Rotate(Vector3.right, Time.deltaTime * tireRotSpeed * currentVelocityOffset);
        BackTire.transform.Rotate(Vector3.right, Time.deltaTime * tireRotSpeed * currentVelocityOffset);
    }

    // Update is called once per frame
    void Update()
    {
        moveInput = Input.GetAxis("Vertical");
        steerInput = Input.GetAxis("Horizontal");

        transform.position = sphereRB.transform.position;

        velocity = BikeBody.transform.InverseTransformDirection(BikeBody.linearVelocity);
        currentVelocityOffset = velocity.z / maxSpeed;
        
    }

    private void FixedUpdate()
    {
        Movement();

        //visuals
        SkidMarks();
    }

    void Movement()
    {
        if (Grounded())
        {
            if (!Input.GetKey(KeyCode.Space))
            {
                Acceleration();
                
            }
            Rotation();
            Break();
            airVelocity = sphereRB.linearVelocity;
        }
        else
        {
            Gravity();
            sphereRB.linearVelocity = new Vector3(airVelocity.x, sphereRB.linearVelocity.y, airVelocity.z);

        }
        BikeTilt();
      
    }

    void Acceleration()
    {
        float targetSpeed = moveInput >= 0 ? maxSpeed : maxReverseSpeed;
        Vector3 targetVelocity = targetSpeed * moveInput * transform.forward;

        sphereRB.linearVelocity = Vector3.Lerp(
            sphereRB.linearVelocity,
            targetVelocity,
            Time.fixedDeltaTime * acceleration
        );

        //Saul's Code
        // sphereRB.linearVelocity = Vector3.Lerp(sphereRB.linearVelocity, maxSpeed * moveInput * transform.forward, Time.fixedDeltaTime * acceleration); 
    }

    void Rotation()
    {
        //replaced moveInput with steerInput
        float steerMovement = Mathf.Clamp(sphereRB.linearVelocity.magnitude, 0, 1);
            transform.Rotate(0, steerInput * steerMovement * turningCurve.Evaluate(Mathf.Abs(currentVelocityOffset)) * steerStrength * Time.fixedDeltaTime, 0, Space.World);
            Handle.transform.localRotation = Quaternion.Slerp(Handle.transform.localRotation, Quaternion.Euler(Handle.transform.localRotation.eulerAngles.x, handleRotVal * steerInput, Handle.transform.localRotation.eulerAngles.z), handleRotSpeed);

    }

    void Break()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            sphereRB.linearVelocity *= breakingFactor / 10;
            sphereRB.linearDamping = driftDrag;
        }
        else
        {
            sphereRB.linearDamping = norDrag;
        }
    }

    bool Grounded()
    {
        float radius = rayLength - 0.02f;
        Vector3 origin = sphereRB.transform.position + radius * Vector3.up;

        if (Physics.SphereCast(origin, radius + 0.02f, -transform.up , out hit, rayLength, DrivableSurface))
        {
            return true;
        }

        else
        {
            return false;
        }
    }

    void Gravity()
    {
        sphereRB.AddForce(gravity * Vector3.down, ForceMode.Acceleration);
    }

    void BikeTilt()
    {
        float xRot = (Quaternion.FromToRotation(BikeBody.transform.up, hit.normal) * BikeBody.transform.rotation).eulerAngles.x;
        float zRot = 0;

        if (currentVelocityOffset > 0)
        {
            zRot = -zTiltAngle * steerInput * currentVelocityOffset;
        }

        Quaternion targerRot = Quaternion.Slerp(BikeBody.transform.rotation, Quaternion.Euler(xRot, transform.eulerAngles.y, zRot), bikeXTiltIncrement);
        Quaternion newRotation = Quaternion.Euler(targerRot.eulerAngles.x, transform.eulerAngles.y, targerRot.eulerAngles.z);
        BikeBody.MoveRotation(newRotation);
    }

    void SkidMarks()
    {
        if (Grounded() && Mathf.Abs(velocity.x) > minSkidVelocity || Input.GetKey(KeyCode.Space))
        {
            skidMarks.emitting = true;
        }

        else
        {
            skidMarks.emitting = false;
        }
    }

    public void ApplyTurbo(float boost)
    {
        Debug.Log("Detect");
        //StartCoroutine(TurboRoutine(boost));
        newMaxSpeed = maxSpeed += boost;
        originalMaxSpeed = newMaxSpeed;

    }

    //might be outdated due to removal of timer
    /*IEnumerator TurboRoutine(float boost)
    {
        //isTurboActive = true;
        maxSpeed += boost;
        //yield return new WaitForSeconds(maxSpeed);
        //maxSpeed = originalMaxSpeed;
        //isTurboActive = false;
    }*/

    private void OnCollisionEnter(Collision other)
    {
        //if (other.gameObject.layer == LayerMask.NameToLayer("OutOfBounds"))
        //if (other.gameObject.CompareTag("OutOfBounds"))
        Debug.Log("test");
        if (other.gameObject.name == "Plane")
        {
            Debug.Log("A");
        }

        if (other.gameObject.CompareTag("OutOfBounds"))
        {
            Debug.Log("B");
        }

        if (1 << other.gameObject.layer == 1 << LayerMask.NameToLayer("OutOfBounds"))
        {
            Debug.Log("C");
        }

        if (1 << other.gameObject.layer == 1 << LayerMask.NameToLayer("OutOfBounds"))
        {
            Debug.Log("Fell out of bounds");
            menuManager.YouLose();
        }
    }

    /*public void HandleCollision(Collision collision)
    {
        if(collision.gameObject.CompareTag("Enemy"))
        {
            Debug.Log("Damage taken: " + currentHealth);
            TakeDamage(5);
        }
    }

    private void TakeDamage(float damageAmount)
    {
        currentHealth -= damageAmount;
        Debug.Log("Life left: " + currentHealth);

        if (currentHealth <= 0)
        {
            Destroy(gameObject);
            //gameOverScene
        }
    }*/
}
