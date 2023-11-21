using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Generation : MonoBehaviour
{
    public List<GameObject> Maps = new List<GameObject>();
    private List<GameObject> InstantiatedMapsList = new List<GameObject>();
    public string groundLayerName = "Ground";

    public Transform player; // Assign the player's transform in the inspector
    public float chunkTriggerDistance = 10f; // Distance at which a new chunk should be generated
    public float chunkGroundTriggerDistance = 3f; // Distance at which a new chunk should be generated

    float offsetX = 0;
    int currentIndex = 0;

    public int maxMapNumber = 10;

    void Update()
    {
        // Calculate the distance between the player and the last instantiated chunk
        float distanceToLastGroundChunk = Mathf.Abs(player.position.x - (offsetX - GetWidthInPixels(Maps[currentIndex].transform, groundLayerName) / 2));

        // Generate a new chunk if the player is close enough to the last one
        if (distanceToLastGroundChunk < chunkGroundTriggerDistance)
        {
            GenerateMapGround();
        }
    }

    void GenerateMapGround()
    {
        var mapPrefab = Maps[currentIndex];
        float mapWidth = GetWidthInPixels(mapPrefab.transform, groundLayerName);
        GameObject instantiatedMap = Instantiate(mapPrefab, new Vector3((mapWidth / 2) + offsetX, 0, 0), Quaternion.identity);
        InstantiatedMapsList.Add(instantiatedMap);
        offsetX += mapWidth;

        // Pass to the next map, and loop back to the first if it reaches the end
        currentIndex = (currentIndex + 1) % Maps.Count;

        // Destroy the oldest chunk if the number of instantiated chunks exceeds a certain limit
        if (InstantiatedMapsList.Count > maxMapNumber)
        {
            Destroy(InstantiatedMapsList[0]);
            InstantiatedMapsList.RemoveAt(0);
        }
    }

    /// <summary>
    /// Permet d'obtenir la largeur d'une map
    /// </summary>
    public float GetWidthInPixels(Transform parentTransform, string layerName)
    {
        float leftmostPoint = float.MaxValue;
        float rightmostPoint = float.MinValue;

        foreach (Transform child in parentTransform)
        {
            if (child.gameObject.layer == LayerMask.NameToLayer(layerName))
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
            }else {
                Debug.LogError("Le nom du layer est incorect, aucun objet n'a été trouvé avec le nom : " + layerName);
            }
        }

        // Calculate the total width in pixels
        float totalWidthInPixels = (rightmostPoint - leftmostPoint) * parentTransform.localScale.x;
        return totalWidthInPixels;
    }

}
