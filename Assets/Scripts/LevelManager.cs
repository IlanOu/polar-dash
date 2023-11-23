using System.Collections;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;


public class LevelManager : MonoBehaviour
{
    public Text textLevel;
    public Text textIndicator;
    public Text textTiming;
    public Text textNewMovement;
    private string defaultTextLevel = "Niveau ";
    private string firstDefaultTextTiming = "DANS ";
    private string secondDefaultTextTiming = "...";
    public ChangeLevelBar changeLevelBar;
    public int currentLevel = 1;
    public int deltaScoreBeforeChangeLevel;
    private int nextScoreBeforeChangeLevel;

    public ActionUX leftActionUX;
    public ActionUX rightActionUX;

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

        changeLevelBar.SetNewValues(ScoreManager.instance.score, nextScoreBeforeChangeLevel);

        ChangeActionUX();
        RefreshTextLevel();
    }
    
    void Update()
    {
        changeLevelBar.SetValue(ScoreManager.instance.score);
        int numberBeforeChangeLevel = nextScoreBeforeChangeLevel - ScoreManager.instance.score;
        if (numberBeforeChangeLevel <= 3)
        {
            PrintTextIndicator(true, numberBeforeChangeLevel);
        }
        // Si on doit changer de level
        if(ScoreManager.instance.score >= nextScoreBeforeChangeLevel)
        {
            nextScoreBeforeChangeLevel += deltaScoreBeforeChangeLevel;
            currentLevel++;
            RefreshTextLevel();

            changeLevelBar.SetNewValues(ScoreManager.instance.score, nextScoreBeforeChangeLevel);

            ChangeMovement();
        }
    }

    void ChangeMovement()
    {
        leftMovement = movementList[Random.Range(0, movementList.Length)];
        rightMovement = movementList[Random.Range(0, movementList.Length)];
        ChangeActionUX();
        StartCoroutine(PrintTextNewMovement());
    }

    void RefreshTextLevel()
    {
        textLevel.text = defaultTextLevel + currentLevel.ToString();
    }

    void ChangeActionUX()
    {
        leftActionUX.PrintImage(leftMovement);
        rightActionUX.PrintImage(rightMovement);
    }
    
    void PrintTextIndicator(bool enabled, int number = 0)
    {
        textIndicator.enabled = enabled;
        textTiming.text = firstDefaultTextTiming + number + secondDefaultTextTiming;
        textTiming.enabled = enabled;
    }

    IEnumerator PrintTextNewMovement()
    {
        PrintTextIndicator(false);
        textNewMovement.enabled = true;
        yield return new WaitForSeconds(2f);
        textNewMovement.enabled = false;
    }
}
