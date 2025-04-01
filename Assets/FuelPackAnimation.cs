using UnityEngine;

public class FuelPackAnimation : MonoBehaviour
{
    public float rotationSpeed = 100f;  // Rotation speed
    public float pulseScale = 0.1f;  // How much it scales up and down
    public float pulseSpeed = 2f;  // How fast the pulsing occurs
    private Vector3 initialScale;
    private Vector3 startPos;

    void Start()
    {
        initialScale = transform.localScale; // Store the original scale
        startPos = transform.position;
    }

    void Update()
    {
        transform.position = new Vector3(startPos.x, 0.15f, startPos.z);
        // Smooth rotation
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
        // Subtle pulsing effect
        float scaleFactor = 1 + Mathf.Sin(Time.time * pulseSpeed) * pulseScale;
        transform.localScale = initialScale * scaleFactor;
    }
}
