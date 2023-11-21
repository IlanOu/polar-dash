using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class LevelManager : MonoBehaviour
{
    public int scoreBeforeChangeLevel;
    public int currentLevel = 1;
    private int nextScoreBeforeChangeLevel;

    public static LevelManager instance;
    void Awake()
    {
        if (instance)
        {
            Debug.Log("IL existe déjà une instance de LevelManager dans cette scène");
            return;
        }
        instance = this;
    }

    void Start()
    {
        nextScoreBeforeChangeLevel = scoreBeforeChangeLevel;
    }
    
    void Update()
    {
        if(ScoreManager.instance.score >= nextScoreBeforeChangeLevel)
        {
            nextScoreBeforeChangeLevel += scoreBeforeChangeLevel;
            currentLevel++;
        }
    }
}
