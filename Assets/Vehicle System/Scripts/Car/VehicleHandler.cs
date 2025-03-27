using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleHandler : MonoBehaviour
{

    [SerializeField]
    Rigidbody rb;

    public float accelerationMultiplier = 1.5f;
    public float brakeMultiplier = 2f;
    public float steeringMultiplier = 50f;
    public float baseSpeed = 1f;
    Vector2 input = Vector2.zero;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        MaintainBaseSpeed();
        if (input.y > 0)
        {
            Accelerate();
        }
        else
        {
            rb.linearDamping = 0.3f;
        }

        if (input.y < 0)
        {
            Brake();
        }

        Steer();
    }

    void MaintainBaseSpeed()        //starts off at a constant speed 
    {
        Vector3 velocity = rb.linearVelocity;
        velocity.z = Mathf.Max(velocity.z, baseSpeed); // Ensures it never slows below base speed
        rb.linearVelocity = velocity;
    }

    void Accelerate()
    {
        //rb.linearDamping = 0;
        //rb.AddForce(rb.transform.forward * accelerationMultiplier * input.y);

        //only accelerates when needed
        rb.AddForce(transform.forward * accelerationMultiplier * input.y, ForceMode.Acceleration);
    }

    void Brake()
    {
        if (rb.linearVelocity.z <= 0)
        {
            return;
        }

        rb.linearDamping = brakeMultiplier;
        //rb.AddForce(brakeMultiplier * input.y * rb.transform.forward);
    }

    void Steer()
    {
        //float initialTorque = 50;
        if (Mathf.Abs(input.x) > 0)
        {
            //rb.AddForce(rb.transform.right * steeringMultiplier * input.x);
            float turnSpeed = steeringMultiplier * input.x * Time.fixedDeltaTime;
            transform.Rotate(Vector3.up, turnSpeed);

            //float torqueMultiplier = initialTorque * Time.deltaTime;
            //rb.AddTorque(transform.right * torqueMultiplier);
        }

    }

    public void SetInput(Vector2 inputVector)
    {
        inputVector.Normalize();
        input = inputVector;
    }
}
