using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public int score = 0;
    public bool scoreIsRunning = false;
    public TextMeshProUGUI textScore;

    private string defaultText = "Score : ";
    private bool scoreUpdateInProgress = false;
    private Coroutine scoreCoroutine;

    public static ScoreManager instance;
    void Awake()
    {
        if (instance != null)
        {
            Debug.Log("IL existe déjà une instance de ScoreManager dans cette scène");
            return;
        }
        instance = this;
    }
    
    void Start()
    {
        scoreIsRunning = true;
        UpdateScoreText();
    }

    void Update()
{
    if (!scoreUpdateInProgress && scoreIsRunning && GameManager.instance.isRunning)
    {
        scoreUpdateInProgress = true;

        // Arrêter la coroutine existante si elle existe
        if (scoreCoroutine != null)
        {
            StopCoroutine(scoreCoroutine);
        }

        // Démarrer une nouvelle coroutine
        scoreCoroutine = StartCoroutine(AddScoreEverySecond());
    }
    else if (!scoreIsRunning || !GameManager.instance.isRunning)
    {
        // Si le jeu n'est plus en cours, arrêter la coroutine existante
        if (scoreCoroutine != null)
        {
            StopCoroutine(scoreCoroutine);
        }
        scoreCoroutine = null;
        scoreUpdateInProgress = false;
    }
}

IEnumerator AddScoreEverySecond()
{
    yield return new WaitForSeconds(1f);
    score++;
    UpdateScoreText();

    // La coroutine s'arrête ici si le jeu n'est plus en cours
    scoreUpdateInProgress = false;
}

    void UpdateScoreText()
    {
        textScore.text = defaultText + score.ToString();
    }

}
