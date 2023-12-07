using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DataStorage : MonoBehaviour
{
    public float speedPenguin;
    public int score;
    public int bestScore;
    public int time;
    public int deciSeconds;
    public int distance;
    public int dodgedObstacle;
    public int burnedCalories;
    public Dictionary<string, int> leftPlayer;
    public Dictionary<string, int> rightPlayer;
    private float jumpCalories = 7.5f;
    private float squatCalories = 10f;
    
    public static DataStorage instance;
    void Awake()
    {
        if(instance != null)
        {
            Debug.Log("Il existe déjà une instance de DataStorage dans cette scène");
            Destroy(this.gameObject);
            return;
        }
        else
        {
            leftPlayer = new Dictionary<string, int>();
            rightPlayer = new Dictionary<string, int>();
            instance = this;
        }
    }

    public void UpdateBestScore()
    {
        bestScore = Math.Max(score, bestScore);
    }

    public void CalculCalories()
    {
        int jump = (int)((leftPlayer["jump"] + rightPlayer["jump"]) * jumpCalories);
        int squat = (int)((leftPlayer["squat"] + rightPlayer["squat"]) * squatCalories);
        burnedCalories = jump + squat;
    }

    public void ResetData()
    {
        score = 0;
        time = 0;
        deciSeconds = 0;
        distance = 0;
        dodgedObstacle = 0;
        burnedCalories = 0;
        leftPlayer.Clear();
        rightPlayer.Clear();
    }
}
