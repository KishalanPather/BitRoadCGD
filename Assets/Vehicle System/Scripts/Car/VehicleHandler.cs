using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleHandler : MonoBehaviour
{

    [SerializeField]
    Rigidbody rb;

    public float accelerationMultiplier = 15f;
    public float brakeMultiplier = 2f;
    public float steeringMultiplier = 50f;
    public float baseSpeed = 6f;
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
            // Calculate speed-adjusted steering
            float currentSpeed = rb.linearVelocity.magnitude;
            float speedFactor = Mathf.Lerp(1f, 0.3f, currentSpeed / 30f); // Adjust 30f to match max speed
            float turnAngle = steeringMultiplier * input.x * speedFactor * Time.fixedDeltaTime;

            // Apply both rotation and velocity redirection
            Quaternion turnRotation = Quaternion.Euler(0, turnAngle, 0);
            rb.MoveRotation(rb.rotation * turnRotation);

            // Align velocity with forward direction
            rb.linearVelocity = transform.forward * rb.linearVelocity.magnitude;
        }

    }

    public void SetInput(Vector2 inputVector)
    {
        inputVector.Normalize();
        input = inputVector;
    }
}