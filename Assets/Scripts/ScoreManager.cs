using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public int score = 0;
    public bool isCurrent = false;
    public TextMeshProUGUI textScore;

    private string defaultText = "Score : ";
    private bool scoreUpdateInProgress = false;

    public static ScoreManager instance;
    void Awake()
    {
        if (instance != null)
        {
            Debug.Log("IL existe déjà une instance de ScoreManager dans cette scène");
            return;
        }
        instance = this;
    }
    
    void Start()
    {
        isCurrent = true;
        UpdateScoreText();
    }

    void Update()
    {
        if (!scoreUpdateInProgress && isCurrent)
        {
            scoreUpdateInProgress = true;
            StartCoroutine(AddScoreEverySecond());
        }
    }

    IEnumerator AddScoreEverySecond()
    {
        if (isCurrent)
        {
            yield return new WaitForSeconds(1f);
            scoreUpdateInProgress = false;

            score++;
            UpdateScoreText();
        }
    }

    void UpdateScoreText()
    {
        textScore.text = defaultText + score.ToString();
    }

}
