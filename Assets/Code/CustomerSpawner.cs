using UnityEngine;

public class CustomerSpawner : MonoBehaviour
{
    public Transform entranceLocation; // The entrance location where customers will be spawned
    public GameObject[] customerSkins; // Array of customer skins
    public float minSpawnDelay = 4f; // Minimum delay between spawns
    public float maxSpawnDelay = 9f; // Maximum delay between spawns

    private float spawnTimer;

    private int lastCustomer = 0;

    private void Start()
    {
        Time.timeScale = 5f;
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
        lastCustomer = getCustomerSkin();
        Instantiate(customerSkins[lastCustomer], entranceLocation.position, Quaternion.identity);


        // If you have other initialization logic for the customer, add it here
    }

    private int getCustomerSkin()
    {
        int customer = Random.Range(0, customerSkins.Length);
        if (customer != lastCustomer) {
            return customer;
        }
        else { 
            return getCustomerSkin();
        }
    }
}
