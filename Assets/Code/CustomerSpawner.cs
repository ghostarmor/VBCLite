using UnityEngine;

public class CustomerSpawner : MonoBehaviour
{
    public Transform entranceLocation; // The entrance location where customers will be spawned
    public GameObject[] customerSkins; // Array of customer skins
    public float minSpawnDelay = 4f; // Minimum delay between spawns
    public float maxSpawnDelay = 9f; // Maximum delay between spawns

    private float spawnTimer;

    private void Start()
    {
        SetRandomSpawnTimer();
    }

    private void Update()
    {
        spawnTimer -= Time.deltaTime;

        if (spawnTimer <= 0)
        {
            SpawnCustomer();
            SetRandomSpawnTimer();
        }
    }

    private void SetRandomSpawnTimer()
    {
        spawnTimer = Random.Range(minSpawnDelay, maxSpawnDelay);
    }

    private void SpawnCustomer()
    {
        GameObject customer = Instantiate(customerSkins[Random.Range(0, customerSkins.Length)], entranceLocation.position, Quaternion.identity);


        // If you have other initialization logic for the customer, add it here
    }
}
