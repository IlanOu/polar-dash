using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generation : MonoBehaviour
{
    
    public List<GameObject> Maps = new List<GameObject>();
    private List<GameObject> InstantiatedMapsList = new List<GameObject>();

    public string layerName = "Ground";
    


    
    void Start()
    {
        float offsetX = 0f;
        float offsetY = 0f;

        foreach (var mapPrefab in Maps)
        {
            GameObject instantiatedMap = Instantiate(mapPrefab, new Vector3(offsetX, offsetY, 0), Quaternion.identity);
            PositionMap(instantiatedMap, offsetX, offsetY);
            offsetX += GetMapWidth(instantiatedMap);
            Debug.Log(GetMapWidth(instantiatedMap));

            InstantiatedMapsList.Add(instantiatedMap);
        }
    }



    void PositionMap(GameObject currentMap, float offsetX, float offsetY)
    {

        if (offsetX == 0f)
        {
            currentMap.transform.position = new Vector3(offsetX, offsetY, 0);
        }
        else
        {
            GameObject previousMap = GetPreviousMap();
            

            // Logique de positionnement ajustée
            Transform leftObject = GetExtremeChild(currentMap, false); // false = Left object
            Transform rightObjectPreviousMap = GetExtremeChild(previousMap, true); // true = Right object

            if (leftObject != null && rightObjectPreviousMap != null){
                float topmostPointLeft = GetTopmostPoint(leftObject);
                float topmostPointRightPreviousMap = GetTopmostPoint(rightObjectPreviousMap);

                float heightAdjustment = topmostPointRightPreviousMap - topmostPointLeft;


                currentMap.transform.position = new Vector3(currentMap.transform.position.x, currentMap.transform.position.y + heightAdjustment, 0);
            }else{
                Debug.LogError("Aucun objet nommé "+layerName+" dans la map.");
            }
        }
    }




    GameObject GetPreviousMap()
    {
        int index = InstantiatedMapsList.Count - 1;
        return index >= 0 ? InstantiatedMapsList[index] : null;
    }


    float GetMapWidth(GameObject map)
    {
        Transform[] children = map.GetComponentsInChildren<Transform>(false);

        float leftmostEdge = float.MaxValue;
        float rightmostEdge = float.MinValue;

        foreach (Transform child in children)
        {
            Renderer childRenderer = child.GetComponent<Renderer>();

            if (childRenderer != null)
            {
                float childLeftEdge = childRenderer.bounds.min.x;
                float childRightEdge = childRenderer.bounds.max.x;

                if (childLeftEdge < leftmostEdge)
                {
                    leftmostEdge = childLeftEdge;
                }

                if (childRightEdge > rightmostEdge)
                {
                    rightmostEdge = childRightEdge;
                }
            }
        }

        // Vérifier si des objets ont été trouvés pour éviter une largeur négative si la map est vide
        if (leftmostEdge <= rightmostEdge)
        {
            return rightmostEdge - leftmostEdge;
        }
        else
        {
            // Aucun objet trouvé, la largeur est zéro ou négative
            return 0f;
        }
    }




    float GetTopmostPoint(Transform obj)
    {
        float topmostPoint = float.MinValue;
        Renderer renderer = obj.GetComponent<Renderer>();

        if (renderer != null)
        {
            topmostPoint = renderer.bounds.max.y;
        }

        return topmostPoint;
    }




    Transform GetExtremeChild(GameObject obj, bool rightmost)
    {
        Transform[] children = obj.GetComponentsInChildren<Transform>(false);

        if (children.Length == 0)
        {
            return null; // Aucun enfant trouvé, retourne null
        }

        float extremeBoundX = rightmost ? float.MinValue : float.MaxValue;
        Transform extremeChild = null;

        foreach (Transform child in children)
        {
            if (child.gameObject.layer == LayerMask.NameToLayer(layerName)){

                Renderer childRenderer = child.GetComponent<Renderer>();

                if (childRenderer != null)
                {
                    float childBoundX = childRenderer.bounds.min.x;

                    if ((rightmost && childBoundX > extremeBoundX) || (!rightmost && childBoundX < extremeBoundX))
                    {
                        extremeBoundX = childBoundX;
                        extremeChild = child;
                    }
                }
            }
        }

        return extremeChild;
    }
}
