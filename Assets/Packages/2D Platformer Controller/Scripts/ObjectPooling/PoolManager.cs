using System.Collections.Generic;
using UnityEngine;

namespace SupanthaPaul
{
    public class PoolManager : MonoBehaviour {

        public static PoolManager instance;
        private void Awake()
        {
            instance = this;
        }

        Dictionary<int, Queue<ObjectInstance>> poolDictionary = new Dictionary<int, Queue<ObjectInstance>>();

        public void CreatePool(GameObject prefab, int poolSize, Transform parent= null)
        {
            int poolKey = prefab.GetInstanceID();

            if(!poolDictionary.ContainsKey(poolKey))
            {
                poolDictionary.Add(poolKey, new Queue<ObjectInstance>());
            }

            for (int i = 0; i < poolSize; i++)
            {
                ObjectInstance newObject = new ObjectInstance(Instantiate(prefab) as GameObject, parent);
                poolDictionary[poolKey].Enqueue(newObject);
            }
        }

        public GameObject ReuseObject(GameObject prefab, Vector3 position, Quaternion rotation)
        {
            int poolKey = prefab.GetInstanceID();
            if (poolDictionary.ContainsKey(poolKey))
            {
                ObjectInstance objectToReuse = poolDictionary[poolKey].Dequeue();
                poolDictionary[poolKey].Enqueue(objectToReuse);

                objectToReuse.Reuse(position, rotation);

                return objectToReuse.gameObject;
            }
            else
                return null;
        
        }

        public class ObjectInstance
        {
            public GameObject gameObject;

            bool hasPoolObjectScript;
            PoolObject poolObjectScript;

            public ObjectInstance(GameObject objectInstance, Transform parent= null)
            {
                gameObject = objectInstance;
                gameObject.SetActive(false);
                if (parent != null)
                    gameObject.transform.SetParent(parent, false);

                if(gameObject.GetComponent<PoolObject>())
                {
                    hasPoolObjectScript = true;
                    poolObjectScript = gameObject.GetComponent<PoolObject>();
                }
            }

            public void Reuse(Vector3 position, Quaternion rotation)
            {
                gameObject.SetActive(true);
                gameObject.transform.position = position;
                gameObject.transform.rotation = rotation;

                if (hasPoolObjectScript)
                {
                    poolObjectScript.OnObjectReuse();
                }
            }
        }
    }
}
