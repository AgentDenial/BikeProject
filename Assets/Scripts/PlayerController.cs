using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Raycasts")]
    RaycastHit hit;
    private float rayLength;
    [SerializeField]
    private LayerMask DrivableSurface;

    [Header("RigidBodies")]
    [SerializeField]
    private Rigidbody sphereRB;
    [SerializeField]
    private Rigidbody bikeBody;

    [Header("Inputs")]
    private float moveInput;
    private float steerInput;

    [Header("Speed")]
    [SerializeField]
    private float maxSpeed;
    [SerializeField]
    private float acceleration;

    [Header("Steering")]
    [SerializeField]
    private float steerStrength;

    [Header("Gravity")]
    [SerializeField]
    private float gravity;

    [Header("Tilting")]
    [SerializeField]
    private float zTiltAngle;
    [SerializeField]
    private float bikeTiltIncrement;
    private float currentVelocityOffset;
    private Vector3 currentVelocity;

    [Header("Breaking")]
    [Range(0, 1)] // Changed to 0-1 for better multiplication logic
    public float breakingFactor = 0.95f;

    [Header("Drift Settings")]
    public float driftSteerMultiplier = 2.0f;
    public float driftTraction = 0.02f;

    [Header("Tests")]
    [SerializeField]
    private bool isGrounded;

    private void Start()
    {
        // Detach physics from parent to allow free rolling
        sphereRB.transform.parent = null;
        bikeBody.transform.parent = null;

        rayLength = sphereRB.GetComponent<SphereCollider>().radius + .7f;
    }

    private void Update()
    {
        // Getting raw input for arcade feeling
        moveInput = Input.GetAxisRaw("Vertical");
        steerInput = Input.GetAxisRaw("Horizontal");

        // Main container follows the sphere position
        transform.position = sphereRB.transform.position;

        currentVelocity = bikeBody.transform.InverseTransformDirection(sphereRB.linearVelocity);
        currentVelocityOffset = currentVelocity.z / maxSpeed;
    }

    private void FixedUpdate()
    {
        Movement();
    }

    void Acceleration()
    {
        if (Mathf.Abs(moveInput) > 0.01f && !Input.GetKey(KeyCode.Space))
        {
            Vector3 moveDir = transform.forward;

            if (isGrounded)
            {
                moveDir = Vector3.ProjectOnPlane(transform.forward, hit.normal).normalized;//ProjectOnPlane helps
                //us to mantain our direction and projects it accord to the ramp inclination in parallel
            }

            sphereRB.AddForce(moveDir * moveInput * acceleration, ForceMode.Acceleration);
        }

        //This helps us for the bike traction
        if (sphereRB.linearVelocity.magnitude > 1f) 
        {
            float currentTraction = Input.GetKey(KeyCode.Space) ? driftTraction : 0.1f;

            // We allign the velocity with the ramp direction or inclination
            Vector3 targetVelocityDir = isGrounded ? Vector3.ProjectOnPlane(transform.forward, hit.normal).normalized : transform.forward;

            sphereRB.linearVelocity = Vector3.Lerp(sphereRB.linearVelocity, targetVelocityDir * sphereRB.linearVelocity.magnitude, currentTraction);
        }

        //Top Spped
        if (sphereRB.linearVelocity.magnitude > maxSpeed)
            sphereRB.linearVelocity = sphereRB.linearVelocity.normalized * maxSpeed;
    }

    void Movement()
    {
        isGrounded = Grounded();

        if (isGrounded)
        {
            Rotation();

            if (!Input.GetKey(KeyCode.Space))
            {
                Acceleration();
            }

            Brake();
        }
        else
        {
            Gravity();
        }

        BikeTilt();
    }

    void Rotation()
    {
        float currentSteer = steerStrength;

        //Hre we combine the steer strenght with the drifting 
        if (Input.GetKey(KeyCode.Space))
        {
            currentSteer *= 1.5f;
        }

        float turn = steerInput * currentSteer * Time.fixedDeltaTime;

        // We only rotate if we are moving
        if (sphereRB.linearVelocity.magnitude > 1f)
        {
            transform.Rotate(0, turn, 0, Space.World);
        }
    }

    void Brake()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            //Drift momentum
            sphereRB.linearVelocity *= 0.98f;
        }
    }

    bool Grounded()
    {
        Vector3 origin = sphereRB.position;
        // Raycast logic to detect the floor
        if (Physics.Raycast(origin, Vector3.down, out hit, rayLength, DrivableSurface))
        {
            Debug.DrawRay(origin, Vector3.down * rayLength, Color.green);
            isGrounded = true;
            return true;
        }
        else
        {
            Debug.DrawRay(origin, Vector3.down * rayLength, Color.red);
            isGrounded = false;
            return false;
        }
    }

    void Gravity()
    {
        // Custom gravity force to prevent floating
        float gravityMultiplier = (sphereRB.linearVelocity.y < 0) ? 1.5f : 1.0f;
        sphereRB.AddForce(Vector3.up * gravity, ForceMode.Acceleration);
    }

    void BikeTilt()
    {
        // Align visual model with ground normal and apply steering lean
        float xRot = (Quaternion.FromToRotation(bikeBody.transform.up, hit.normal) * bikeBody.transform.rotation).eulerAngles.x;
        float zRot = 0;

        if (currentVelocityOffset > 0.1f)
        {
            zRot = -zTiltAngle * steerInput * currentVelocityOffset;
        }

        Quaternion targetRot = Quaternion.Slerp(bikeBody.transform.rotation, Quaternion.Euler(xRot, transform.eulerAngles.y, zRot), bikeTiltIncrement);
        Quaternion newRotation = Quaternion.Euler(targetRot.eulerAngles.x, transform.eulerAngles.y, targetRot.eulerAngles.z);

        bikeBody.MoveRotation(newRotation);
    }
}