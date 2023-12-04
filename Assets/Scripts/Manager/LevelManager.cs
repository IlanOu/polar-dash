using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;


public class LevelManager : MonoBehaviour
{
    private string firstDefaultTextIndicator = "Changement de mouvement dans ";
    private string secondDefaultTextIndicator = "...";
    [HideInInspector] public GameObject GO_parentChangeLevelBar;
    public int currentLevel = 1;
    public int deltaScoreBeforeChangeLevel;
    private int nextScoreBeforeChangeLevel;
    private int currentStepLevel = 0;

    [Header("Interfaces")]
    public TextMeshProUGUI textLevel;
    public TextMeshProUGUI textIndicator;
    public TextMeshProUGUI textNewMovement;
    private string defaultTextLevel = "Niveau ";
    public ActionUX leftActionUX;
    public ActionUX rightActionUX;
    public ChangeLevelBar changeLevelBar;

    [Header("Mouvements")]
    [HideInInspector] public string leftMovement;
    [HideInInspector] public string rightMovement;

    [HideInInspector] public string leftAction;
    [HideInInspector] public string rightAction;

    public string[] movementList;
    public string[] actionList;
    
    public static LevelManager instance;

    [Header("Génération")]
    public ObstaclesFactory obstaclesFactory;
    void Awake()
    {
        GO_parentChangeLevelBar = changeLevelBar.transform.parent.gameObject;

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
            if (currentStepLevel % 2 == 0){
                currentLevel++;
                obstaclesFactory.isGenerationEnabled = !obstaclesFactory.isGenerationEnabled;

                RefreshTextLevel();
                changeLevelBar.SetNewValues(ScoreManager.instance.score, nextScoreBeforeChangeLevel);
                ChangeMovement();
            }
            nextScoreBeforeChangeLevel += deltaScoreBeforeChangeLevel;
            currentStepLevel++;
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
        GO_parentChangeLevelBar.SetActive(false);
        textIndicator.enabled = enabled;
        textIndicator.text = firstDefaultTextIndicator + number.ToString() + secondDefaultTextIndicator;
    }

    IEnumerator PrintTextNewMovement()
    {
        PrintTextIndicator(false);
        textNewMovement.enabled = true;
        yield return new WaitForSeconds(2f);
        textNewMovement.enabled = false;
        GO_parentChangeLevelBar.SetActive(true);
    }
}
