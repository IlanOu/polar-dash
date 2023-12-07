using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ObstaclesFactory : MonoBehaviour
{
    [SerializeField] private List<GameObject> obstacles = new List<GameObject>();

    [Header("Spawn Settings")]
    [SerializeField] private int maxObstaclesCount = 10;
    public bool isGenerationEnabled = true;
    [Range(0.5f, 6f)] [SerializeField] private float spawnFrequency = 2f;
    [SerializeField] private float spawnDistanceToPlayer = 10f;
    
    [Header("Dependencies")]
    [SerializeField] private Transform playerTransform;
    
    private List<GameObject> instantiatedObstacles = new List<GameObject>();
    // private float randomRange = 0f;
    private float timeSinceLastSpawn = 0f;

    private void Update()
    {
        if (GameManager.instance.isRunning && isGenerationEnabled)
        {
            timeSinceLastSpawn += Time.deltaTime;

            if (timeSinceLastSpawn >= spawnFrequency)
            {
                GenerateObstacle();
                timeSinceLastSpawn = 0f;
            }

            DestroyOldObstacles();
        }
    }

    private void GenerateObstacle()
    {
        GameObject selectedObstacle = GetRandomObstacle();
        Vector3 spawnPosition = CalculateSpawnPosition();
        GameObject instantiatedObstacle = Instantiate(selectedObstacle, spawnPosition, Quaternion.identity);
        instantiatedObstacles.Add(instantiatedObstacle);
    }

    private GameObject GetRandomObstacle()
    {
        return obstacles.Count > 0 ? obstacles[Random.Range(0, obstacles.Count)] : null;
    }

    private Vector3 CalculateSpawnPosition()
    {
        return new Vector3(playerTransform.position.x + (spawnDistanceToPlayer + Random.Range(0, spawnDistanceToPlayer / 2)),
            ObjectsManager.groundHeight, 0f);
    }

    private void DestroyOldObstacles()
    {
        if (instantiatedObstacles.Count > maxObstaclesCount)
        {
            GameObject obstacleToRemove = instantiatedObstacles[0];
            instantiatedObstacles.RemoveAt(0);
            Destroy(obstacleToRemove);
        }
    }
}
