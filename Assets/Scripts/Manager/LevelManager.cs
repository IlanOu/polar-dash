using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    public enum GameState
    {
        InLevel,
        BetweenLevels
    }

    public GameState currentState = GameState.InLevel;

    // private string firstDefaultTextIndicator = "Changement de mouvement dans ";
    // private string secondDefaultTextIndicator = "...";

    // [HideInInspector] public GameObject GO_parentChangeLevelBar;
    public int currentLevel = 1;
    public int nextTimeBeforeChangeLevel;
    private int currentStepLevel = 0;

    [Header("Interfaces")]
    public TextMeshProUGUI textLevel;
    public TextMeshProUGUI textIndicator;
    // public TextMeshProUGUI textNewMovement;
    private string defaultTextLevel = "Niveau ";
    public ActionUX leftActionUX;
    public ActionUX rightActionUX;
    // public ChangeLevelBar changeLevelBar;

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
        // GO_parentChangeLevelBar = changeLevelBar.transform.parent.gameObject;

        if (instance)
        {
            Debug.Log("Il existe déjà une instance de LevelManager dans cette scène");
            return;
        }
        instance = this;
    }
    

    void Start()
    {
        nextTimeBeforeChangeLevel = ScoreManager.TimeForNextLevel;
        // ATTENTION ON SE BASE PLUS SUR LE SCORE MAIS SUR LE TIME
        // changeLevelBar.SetNewValues(ScoreManager.instance.score, nextScoreBeforeChangeLevel); 
        // ChangeActionUX();
        RefreshTextLevel();
    }

    void Update()
    {
        switch (currentState)
        {
            case GameState.InLevel:
                UpdateInLevel();
                break;

            case GameState.BetweenLevels:
                UpdateBetweenLevels();
                break;
        }
    }

    public void UpdateInLevel()
    {
        obstaclesFactory.isGenerationEnabled = true;
        // ATTENTION ON SE BASE PLUS SUR LE SCORE MAIS SUR LE TIME
        // changeLevelBar.SetValue(ScoreManager.instance.score);
        int numberBeforeChangeLevel = nextTimeBeforeChangeLevel - DataStorage.instance.time;
        if (numberBeforeChangeLevel <= 3)
        {
            // PrintTextIndicator(true, numberBeforeChangeLevel);
        }

        if (ScoreManager.instance.stepTime >= nextTimeBeforeChangeLevel)
        {
            currentState = GameState.BetweenLevels;
            currentLevel++;
            currentStepLevel++;

            RefreshTextLevel();
            // ATTENTION ON SE BASE PLUS SUR LE SCORE MAIS SUR LE TIME
            // changeLevelBar.SetNewValues(ScoreManager.instance.score, nextScoreBeforeChangeLevel);
            // ChangeMovement();
            
            nextTimeBeforeChangeLevel += ScoreManager.TimeBetweenLevel;
        }
    }

    void UpdateBetweenLevels()
    {
        obstaclesFactory.isGenerationEnabled = false;
        if (ScoreManager.instance.stepTime >= nextTimeBeforeChangeLevel)
        {
            currentStepLevel++;
            currentState = GameState.InLevel; // Passe à l'état suivant lorsque nécessaire
            nextTimeBeforeChangeLevel += ScoreManager.TimeForNextLevel;
        }
    }


    /* void ChangeMovement()
    {
        leftMovement = movementList[Random.Range(0, movementList.Length)];
        rightMovement = movementList[Random.Range(0, movementList.Length)];
        // ChangeActionUX();
        // StartCoroutine(PrintTextNewMovement());
    } */

    void RefreshTextLevel()
    {
        textLevel.text = defaultTextLevel + currentLevel.ToString();
    }

    /* void ChangeActionUX()
    {
        leftActionUX.PrintImage(leftMovement);
        rightActionUX.PrintImage(rightMovement);
    } */

    /* void PrintTextIndicator(bool enabled, int number = 0)
    {
        // GO_parentChangeLevelBar.SetActive(false);
        textIndicator.enabled = enabled;
        textIndicator.text = firstDefaultTextIndicator + number.ToString() + secondDefaultTextIndicator;
    } */

    /* IEnumerator PrintTextNewMovement()
    {
        // PrintTextIndicator(false);
        textNewMovement.enabled = true;
        yield return new WaitForSeconds(2f);
        textNewMovement.enabled = false;
        // GO_parentChangeLevelBar.SetActive(true);
    } */
}
