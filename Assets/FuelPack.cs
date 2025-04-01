using System.Collections;
using UnityEngine;

public class FuelPack : MonoBehaviour
{
    public float fuelAmount = 20f; // Fuel restored
    public float spinSpeed = 1440f; // Rotation speed when collected
    public float destroyDelay = 0.5f; // Time before it's destroyed
    public float riseHeight = 1.25f;
    public AudioClip collectSound; // Sound effect for collection

    private AudioSource audioSource;
    private bool isCollected = false;

    private void Start()
    {
        // Get or add an AudioSource component
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        audioSource.playOnAwake = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isCollected)
        {
            isCollected = true;

            Debug.Log("Fuel Pack Collected!");
            VehicleHandler vehicle = other.GetComponent<VehicleHandler>();
            if (vehicle != null)
            {
                vehicle.Refuel(fuelAmount);
            }

            if (collectSound != null)
            {
                audioSource.PlayOneShot(collectSound);
            }

            StartCoroutine(CollectAnimation());
        }
    }

    private IEnumerator CollectAnimation()
    {
        float elapsedTime = 0f;
        Vector3 startPos = transform.position;

        while (elapsedTime < destroyDelay)
        {
            transform.Rotate(Vector3.up * spinSpeed * Time.deltaTime, Space.World);
            transform.position = Vector3.Lerp(startPos, startPos + Vector3.up * riseHeight, elapsedTime / destroyDelay);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        Destroy(gameObject);
    }
}
