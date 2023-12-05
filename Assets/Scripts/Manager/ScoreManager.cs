using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public int score = 0;
    public int bestScore = 0;
    public int stepScore = 0;
    public TextMeshProUGUI textScore;

    private const string DefaultText = "Score : ";
    public const int ScoreForNextLevel = 10;
    public const int TimeBetweenLevel = 5;

    private Coroutine scoreCoroutine;
    private Coroutine stepScoreCoroutine;

    public static ScoreManager instance;

    void Awake()
    {
        instance = this;
    }

    void Update()
    {
        UpdateCoroutines();
    }

    void UpdateCoroutines()
    {
        if (GameManager.instance.isRunning)
        {
            StartScoreCoroutine();
            StartStepScoreCoroutine();
        }
        else
        {
            StopScoreCoroutine();
            StopStepScoreCoroutine();
        }
    }

    void StartScoreCoroutine()
    {
        if (LevelManager.instance.currentState == LevelManager.GameState.InLevel && scoreCoroutine == null)
        {
            scoreCoroutine = StartCoroutine(IncrementScore());
        }
    }

    void StopScoreCoroutine()
    {
        if (scoreCoroutine != null)
        {
            StopCoroutine(scoreCoroutine);
            scoreCoroutine = null;
        }
    }

    void StartStepScoreCoroutine()
    {
        if (stepScoreCoroutine == null && GameManager.instance.isRunning)
        {
            stepScoreCoroutine = StartCoroutine(IncrementStepScore());
        }
    }

    void StopStepScoreCoroutine()
    {
        if (stepScoreCoroutine != null)
        {
            StopCoroutine(stepScoreCoroutine);
            stepScoreCoroutine = null;
        }
    }

    IEnumerator IncrementScore()
    {
        yield return new WaitForSeconds(1f);

        score++;
        UpdateScoreText();

        if (score % ScoreForNextLevel == 0)
        {
            LevelManager.instance.UpdateInLevel();
        }

        scoreCoroutine = null;
    }

    IEnumerator IncrementStepScore()
    {
        yield return new WaitForSeconds(1f);

        stepScore++;
        // Ajoutez d'autres actions ici si nécessaire

        stepScoreCoroutine = null;
    }

    void UpdateScoreText()
    {
        textScore.text = DefaultText + score.ToString();
    }

    public void UpdateBestScore()
    {
        bestScore = Math.Max(score, bestScore);
    }
}
