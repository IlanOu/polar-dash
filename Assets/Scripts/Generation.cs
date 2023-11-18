using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Generation : MonoBehaviour
{
    public List<GameObject> Maps = new List<GameObject>();
    private List<GameObject> InstantiatedMapsList = new List<GameObject>();
    public string layerName = "Ground";

    void Start()
    {
        float offsetX = 0;
        foreach (var mapPrefab in Maps)
        {
            float mapWidth = GetWidthInPixels(mapPrefab.transform);
            GameObject instantiatedMap = Instantiate(mapPrefab, new Vector3((mapWidth/2) + offsetX, 0, 0), Quaternion.identity);
            InstantiatedMapsList.Add(instantiatedMap);
            offsetX += mapWidth;
        }
    }


    /// <summary>
    /// Permet d'obtenir la largeur d'une map
    /// </summary>
    public float GetWidthInPixels(Transform parentTransform)
    {
        float leftmostPoint = float.MaxValue;
        float rightmostPoint = float.MinValue;

        foreach (Transform child in parentTransform)
        {
            // Ensure the child has a SpriteRenderer attached
            SpriteRenderer spriteRenderer = child.GetComponent<SpriteRenderer>();

            if (spriteRenderer != null)
            {
                // Calculate the leftmost and rightmost points
                float positionX = child.position.x;
                float halfWidth = spriteRenderer.bounds.size.x * 0.5f;

                float left = positionX - halfWidth;
                float right = positionX + halfWidth;

                // Update the leftmost and rightmost points
                leftmostPoint = Mathf.Min(leftmostPoint, left);
                rightmostPoint = Mathf.Max(rightmostPoint, right);
            }
            else
            {
                Debug.LogError("Some children do not have a SpriteRenderer.");
            }
        }

        // Calculate the total width in pixels
        float totalWidthInPixels = (rightmostPoint - leftmostPoint) * parentTransform.localScale.x;
        return totalWidthInPixels;
    }



    public float GetHeightInPixels(Transform parentTransform)
    {
        float bottommostPoint = float.MaxValue;
        float topmostPoint = float.MinValue;

        foreach (Transform child in parentTransform)
        {
            // Ensure the child has a SpriteRenderer attached
            SpriteRenderer spriteRenderer = child.GetComponent<SpriteRenderer>();

            if (spriteRenderer != null)
            {
                // Calculate the bottommost and topmost points
                float positionY = child.position.y;
                float halfHeight = spriteRenderer.bounds.size.y * 0.5f;

                float bottom = positionY - halfHeight;
                float top = positionY + halfHeight;

                // Update the bottommost and topmost points
                bottommostPoint = Mathf.Min(bottommostPoint, bottom);
                topmostPoint = Mathf.Max(topmostPoint, top);
            }
            else
            {
                Debug.LogError("Some children do not have a SpriteRenderer.");
            }
        }

        // Calculate the total height in pixels
        float totalHeightInPixels = (topmostPoint - bottommostPoint) * parentTransform.localScale.y;
        return totalHeightInPixels;
    }
}
