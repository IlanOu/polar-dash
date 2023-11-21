    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class ObstaclesFactory : MonoBehaviour
    {
        public List<GameObject> Obstacles = new List<GameObject>();
        public int obstaclePoolSize = 5; // Nombre initial d'obstacles à créer
        public float despawnDistance = 20f; // Distance à partir de laquelle les obstacles seront désactivés
        public Transform playerTransform; // Référence au transform du joueur
        public bool isGenerationEnabled = true; // Variable booléenne pour contrôler la génération

        [Range(0.5f, 6f)]
        public float spawnFrequency = 1f; // Fréquence d'apparition des obstacles (modifiable depuis l'éditeur Unity)
        
        
        private float startSpawFrequency = 1f;
        private List<GameObject> obstaclePool = new List<GameObject>();

        // Start is called before the first frame update
        void Start()
        {
            // Initialiser le pool d'obstacles
            InitializeObstaclePool();

            // Lancer la génération d'obstacles de manière répétée avec un délai initial
            InvokeRepeating("SpawnObstacle", 0f, spawnFrequency);
            startSpawFrequency = spawnFrequency;
        }

        // Initialiser le pool d'obstacles
        void InitializeObstaclePool()
        {
            for (int i = 0; i < obstaclePoolSize; i++)
            {
                GameObject obstacle = Instantiate(Obstacles[Random.Range(0, Obstacles.Count)], Vector3.zero, Quaternion.identity);
                obstacle.SetActive(false);
                obstaclePool.Add(obstacle);
            }
        }

        // Méthode pour générer un obstacle
        void SpawnObstacle()
        {
            // Vérifier si la génération est activée
            if (isGenerationEnabled)
            {
                // Trouver un obstacle inactif dans le pool
                GameObject obstacle = GetInactiveObstacle();

                if (obstacle != null)
                {
                    // Activer l'obstacle
                    obstacle.SetActive(true);

                    // Définir la position de spawn avec une hauteur égale à 0.5 fois la hauteur de l'obstacle
                    Vector3 spawnPosition = new Vector3(playerTransform.position.x + 10f, obstacle.transform.localScale.y * 0.5f, 0f);
                    obstacle.transform.position = spawnPosition;
                }
                else
                {
                    Debug.LogWarning("Tous les obstacles sont actifs. Considère d'augmenter la taille du pool d'obstacles.");
                }
            }
        }

        // Update is called once per frame
        void Update()
        {
            // Désactiver les obstacles qui sont trop éloignés du joueur
            DeactivateObstacles();

            if (startSpawFrequency != spawnFrequency){
                CancelInvoke("SpawnObstacle");
                InvokeRepeating("SpawnObstacle", 0f, spawnFrequency);
                startSpawFrequency = spawnFrequency;
            }
        }

        // Désactiver les obstacles si la distance entre le joueur et l'obstacle est supérieure à la distance de despawn
        void DeactivateObstacles()
        {
            foreach (var obstacle in obstaclePool)
            {
                if (obstacle.activeSelf && Vector3.Distance(obstacle.transform.position, playerTransform.position) > despawnDistance)
                {
                    obstacle.SetActive(false);
                }
            }
        }

        // Trouver un obstacle inactif dans le pool
        GameObject GetInactiveObstacle()
        {
            foreach (var obstacle in obstaclePool)
            {
                if (!obstacle.activeSelf)
                {
                    return obstacle;
                }
            }
            return null;
        }
    }
