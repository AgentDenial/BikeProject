using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Raycasts")]
    private RaycastHit hit;
    private float rayLength;
    [SerializeField] private LayerMask DrivableSurface;

    [Header("RigidBodies")]
    [SerializeField] private Rigidbody sphereRB; // La esfera f�sica
    [SerializeField] private GameObject bikeBody; // El modelo visual de la moto

    [Header("Inputs")]
    private float moveInput;
    private float steerInput;

    [Header("Speed")]
    [SerializeField] private float maxSpeed = 50f;
    [SerializeField] private float acceleration = 30f;

    [Header("Steering")]
    [SerializeField] private float steerStrength = 100f;

    [Header("Gravity")]
    [SerializeField] private float gravity = -80f;

    [Header("Tilting")]
    [SerializeField] private float zTiltAngle = 45f;
    [SerializeField] private float bikeTiltIncrement = 0.1f;
    private float currentVelocityOffset;

    [Header("Drift Settings")]
    public float driftTraction = 0.02f;

    [Header("Status")]
    [SerializeField] private bool isGrounded;

    private void Start()
    {
        // Separamos la esfera para que no rote el contenedor
        sphereRB.transform.parent = null;

        // Ajustamos el rayo seg�n el radio de tu esfera
        rayLength = sphereRB.GetComponent<SphereCollider>().radius + 0.2f;
    }

    private void Update()
    {
        // Obtener Inputs
        moveInput = Input.GetAxisRaw("Vertical");
        steerInput = Input.GetAxisRaw("Horizontal");

        // El contenedor sigue a la esfera
        transform.position = sphereRB.transform.position;

        // Factor de velocidad para inclinaci�n y giro
        currentVelocityOffset = sphereRB.linearVelocity.magnitude / maxSpeed;
    }

    private void FixedUpdate()
    {
        Movement();
    }

    void Movement()
    {
        isGrounded = Grounded();

        if (isGrounded)
        {
            Acceleration();
            Rotation();
            Brake();
        }
        else
        {
            Gravity();
        }

        BikeTilt();
    }

    void Acceleration()
    {
        // 1. L�gica de Empuje (Adelante/Atr�s)
        if (Mathf.Abs(moveInput) > 0.01f && !Input.GetKey(KeyCode.Space))
        {
            // Verificamos si la moto ya se mueve hacia adelante
            float forwardSpeed = Vector3.Dot(transform.forward, sphereRB.linearVelocity);

            // Si presionas S pero vas r�pido hacia adelante, NO aceleramos (frenamos primero)
            bool isBraking = (moveInput < -0.1f && forwardSpeed > 1f);

            if (!isBraking)
            {
                Vector3 moveDir = Vector3.ProjectOnPlane(transform.forward, hit.normal).normalized;
                sphereRB.AddForce(moveDir * moveInput * acceleration, ForceMode.Acceleration);
            }
        }

        // 2. Tracci�n y Alineaci�n de velocidad
        if (sphereRB.linearVelocity.magnitude > 1f)
        {
            float currentTraction = Input.GetKey(KeyCode.Space) ? driftTraction : 0.15f;
            Vector3 targetVelocityDir = isGrounded ? Vector3.ProjectOnPlane(transform.forward, hit.normal).normalized : transform.forward;

            sphereRB.linearVelocity = Vector3.Lerp(sphereRB.linearVelocity, targetVelocityDir * sphereRB.linearVelocity.magnitude, currentTraction);
        }

        // 3. Fricci�n autom�tica cuando no hay input
        if (Mathf.Abs(moveInput) < 0.01f)
        {
            sphereRB.linearVelocity = Vector3.Lerp(sphereRB.linearVelocity, Vector3.zero, Time.fixedDeltaTime * 2f);
        }

        // L�mite de Velocidad
        if (sphereRB.linearVelocity.magnitude > maxSpeed)
            sphereRB.linearVelocity = sphereRB.linearVelocity.normalized * maxSpeed;
    }

    void Rotation()
    {
        float currentSteer = steerStrength;
        if (Input.GetKey(KeyCode.Space)) currentSteer *= 1.5f;

        // Solo giramos si tenemos algo de velocidad
        float turn = steerInput * currentSteer * Time.fixedDeltaTime * Mathf.Clamp01(currentVelocityOffset * 2f);
        transform.Rotate(0, turn, 0, Space.World);
    }

    void Brake()
    {
        // HANDBRAKE (Drift)
        if (Input.GetKey(KeyCode.Space))
        {
            sphereRB.linearVelocity *= 0.98f;
        }

        // FULL STOP (Presionar S mientras vas hacia adelante)
        float forwardSpeed = Vector3.Dot(transform.forward, sphereRB.linearVelocity);
        if (moveInput < -0.1f && forwardSpeed > 0.5f)
        {
            sphereRB.linearVelocity = Vector3.Lerp(sphereRB.linearVelocity, Vector3.zero, Time.fixedDeltaTime * 10f);
        }
    }

    bool Grounded()
    {
        if (Physics.Raycast(sphereRB.position, Vector3.down, out hit, rayLength, DrivableSurface))
        {
            Debug.DrawRay(sphereRB.position, Vector3.down * rayLength, Color.green);
            return true;
        }
        Debug.DrawRay(sphereRB.position, Vector3.down * rayLength, Color.red);
        return false;
    }

    void Gravity()
    {
        float multiplier = (sphereRB.linearVelocity.y < 0) ? 2.0f : 1.0f;
        sphereRB.AddForce(Vector3.up * gravity * multiplier, ForceMode.Acceleration);
    }

    void BikeTilt()
    {
        if (isGrounded)
        {
            // 1. Calculamos la inclinaci�n frontal (X) basada en la rampa
            // Usamos transform.InverseTransformDirection para que la inclinaci�n sea relativa a la moto
            Vector3 localNormal = transform.InverseTransformDirection(hit.normal);
            float xRot = Mathf.Atan2(localNormal.z, localNormal.y) * Mathf.Rad2Deg;

            // 2. Calculamos la inclinaci�n lateral (Z)
            // Multiplicamos por currentVelocityOffset para que no se incline si est� parada
            float zRot = -zTiltAngle * steerInput * currentVelocityOffset;

            // 3. Creamos la rotaci�n local objetivo
            // Mantenemos Y en 0 porque el padre (PlayerBike) ya maneja la direcci�n
            Quaternion targetLocalRot = Quaternion.Euler(xRot, 0, zRot);

            // 4. Aplicamos el Slerp (Aumenta bikeTiltIncrement en el inspector para m�s respuesta)
            bikeBody.transform.localRotation = Quaternion.Slerp(bikeBody.transform.localRotation, targetLocalRot, bikeTiltIncrement);
        }
        else
        {
            // En el aire, regresamos a la posici�n neutral r�pido para aterrizar bien
            bikeBody.transform.localRotation = Quaternion.Slerp(bikeBody.transform.localRotation, Quaternion.identity, Time.deltaTime * 5f);
        }
    }
}