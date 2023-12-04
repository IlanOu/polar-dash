using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public int score = 0;
    public int bestScore = 0;
    public bool scoreIsRunning = false;
    public TextMeshProUGUI textScore;

    private string defaultText = "Score : ";
    private bool scoreUpdateInProgress = false;
    private Coroutine scoreCoroutine;
    private int scoreForNextLevel = 10; // Définissez le nombre de points nécessaires pour passer au niveau suivant

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
        if (!scoreUpdateInProgress && scoreIsRunning && GameManager.instance.isRunning && LevelManager.instance.currentState == LevelManager.GameState.InLevel)
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
        else if (!scoreIsRunning || !GameManager.instance.isRunning || LevelManager.instance.currentState == LevelManager.GameState.BetweenLevels)
        {
            // Si le jeu n'est plus en cours ou s'il est entre les niveaux, arrêter la coroutine existante
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

        if (LevelManager.instance.currentState == LevelManager.GameState.InLevel) // Vérifier que le jeu est dans l'état "InLevel"
        {
            score++;
            UpdateScoreText();

            // Vérifier si le score atteint le seuil pour passer au niveau suivant
            if (score % scoreForNextLevel == 0)
            {
                LevelManager.instance.UpdateInLevel(); // Appeler la méthode d'incrémentation du niveau dans le LevelManager
            }
        }

        // La coroutine s'arrête ici si le jeu n'est plus en cours ou s'il est entre les niveaux
        scoreUpdateInProgress = false;
    }

    void UpdateScoreText()
    {
        textScore.text = defaultText + score.ToString();
    }

    public void UpdateBestScore()
    {
        bestScore = Math.Max(score, bestScore);
    }
}
