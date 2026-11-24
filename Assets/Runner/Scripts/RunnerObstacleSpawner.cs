using UnityEngine;

public class RunnerObstacleSpawner : MonoBehaviour
{
    [Header("Obstacle Settings")]
    [SerializeField] private GameObject obstaclePrefab;
    [SerializeField] private float spawnX = 12f;
    [SerializeField] private float groundY = -1.15f;

    [Header("Spawn Timing")]
    [SerializeField] private float minSpawnDelay = 1.5f;
    [SerializeField] private float maxSpawnDelay = 3f;

    [Header("Group Settings")]
    [SerializeField] private int minObstacles = 1;
    [SerializeField] private int maxObstacles = 3;
    [SerializeField] private float gapBetweenObstacles = 0.8f;

    private float spawnTimer;
    private bool isSpawning;

    private void Start()
    {
        isSpawning = false;
        spawnTimer = 1f;
    }

    private void Update()
    {
        if (!isSpawning)
            return;

        spawnTimer -= Time.deltaTime;

        if (spawnTimer <= 0f)
        {
            SpawnObstacleGroup();
            SetNextSpawnTime();
        }
    }

    private void SpawnObstacleGroup()
    {
        if (obstaclePrefab == null)
        {
            Debug.LogError("Assign the obstacle prefab in the Inspector!");
            return;
        }

        int count = Random.Range(minObstacles, maxObstacles + 1);

        for (int i = 0; i < count; i++)
        {
            Vector3 position = new Vector3(
                spawnX + i * gapBetweenObstacles,
                groundY,
                0f
            );

            Instantiate(obstaclePrefab, position, Quaternion.identity);
        }
    }

    private void SetNextSpawnTime()
    {
        spawnTimer = Random.Range(minSpawnDelay, maxSpawnDelay);
    }

    public void StartSpawning()
    {
        isSpawning = true;
        SetNextSpawnTime();
    }

    public void StopSpawning()
    {
        isSpawning = false;
    }
}