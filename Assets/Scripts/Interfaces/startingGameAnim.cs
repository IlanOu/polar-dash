using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartingGameAnim : MonoBehaviour
{
    public Image whiteBackground;
    public float delayInSeconds = 3f; // Temps en secondes avant que l'image ne devienne opaque
    public float startOpacity = 0.6f; // Opacité initiale
    private float elapsedTime = 0f; // Temps écoulé depuis le début du jeu

    // Start is called before the first frame update
    void Start()
    {
        // Ajuste l'alpha initial de l'image
        whiteBackground.color = new Color(whiteBackground.color.r, whiteBackground.color.g, whiteBackground.color.b, startOpacity);
    }

    // Update is called once per frame
    void Update()
    {
        // Vérifie si le délai spécifié est atteint
        if (elapsedTime < delayInSeconds)
        {
            // Incrémente le temps écoulé uniquement tant que le délai n'est pas atteint
            elapsedTime += Time.deltaTime;

            // Calcule la progression de l'opacité en fonction du temps écoulé
            float alpha = 1 - Mathf.Clamp01(elapsedTime / delayInSeconds);

            // Ajuste l'alpha de l'image en fonction de la progression
            whiteBackground.color = new Color(whiteBackground.color.r, whiteBackground.color.g, whiteBackground.color.b, alpha);
        }
        else
        {
            // Désactive la mise à jour une fois le délai atteint
            enabled = false;
        }
    }
}
