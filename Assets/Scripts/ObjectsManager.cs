using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public static class ObjectsManager
{
    public static int groundHeight = 0;

    public static Vector2 spriteSize(GameObject spriteObject){
        SpriteRenderer spriteRenderer = spriteObject.GetComponent<SpriteRenderer>();

        if (spriteRenderer == null){
            Debug.LogError("Il n'y a pas de spriteRenderer sur l'objet :", spriteObject);
            return Vector2.zero;
        }

        return new Vector2(spriteRenderer.bounds.size.x, spriteRenderer.bounds.size.y);
    }
}