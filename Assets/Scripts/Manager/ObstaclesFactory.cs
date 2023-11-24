using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ObstaclesFactory : MonoBehaviour
{
    [SerializeField] private List<GameObject> obstacles = new List<GameObject>();

    [Header("Spawn Settings")]
    [SerializeField] private int maxObstaclesCount = 10;
    [SerializeField] private bool isGenerationEnabled = true;
    [Range(0.5f, 6f)]
    [SerializeField] private float spawnFrequency = 2f;
    [SerializeField] private float spawnDistanceToPlayer = 10f;
    
    [Header("Dependencies")]
    [SerializeField] private Transform playerTransform;
    

    List<GameObject> instantiatedObstacles = new List<GameObject>();
    float randomRange = 0f;
    float timeSinceLastSpawn = 0f;


    private void Update() {
        if (GameManager.instance.isRunning){
            timeSinceLastSpawn += Time.deltaTime;

            if (timeSinceLastSpawn >= spawnFrequency){
                randomRange = Random.Range(0, spawnDistanceToPlayer/2);
                createObstacle();
                timeSinceLastSpawn = 0f;
            }

            destroyOldObstacles();
        }
    }

    void createObstacle(){
        GameObject obj = obstacles[Mathf.RoundToInt(Random.Range(0, obstacles.Count))];
        Vector3 pos = new Vector3(  playerTransform.position.x + (spawnDistanceToPlayer + randomRange), 
                                    ObjectsManager.groundHeight, 
                                    0f);
        GameObject instantiated = Instantiate(obj, pos, Quaternion.identity);
        instantiatedObstacles.Add(instantiated);
        
    }

    void destroyOldObstacles(){
        if (instantiatedObstacles.Count > maxObstaclesCount){
            GameObject obstacleToRemove = instantiatedObstacles[0];
            instantiatedObstacles.RemoveAt(0);
            Destroy(obstacleToRemove);

        }
    }
}