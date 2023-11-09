using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generation : MonoBehaviour
{
    
    public List<GameObject> Maps = new List<GameObject>();


    // Start is called before the first frame update
    void Start()
    {
        int i = 0;
        foreach (var Map in Maps)
        {
            
            // Debug.Log(Map);
            Instantiate(Map, new Vector3(i * 20, -10, 0), Quaternion.identity);
            Map.transform.position = Vector3.zero;

            i += 1;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
