using UnityEngine;

public class FuelSpawner : MonoBehaviour
{
    public GameObject fuelPackPrefab; // Assign in Inspector
    public Transform[] spawnPoints; // Set different locations for spawning
    public float spawnInterval = 10f; // Time between spawns

    void Start()
    {
        InvokeRepeating(nameof(SpawnFuelPack), 2f, spawnInterval);
    }

    void SpawnFuelPack()
    {
        if (spawnPoints.Length == 0 || fuelPackPrefab == null) return;

        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
        Instantiate(fuelPackPrefab, spawnPoint.position, Quaternion.identity);
    }
}