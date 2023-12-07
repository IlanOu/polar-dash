using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageLoader : MonoBehaviour
{
    public Image photo;
    private string pathBegin = "StreamingAssets/photos/";

    void Start()
    {
        LoadImageFromPath(DataTreat.instance.imagePath);
    }

    public void LoadImageFromPath(string path)
    {
        if(path != null)
        {
            path = pathBegin + path;
            // Vérifier si le fichier existe
            if (System.IO.File.Exists(path))
            {
                // Charger l'image en tant que tableau d'octets
                byte[] fileData = System.IO.File.ReadAllBytes(path);

                // Créer une texture2D
                Texture2D texture = new Texture2D(2, 2);

                // Charger l'image à partir des données d'octets
                texture.LoadImage(fileData);

                // Créer un sprite à partir de la texture
                Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));

                // Mettre à jour le composant Image avec le sprite
                photo.sprite = sprite;
                photo.enabled = true;
                DataTreat.instance.imagePath = "";
            }
            else
            {
                Debug.Log("Le fichier d'image n'existe pas : " + path);
            }
        }
        
    }
}
