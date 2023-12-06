using System;
using System.Collections;
using System.Collections.Generic;
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
    public static DataStorage instance;
    void Awake()
    {
        leftPlayer = new Dictionary<string, int>();
        rightPlayer = new Dictionary<string, int>();
        if(instance != null)
        {
            Debug.Log("Il existe déjà une instance de DataStorage dans cette scène");
            return;
        }
        instance = this;
    }

    public void UpdateBestScore()
    {
        bestScore = Math.Max(score, bestScore);
    }
}
