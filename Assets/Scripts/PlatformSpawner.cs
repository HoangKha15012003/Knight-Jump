using UnityEngine;

public class PlatformSpawner : MonoBehaviour
{
    public GameObject platformPrefab;
    public float spawnRate = 1.5f;
    public float minX = -3f, maxX = 3f;
    public float spawnY = 6f;

    void Start()
    {
        InvokeRepeating("SpawnPlatform", 1f, spawnRate);
    }

    void SpawnPlatform()
    {
        float randomX = Random.Range(minX, maxX);
        Vector3 spawnPos = new Vector3(randomX, spawnY, 0);
        Instantiate(platformPrefab, spawnPos, Quaternion.identity);
    }
}
