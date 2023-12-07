using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartingGameAnim : MonoBehaviour
{
    [Header("Animation -> game")]
    public Image whiteBackground;
    public Image floconsBackground;
    public float delayInSeconds = 3f; // Temps en secondes avant que l'image ne devienne opaque
    public float startOpacity = 0.6f; // Opacité initiale
    private float elapsedTime = 0f; // Temps écoulé depuis le début du jeu

    public bool inStartScene = false;

    [Header("Animation -> menu")]
    public GameObject parentBackground;
    public float speed, time, delay;

    public static StartingGameAnim instance;
    void Awake()
    {
        if(instance != null)
        {
            Debug.Log("Il existe déjà une instance de startGameAnim dans cette scène");
            return;
        }
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        // Ajuste l'alpha initial de l'image
        Debug.Log(whiteBackground);
        whiteBackground.color = new Color(whiteBackground.color.r, whiteBackground.color.g, whiteBackground.color.b, startOpacity);
        floconsBackground.color = new Color(floconsBackground.color.r, floconsBackground.color.g, floconsBackground.color.b, startOpacity);
    }

    // Update is called once per frame
    void Update()
    {
        if (!inStartScene)
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
                floconsBackground.color = new Color(floconsBackground.color.r, floconsBackground.color.g, floconsBackground.color.b, alpha);
            }
            else
            {
                // Désactive la mise à jour une fois le délai atteint
                enabled = false;
            }
        }else{
            whiteBackground.color = new Color(whiteBackground.color.r, whiteBackground.color.g, whiteBackground.color.b, 1.0f);
        floconsBackground.color = new Color(floconsBackground.color.r, floconsBackground.color.g, floconsBackground.color.b, 1.0f);


        }
    }

    public float StartTransitionToGameScene()
    {
        float x = 0f, y = 0f;
        Vector2 targetPosition = new Vector2(x, y);
        StartCoroutine(ActiveTransitionGame(targetPosition));
        return time + delay;
        
    }

    IEnumerator ActiveTransitionGame(Vector2 targetPosition)
    {
        yield return new WaitForSeconds(delay);
        float t = 0f;

        RectTransform rectTransform = parentBackground.GetComponent<RectTransform>();
        Vector2 startPosition = rectTransform.anchoredPosition3D;

        while (t < time)
        {
            t += Time.deltaTime * speed;

            rectTransform.anchoredPosition3D = Vector3.Lerp(startPosition, targetPosition, t);
            yield return null;
        }
    }
}
