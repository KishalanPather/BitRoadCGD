using System;
using UnityEngine;

public class SpeedBoostAnimation : MonoBehaviour
{
    public float rotationSpeed = 250f;  // Rotation speed
    public float pulseScale = 5f;  // How much it scales up and down
    public float pulseSpeed = 2f;  // How fast the pulsing occurs
    private Vector3 initialScale;
    private Vector3 startPos;

    void Start()
    {
        initialScale = transform.localScale;
        startPos = transform.position;
    }

    void Update()
    {
        transform.position = new Vector3(startPos.x, 0.1f, startPos.z);
        // Smooth rotation
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
        
        // Subtle pulsing effect
        float scaleFactor = 1 + Mathf.Sin(Time.time * pulseSpeed) * pulseScale;
        transform.localScale = initialScale * scaleFactor;
    }
}
