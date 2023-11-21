using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;


public class LevelManager : MonoBehaviour
{
    public Text textAction;
    public int currentLevel = 1;
    public int deltaScoreBeforeChangeLevel;
    private int nextScoreBeforeChangeLevel;

    public int deltaLevelBeforeChangeAction;
    private int nextLevelBeforeChangeAction;

    public string leftMovement;
    public string rightMovement;

    public string leftAction;
    public string rightAction;

    public string[] movementList;
    public string[] actionList;
    
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
        nextScoreBeforeChangeLevel = deltaScoreBeforeChangeLevel;

        nextLevelBeforeChangeAction = deltaLevelBeforeChangeAction;
        leftAction = actionList[0];
        rightAction = actionList[1];

        ChangeMovement();
    }
    
    void Update()
    {
        if(ScoreManager.instance.score >= nextScoreBeforeChangeLevel)
        {
            nextScoreBeforeChangeLevel += deltaScoreBeforeChangeLevel;
            currentLevel++;
            ChangeMovement();
        }
        if(currentLevel >= nextLevelBeforeChangeAction)
        {
            nextLevelBeforeChangeAction += deltaLevelBeforeChangeAction;
            ChangeRole();
        }
    }

    void ChangeMovement()
    {
        leftMovement = movementList[Random.Range(0, movementList.Length)];
        rightMovement = movementList[Random.Range(0, movementList.Length)];
        RefreshTextAction();
    }

    void ChangeRole()
    {
        string tempAction = leftAction;
        leftAction = rightAction;
        rightAction = tempAction;
        RefreshTextAction();
    }

    void RefreshTextAction()
    {
        if (leftAction == "jump")
        {
            textAction.text = leftMovement + " LEFT TO JUMP";
        }
        if (rightAction == "jump")
        {
            textAction.text = rightMovement + " RIGHT TO JUMP";
        }
    }
}
