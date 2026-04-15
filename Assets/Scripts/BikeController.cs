using System;
using UnityEngine;
using UnityEngine.ProBuilder.Shapes;

public class BikeController : MonoBehaviour
{
    RaycastHit hit;
    float moveInput, steerInput, rayLenght, currentVelocityOffset;

    [HideInInspector] public Vector3 velocity;

    public float maxSpeed, acceleration, steerStrenght, tiltAngle, gravity, bikeXTiltIncrement = .9f, zTiltAngle = 45f, handleRotVal = 30f , handleRotSpeed = .15f ;
    public float skidWidth = .062f, minSkidVelocity = 10f, tyreRotSpeed = 10000f;
    public float norDrag = 2f, driftDrag = 0.5f; 
    public Rigidbody sphereRB, BikeBody;
    public GameObject Handle;
    public GameObject FrontTyre;
    public GameObject BackTyre;
    public TrailRenderer skidMarks;
   [ Range(1,10)]
    public float breakingFactor;
    public LayerMask DrivableSurface;
    public AnimationCurve turningCurve;

    void Start()
    {

        sphereRB.transform.parent = null;
        BikeBody.transform.parent = null;
        rayLenght = sphereRB.GetComponent<SphereCollider>().radius + .2f;

        //visuals
        skidMarks.startWidth = skidWidth;
        skidMarks.emitting = false;
        FrontTyre.transform.Rotate(Vector3.right, Time.deltaTime * tyreRotSpeed * currentVelocityOffset);
        BackTyre.transform.Rotate(Vector3.right, Time.deltaTime * tyreRotSpeed * currentVelocityOffset);
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
        }
        else
        {
            Gravity();

        }
        BikeTilt();
      
    }

    void Acceleration()
    {
        sphereRB.linearVelocity = Vector3.Lerp(sphereRB.linearVelocity, maxSpeed * moveInput * transform.forward, Time.fixedDeltaTime * acceleration);
    }

    void Rotation()
    {
        transform.Rotate(0,steerInput * moveInput * turningCurve.Evaluate(Mathf.Abs(currentVelocityOffset)) * steerStrenght *Time.fixedDeltaTime,0 , Space.World);
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
        float radius = rayLenght - 0.02f;
        Vector3 origin = sphereRB.transform.position + radius * Vector3.up;

        if (Physics.SphereCast(origin, radius + 0.02f, -transform.up , out hit, rayLenght, DrivableSurface))
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
}
