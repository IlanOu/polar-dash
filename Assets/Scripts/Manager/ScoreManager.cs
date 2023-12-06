using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public int scorePerDistance;
    [HideInInspector] public int stepTime = 0;
    public TextMeshProUGUI textScore;
    private const string DefaultText = " PTS";
    public const int TimeForNextLevel = 10000;
    public const int TimeBetweenLevel = 5;

    private Coroutine timeCoroutine;
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
            DataStorage.instance.leftPlayer.Add(movement, 0);
            DataStorage.instance.rightPlayer.Add(movement, 0);
        }
    }

    void Update()
    {
        UpdateCoroutines();
        UpdateScore();
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
        if (LevelManager.instance.currentState == LevelManager.GameState.InLevel && timeCoroutine == null)
        {
            timeCoroutine = StartCoroutine(IncrementTime());
        }
    }

    void StopScoreCoroutine()
    {
        if (timeCoroutine != null)
        {
            StopCoroutine(timeCoroutine);
            timeCoroutine = null;
        }
    }

    void StartStepScoreCoroutine()
    {
        if (stepScoreCoroutine == null && GameManager.instance.isRunning)
        {
            stepScoreCoroutine = StartCoroutine(IncrementStepTime());
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

    IEnumerator IncrementTime()
    {
        yield return new WaitForSeconds(1f / 10);
        DataStorage.instance.deciSeconds++;
        DataStorage.instance.time = (int)(DataStorage.instance.deciSeconds / 10);

        if (DataStorage.instance.time % TimeForNextLevel == 0)
        {
            LevelManager.instance.UpdateInLevel();
        }

        timeCoroutine = null;
    }

    IEnumerator IncrementStepTime()
    {
        yield return new WaitForSeconds(1f);

        stepTime++;
        stepScoreCoroutine = null;
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

    private void UpdateScore()
    {
        int distance = Mathf.FloorToInt(((DataStorage.instance.speedPenguin / 10) * DataStorage.instance.deciSeconds));
        DataStorage.instance.distance = distance;
        DataStorage.instance.score = distance * scorePerDistance;
        UpdateScoreText();
    }

    void UpdateScoreText()
    {
        textScore.text = DataStorage.instance.score.ToString() + DefaultText;
    }
}
