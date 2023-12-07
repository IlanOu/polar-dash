using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DontDestroyOnLoadScene : MonoBehaviour
{
    public List<GameObject> objects;

    public static DontDestroyOnLoadScene instance;
    void Awake()
    {
        foreach (GameObject element in objects)
        {
            DontDestroyOnLoad(element);
        }

        if(instance != null)
        {
            Debug.Log("Il existe déjà une instance de DontDestroyOnLoadScene dans cette scène");
            return;
        }

        instance = this;
    }

    public void addObjectToDontDestroyOnLoad(GameObject newObject)
    {
        if (!objects.Contains(newObject))
        {
            objects.Add(newObject);

            DontDestroyOnLoad(newObject);
        }
        else
        {
            Debug.LogWarning("L'objet est déjà dans la liste.");
        }
    }

}
