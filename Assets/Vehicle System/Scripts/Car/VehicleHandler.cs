using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleHandler : MonoBehaviour
{
    [SerializeField] Rigidbody rb;


    //handling stuff
    float accelerationMultiplier = 1.5f;
    float brakeMultiplier = 2f;
    float steeringMultiplier = 50f;
    float maxSpeed = 150f;

    //Power up stuff
    float maxFuel = 100f;
    float currentFuel;
    bool isBoosted = false;
    bool gameOverTriggered = false;

    [Header("Fuel Consumption Settings")]
    [SerializeField] private float baseFuelConsumption = 1f;   // Fuel drain when moving
    [SerializeField] private float speedFuelFactor = 2f;      // Fuel burn per unit speed
    [SerializeField] private float accelerationFuelFactor = 3f; // Fuel burn per acceleration input
    [SerializeField] private float idleFuelConsumption = 1f;   // Fuel burn when stationary but running

    // Boost settings
    [SerializeField] float boostAccelerationMultiplier = 10f; // Increased force during boost
    [SerializeField] float boostFuelConsumptionMultiplier = 0.2f; // 20% of normal fuel usage during boost
    [SerializeField] float boostDuration = 5f; // Boost lasts 5 seconds
    [SerializeField] private float speedBoostMultiplier = 3f; 

    [Header("Efficient Driving Settings")]
    [SerializeField] private float efficientFuelThreshold = 0.03f;   // Frame fuel usage must be below this to count as efficient
    [SerializeField] private float efficientStreakRequired = 5f;    // Seconds of efficient driving required to boost score
    [SerializeField] private float efficientMultiplier = 2f;      // Score multiplier for efficient driving
    private float efficientStreakTimer = 0f;
    Vector2 input = Vector2.zero;
    private float lastSpeed = 0f;

    void Start()
    {
        if (rb == null)
        {
            rb = GetComponent<Rigidbody>();
        }
        currentFuel = maxFuel;
    }

    private void FixedUpdate()
    {
        if (!GameManager.Instance.IsGameActive()) return;

        float currentSpeed = GetCurrentSpeed();
        float acceleration = (currentSpeed - lastSpeed) / Time.fixedDeltaTime;
        lastSpeed = currentSpeed;

        if (currentFuel <= 0)
        {
            if (!gameOverTriggered)
            {
                gameOverTriggered = true;
                StartCoroutine(TriggerGameOver());
            }
            return;
        }

        if (input.y > 0)
        {
            if(isBoosted)
                BoostAccelerate();
            else
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
        
        float usedFuel = ConsumeFuel(currentSpeed, acceleration);
        //Debug.Log("Fuel Used: " + usedFuel);

        // Check for efficient driving (fuel consumption low)
      
        if (usedFuel < efficientFuelThreshold && GetCurrentSpeed() >= 65 && transform.position.z > 100f)
        {
            efficientStreakTimer += Time.fixedDeltaTime;
            Debug.Log("Efficient Driving");
        }
        else
        {
            efficientStreakTimer = 0f;
        }

        EnforceMaxSpeed();
    }

    void Accelerate()
    {
        rb.linearDamping = 0;
        rb.AddForce(accelerationMultiplier * input.y * rb.transform.forward);
    }

    void BoostAccelerate()
    {
        rb.linearDamping = 0;
        rb.AddForce(boostAccelerationMultiplier * input.y * rb.transform.forward);
    }

    void Brake()
    {
        if (rb.linearVelocity.z <= 0)
        {
            return;
        }
        rb.linearDamping = brakeMultiplier;
    }

    void Steer()
    {
        if (Mathf.Abs(input.x) > 0)
        {
            // Calculate speed-adjusted steering
            float currentSpeed = rb.linearVelocity.magnitude;
            float speedFactor = Mathf.Lerp(1f, 0.3f, currentSpeed / 30f); // Adjust 30f to match your max speed
            float turnAngle = steeringMultiplier * input.x * speedFactor * Time.fixedDeltaTime;
            
            // Apply both rotation and velocity redirection
            Quaternion turnRotation = Quaternion.Euler(0, turnAngle, 0);
            rb.MoveRotation(rb.rotation * turnRotation);
            
            // Align velocity with forward direction
            rb.linearVelocity = transform.forward * rb.linearVelocity.magnitude;
        }
    }

    /// <summary>
    /// Consumes fuel based on speed, acceleration, and base idle usage.
    /// </summary>
    float ConsumeFuel(float speed, float acceleration)
    {
        if(isBoosted) return 0f;

        float fuelUsed = baseFuelConsumption + (speed * speedFuelFactor) + (Mathf.Max(0,acceleration) * accelerationFuelFactor);

        // If the vehicle is stationary, apply idle fuel consumption
        if (speed < 0.1f)
        {
            fuelUsed = idleFuelConsumption;
        }

        //Debug.Log("Fuel Used: " + fuelUsed * Time.fixedDeltaTime * 10);
        currentFuel -= fuelUsed * Time.fixedDeltaTime * 10;
        currentFuel = Mathf.Max(currentFuel, 0); // Prevent negative fuel
        float frameFuelUsed = fuelUsed * Time.fixedDeltaTime * 10;
        return frameFuelUsed;
    }

    /// <summary>
    /// Gradually decelerates the car and then triggers game over.
    /// </summary>
    private IEnumerator TriggerGameOver()
    {
        float decelerationTime = 2f;
        float timer = 0f;
        Vector3 initialVelocity = rb.linearVelocity;
        while(timer < decelerationTime)
        {
            timer += Time.fixedDeltaTime;
            rb.linearVelocity = Vector3.Lerp(initialVelocity, Vector3.zero, timer / decelerationTime);
            yield return new WaitForFixedUpdate();
        }
        rb.linearVelocity = Vector3.zero;
        GameManager.Instance.EndGame();
    }

        /// <summary>
    /// Enforces the maximum speed limit.
    /// </summary>
    void EnforceMaxSpeed()
    {
        float currentSpeed = GetCurrentSpeed();
        if (currentSpeed > maxSpeed && !isBoosted)
        {
            rb.linearVelocity = rb.linearVelocity.normalized * (maxSpeed / 3.6f); // Convert to m/s
        }
    }

    /// <summary>
    /// Returns the current fuel efficiency as a ratio (0 to 1).
    /// </summary>
    public float GetFuelEfficiency()
    {
        return currentFuel / maxFuel;
    }

    public float GetCurrentFuel()
    {
        return currentFuel;
    }

    /// <summary>
    /// Returns the current speed in KM/H.
    /// </summary>
    public float GetCurrentSpeed()
    {
        return rb.linearVelocity.magnitude * 3.6f;
    }

    /// <summary>
    /// Refuels the vehicle by a specified amount.
    /// </summary>
    public void Refuel(float amount)
    {
        currentFuel = Mathf.Min(currentFuel + amount, maxFuel);
    }

    public void SetInput(Vector2 inputVector)
    {
        inputVector.Normalize();
        input = inputVector;
    }

    /// <summary>
    /// Activates the speed boost for a limited duration.
    /// This method can be called by your speed boost power-up class.
    /// </summary>
    public void ActivateSpeedBoost()
    {
        if (!isBoosted)
        {
            StartCoroutine(SpeedBoostCoroutine());
        }
    }

    private IEnumerator SpeedBoostCoroutine()
    {
        isBoosted = true;
        Debug.Log("Speed Boost Activated!");

        // Optionally, you can do any visual or audio effects here

        yield return new WaitForSeconds(boostDuration);

        isBoosted = false;
        Debug.Log("Speed Boost Ended.");
    }

        /// <summary>
    /// Returns the current score multiplier based on efficient driving and boost status.
    /// </summary>
    public float GetScoreMultiplier()
    {
        float multiplier = 1f;
        if (efficientStreakTimer >= efficientStreakRequired)
        {
            multiplier *= efficientMultiplier;
        }
        // If boost is active and the vehicle is near its max speed, apply an additional multiplier.
        if (isBoosted && GetCurrentSpeed() >= maxSpeed * 0.9f)
        {
            multiplier *= speedBoostMultiplier;
        }
        return multiplier;
    }
}