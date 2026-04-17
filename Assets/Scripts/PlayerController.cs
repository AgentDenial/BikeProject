using UnityEngine;
using UnityEngine.InputSystem;

//youtube example code

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
    [SerializeField]
    private float reverseSpeed;

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
    [Range(1, 10)]
    public float breakingFactor;

    [Header("Tests")]
    [SerializeField]
    private bool isGrounded;

    private void Start()
    {
        sphereRB.transform.parent = null;
        bikeBody.transform.parent = null;
        //moveInput = Input.GetAxis("Horizontal");

        rayLength = sphereRB.GetComponent<SphereCollider>().radius + .07f;
    }

    private void Update()
    {
        moveInput = Input.GetAxis("Vertical");
        steerInput = Input.GetAxis("Horizontal");
        transform.position = sphereRB.transform.position;

        currentVelocity = bikeBody.transform.InverseTransformDirection(sphereRB.linearVelocity);

        float speedReference = currentVelocity.z >= 0 ? maxSpeed : reverseSpeed;
        currentVelocityOffset = Mathf.Clamp(currentVelocity.z / speedReference, -1f, 1f);

    }

    private void FixedUpdate()
    {
        //Acceleration();
        //Rotation();
        //Brake();
        Movement();
    }

    void Acceleration()
    {
        float targetSpeed = 0f;

        if (moveInput > 0)
            targetSpeed = maxSpeed;
        else if (moveInput < 0)
            targetSpeed = reverseSpeed;

        Vector3 targetVelocity = transform.forward * moveInput * targetSpeed;

        sphereRB.linearVelocity = Vector3.Lerp(
            sphereRB.linearVelocity,
            targetVelocity,
            Time.fixedDeltaTime * acceleration
        );
    }

    void Movement()
    {
        if (Grounded())
        {
            if (!Input.GetKey(KeyCode.Space))
            {
                Acceleration();
                Rotation();
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
        transform.Rotate(0, steerInput * steerStrength * moveInput * Time.fixedDeltaTime, 0, Space.World);
    }

    void Brake()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            sphereRB.linearVelocity *= breakingFactor / 10;
        }
    }

    bool Grounded()
    {
        /*Debug.DrawRay(origin, Vector3.down = checkDistance, Color.red, 0.1f);
        if (Physics.Raycast(sphereRB.position, Vector3.down, out hit, rayLength, DrivableSurface))
        {
            Debug.Log("grounded");
            isGrounded = true;
            return true;
            
        }
        else
        {
            Debug.Log("Not Grounded");
            isGrounded = false; 
            return false;
        }*/
        SphereCollider sphereCol = sphereRB.GetComponent<SphereCollider>();
        Vector3 origin = sphereRB.position;
        float distance = sphereCol.radius + 0.7f;   // generous for testing

        // Visual debug ray (red = not hitting, green = hitting)
        if (Physics.Raycast(origin, Vector3.down, out hit, distance, DrivableSurface))
        {
            Debug.DrawRay(origin, Vector3.down * distance, Color.green);
            Debug.Log(" Grounded! Distance: " + hit.distance + " | Normal: " + hit.normal);
            isGrounded = true;
            return true;
        }
        else
        {
            Debug.DrawRay(origin, Vector3.down * distance, Color.red);
            Debug.Log(" Not Grounded");
            isGrounded = false;
            return false;
        }

    }

    void Gravity()
    {
        sphereRB.AddForce(gravity * Vector3.down, ForceMode.Acceleration);
    }

    void BikeTilt()
    {
        float xRot = (Quaternion.FromToRotation(bikeBody.transform.up, hit.normal) *bikeBody.transform.rotation).eulerAngles.x;
        float zRot = -zTiltAngle * steerInput * Mathf.Abs(currentVelocityOffset);
        

        Quaternion targetRot = Quaternion.Slerp(bikeBody.transform.rotation, Quaternion.Euler(xRot, transform.eulerAngles.y, zRot), bikeTiltIncrement);

        //Quaternion newRotation = Quaternion.Euler(xRot, transform.eulerAngles.y, transform.eulerAngles.z);
        Quaternion newRotation = Quaternion.Euler(targetRot.eulerAngles.x, transform.eulerAngles.y, targetRot.eulerAngles.z);
        bikeBody.MoveRotation(newRotation);
    }
}

/*
 * old code
 * public class PlayerController : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    GameObject Player;
    Rigidbody rb;
    public InputActionAsset InputActions;

    private InputAction m_moveAction;
    private InputAction m_lookAction;
    private InputAction m_jumpAction;
    private InputAction m_attackAction;

    private Vector2 m_moveAmt;
    private Vector2 m_lookAmt; // might not be needed

    private int speedUpgrades;
    [SerializeField] private float movementSpeed;
    [SerializeField] private bool isGrounded;
    [SerializeField] private float jumpForce;
    [SerializeField] private float rotateSpeed;

    private void OnEnable()
    {
        InputActions.FindActionMap("Player").Enable();
    }

    private void OnDisable()
    {
        InputActions.FindActionMap("Player").Disable();
    }
    private void Awake()
    {
        if (rb == null)
        {
            rb = GetComponent<Rigidbody>();
            Debug.Log("MissingPlayerFound");
        }
        m_moveAction = InputSystem.actions.FindAction("Move");
        m_lookAction = InputSystem.actions.FindAction("Look");
        m_jumpAction = InputSystem.actions.FindAction("Jump");
        m_attackAction = InputSystem.actions.FindAction("Attack");
    }
    void Start()
    {


    }

    // Update is called once per frame
    void Update()
    {
        m_moveAmt = m_moveAction.ReadValue<Vector2>();
        m_lookAmt = m_lookAction.ReadValue<Vector2>();

        if (m_jumpAction.WasPressedThisFrame())
        {
            Jump();
        }
        if (m_attackAction.WasCompletedThisFrame())
        {
            Attack();
        }
    }

    public void FixedUpdate()
    {
        Moving();
        Rotating();
    }

    private void Moving()
    {
        rb.MovePosition(rb.position + transform.forward * m_moveAmt.y * movementSpeed * Time.deltaTime);
        //rb.MovePosition(rb.position + transform.right * m_moveAmt.x * movementSpeed * Time.deltaTime);
    }

    private void Rotating()
    {
        float rotationAmount = m_lookAmt.x * rotateSpeed * Time.deltaTime;
        Quaternion deltaRotation = Quaternion.Euler(0, rotationAmount, 0);
        rb.MoveRotation(rb.rotation * deltaRotation);
        //rb.transform.Rotate(new Vector3(0, rotationAmount, 0));
    }

    public void Attack()
    {
        Debug.Log("Pewpew");
    }
    public void Jump()
    {
        if (isGrounded == true)
        {
            //rb.AddForceAtPosition(new Vector3(0f, 5f, 0f), Vector3.up, ForceMode.Impulse);
            rb.AddForce(Vector2.up * jumpForce, ForceMode.Impulse);
            Debug.Log("Jumped");
        }
;
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
            Debug.Log("Grounded");
        }
    }
    private void OnCollisionExit(Collision other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
            Debug.Log("NotGrounded");
        }
    }
}*/

