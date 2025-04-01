using System.Collections;
using UnityEngine;

public class RoadGeneration : MonoBehaviour
{
    [SerializeField]
    GameObject startingSectionPrefab;

    [SerializeField]
    GameObject[] sectionPrefabs;

    [SerializeField]
    GameObject[] obstaclePrefabs;

    [SerializeField] GameObject fuelPackPrefab;
    [SerializeField] float initialFuelPackSpawnChance = 0.3f;
    [SerializeField] float minFuelPackSpawnChance = 0.1f;
    [SerializeField] float maxFuelPackAmount = 35f;
    [SerializeField] float minFuelPackAmount = 15f;
    [SerializeField] float progressDistanceForScaling = 10000f;

    [SerializeField] GameObject speedBoostPrefab;
    [SerializeField] float speedBoostSpawnChance = 0.1f;
    GameObject[] sectionsPool = new GameObject[15];

    GameObject[] sections = new GameObject[8];

    Transform vehicleTransform;

    WaitForSeconds waitTime = new WaitForSeconds(0.1f);

    const float sectionLength = 25;
    float obsSpawnRate = 3.5f; // Initial spawn rate
    float spawnRateDecrease = 0.1f; // Rate decrease over time
    float minSpawnRate = 1f; // Minimum spawn interval
    float lastSpawnTime;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        vehicleTransform = GameObject.FindGameObjectWithTag("Player").transform;
        
        int prefabIndex = 0;

        // create a pool for endless section
        for (int i = 0; i < sectionsPool.Length; i++)
        {
            sectionsPool[i] = Instantiate(sectionPrefabs[prefabIndex]);
            sectionsPool[i].SetActive(false);
            prefabIndex++;

            if (prefabIndex > sectionPrefabs.Length - 1)
            {
                prefabIndex = 0;
                
            }
        }

        GameObject firstSection = Instantiate(startingSectionPrefab);
        firstSection.transform.position = new Vector3(0, 0, 0);
        firstSection.SetActive(true);
        sections[0] = firstSection;


        // create sections
        for (int i = 1; i < sections.Length; i++)
        {
            GameObject randomSection = GetRandomSectionFromPool();

            randomSection.transform.position = new Vector3(sectionsPool[i].transform.position.x, 0, i * sectionLength);
            randomSection.SetActive(true);

            sections[i] = randomSection;
        }

        StartCoroutine(UpdateEndlessGeneration());
        StartCoroutine(IncreaseSpawnRate());
    }
    IEnumerator UpdateEndlessGeneration()
    {
        while (true)
        {
            UpdateSectionsPosition();
            yield return waitTime;
        }
    }

    void UpdateSectionsPosition()
    {
        for (int i = 0; i < sections.Length; i++)
        {
            if (sections[i].transform.position.z - vehicleTransform.position.z < -sectionLength)
            {
                Vector3 lastSectorPosition = sections[i].transform.position;
                GameObject oldSection = sections[i];
                sections[i].SetActive(false);

                sections[i] = GetRandomSectionFromPool();

                sections[i].transform.position = new Vector3(lastSectorPosition.x, 0, lastSectorPosition.z + sectionLength * sections.Length);
                sections[i].SetActive(true);

                //SpawnObstacles(sections[i]);
                if (sections[i].transform.position.z > 75f)
                {
                    SpawnFuelPack(sections[i]);
                    SpawnSpeedBoost(sections[i]);
                }
            }
        }
    }

    GameObject GetRandomSectionFromPool()
    {
        int rand = Random.Range(0, sectionsPool.Length);

        bool isNewSection = false;

        while (!isNewSection)
        {
            if (!sectionsPool[rand].activeInHierarchy)
            {
                isNewSection = true;
            }
            else
            {
                rand++;

                if (rand > sectionsPool.Length - 1)
                {
                    rand = 0;
                }
            }
        }

        return sectionsPool[rand];
    }

    void SpawnObstacles(GameObject section)
    {
        if (Time.time - lastSpawnTime >= obsSpawnRate)
        {
            int obstacleCount = Random.Range(1, 4); // Spawn 1 to 3 obstacles per section
            for (int i = 0; i < obstacleCount; i++)
            {
                GameObject obstacle = Instantiate(obstaclePrefabs[Random.Range(0, obstaclePrefabs.Length)]);
                float offsetX = Random.Range(-3f, 3f); // Adjust for obstacle placement
                float offsetZ = Random.Range(-sectionLength / 2, sectionLength / 2);
                obstacle.transform.position = section.transform.position + new Vector3(offsetX, 0, offsetZ);
            }
            lastSpawnTime = Time.time;
        }
    }

    void SpawnFuelPack(GameObject section)
    {
        float progressFactor = Mathf.Clamp01(vehicleTransform.position.z / progressDistanceForScaling);

        float currentSpawnChance = Mathf.Lerp(initialFuelPackSpawnChance, minFuelPackSpawnChance, progressFactor);

        if (Random.value <= currentSpawnChance)
        {
            GameObject fuelPack = Instantiate(fuelPackPrefab);

            float upperBound = Mathf.Lerp(maxFuelPackAmount, minFuelPackAmount, progressFactor);
            float fuelAmount = Random.Range(minFuelPackAmount, upperBound);

            FuelPack fp = fuelPack.GetComponent<FuelPack>();
            if (fp != null)
            {
                fp.fuelAmount = fuelAmount;
            }

            float offsetX = Random.Range(-0.175f, 0.175f);
            float offsetZ = Random.Range(-sectionLength / 2.5f, sectionLength / 2.5f);
            fuelPack.transform.position = section.transform.position + new Vector3(offsetX, 0f, offsetZ);
            
            Debug.Log("Fuel Pack spawned with " + fuelAmount + " fuel and chance: " + currentSpawnChance + "Current Progress: " + progressFactor);
        }
    }

        void SpawnSpeedBoost(GameObject section)
    {
        if (Random.value <= speedBoostSpawnChance)
        {
            GameObject speedBoost = Instantiate(speedBoostPrefab);
            float offsetX = Random.Range(-0.175f, 0.175f);
            float offsetZ = Random.Range(-sectionLength / 2.5f, sectionLength / 2.5f);
            speedBoost.transform.position = section.transform.position + new Vector3(offsetX, 0.25f, offsetZ);
            
            Debug.Log("Speed Boost Spawned!");
        }
    }


    IEnumerator IncreaseSpawnRate()
    {
        while (obsSpawnRate > minSpawnRate)
        {
            yield return new WaitForSeconds(10f); // Every 10 seconds, decrease spawn rate
            obsSpawnRate = Mathf.Max(minSpawnRate, obsSpawnRate - spawnRateDecrease);
        }
    }
}
