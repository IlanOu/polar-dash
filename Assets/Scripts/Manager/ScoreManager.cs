using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
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
        if(instance != null)
        {
            Debug.Log("Il existe déjà une instance de ScoreManager dans cette scène");
            return;
        }
        instance = this;
    }

    void Start()
    {
        foreach(string movement in LevelManager.instance.movementList)
        {
            Debug.Log(movement);
            DataStorage.instance.leftPlayer.Add(movement, 0);
            DataStorage.instance.rightPlayer.Add(movement, 0);
        }
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

        DataStorage.instance.score++;
        UpdateScoreText();

        if (DataStorage.instance.score % ScoreForNextLevel == 0)
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
        textScore.text = DefaultText + DataStorage.instance.score.ToString();
    }

    public void UpdateDictionnary(string side, string movement)
    {
        if(side == "left")
        {
            DataStorage.instance.leftPlayer[movement]++;
            if(movement == "squat" || movement == "bottom")
            {
                DataStorage.instance.leftPlayer["jump"]--;
            }
        }
        else if(side == "right")
        {
            DataStorage.instance.rightPlayer[movement]++;
            if(movement == "squat" || movement == "bottom")
            {
                DataStorage.instance.rightPlayer["jump"]--;
            }
        }
    }
}
