using System.Collections;
using TMPro;
using UnityEngine;

public class WriteData : MonoBehaviour
{
    [Header("Data smooth animation")]
    public float timeAnimation;
    public float timePrinting; 
    public float edge0;
    public float edge1;

    [Header("Calories")]
    public Vector2 caloriesTargetPosition;
    public Vector2 caloriesTargetSize;
    public float caloriesTargetRotation;
    public float textCaloriesDescFontSizeTarget;
    public Vector2 textDescCaloriesTargetSize;
    public float textCaloriesFontSizeTarget;
    public Vector2 textCaloriesTargetSize;
    public GameObject calories;
    public TextMeshProUGUI textCaloriesDesc;
    public TextMeshProUGUI textCalories;

    [Header("Score")]
    public Vector2 scoreTargetPosition;
    public Vector2 scoreTargetSize;
    public float scoreTargetRotation;
    public float textScoreDescFontSizeTarget;
    public Vector2 textDescScoreTargetSize;
    public float textScoreFontSizeTarget;
    public Vector2 textScoreTargetSize;
    public GameObject score;
    public TextMeshProUGUI textScoreDesc;
    public TextMeshProUGUI textScore;

    [Header("Obstacles")]
    public Vector2 obstaclesTargetPosition;
    public Vector2 obstaclesTargetSize;
    public float obstaclesTargetRotation;
    public float textObstaclesDescFontSizeTarget;
    public Vector2 textDescObstaclesTargetSize;
    public float textObstaclesFontSizeTarget;
    public Vector2 textObstaclesTargetSize;
    public GameObject obstacles;
    public TextMeshProUGUI textObstaclesDesc;
    public TextMeshProUGUI textObstacles;
    
    [Header("Time")]
    public Vector2 timeTargetPosition;
    public Vector2 timeTargetSize;
    public float timeTargetRotation;
    public float textTimeDescFontSizeTarget;
    public Vector2 textDescTimeTargetSize;
    public float textTimeFontSizeTarget;
    public Vector2 textTimeTargetSize;
    public GameObject time;
    public TextMeshProUGUI textTimeDesc;
    public TextMeshProUGUI textTime;
    
    [Header("Distance")]
    public Vector2 distanceTargetPosition;
    public Vector2 distanceTargetSize;
    public float distanceTargetRotation;
    public float textDistanceDescFontSizeTarget;
    public Vector2 textDescDistanceTargetSize;
    public float textDistanceFontSizeTarget;
    public Vector2 textDistanceTargetSize;
    public GameObject distance;
    public TextMeshProUGUI textDistanceDesc;
    public TextMeshProUGUI textDistance;
    private string defaultTextObstacles = " OBSTACLES";
    private string defaultTextTime = "\nSEC";
    private string defaultTextDistance = " Mètres";
    void Start()
    {
        textCalories.SetText(DataStorage.instance.burnedCalories.ToString());
        textScore.SetText(DataStorage.instance.score.ToString());
        textObstacles.SetText(DataStorage.instance.dodgedObstacle.ToString() + defaultTextObstacles);
        textTime.SetText(DataStorage.instance.time.ToString() + defaultTextTime);
        textDistance.SetText(DataStorage.instance.distance.ToString() + defaultTextDistance);
        StartCoroutine(StartDataAnimation());
    }

    IEnumerator StartDataAnimation()
    {
        calories.SetActive(true);
        yield return new WaitForSeconds(timePrinting);
        StartCoroutine(StartObjectAnimation(calories, textCaloriesDesc, textCalories, caloriesTargetPosition, caloriesTargetSize, caloriesTargetRotation, textCaloriesDescFontSizeTarget, textCaloriesFontSizeTarget, textDescCaloriesTargetSize, textCaloriesTargetSize));

        yield return new WaitForSeconds(timeAnimation);

        score.SetActive(true);
        yield return new WaitForSeconds(timePrinting);
        StartCoroutine(StartObjectAnimation(score, textScoreDesc, textScore, scoreTargetPosition, scoreTargetSize, scoreTargetRotation, textScoreDescFontSizeTarget, textScoreFontSizeTarget, textDescScoreTargetSize, textScoreTargetSize));

        yield return new WaitForSeconds(timeAnimation);

        obstacles.SetActive(true);
        yield return new WaitForSeconds(timePrinting);
        StartCoroutine(StartObjectAnimation(obstacles, textObstaclesDesc, textObstacles, obstaclesTargetPosition, obstaclesTargetSize, obstaclesTargetRotation, textObstaclesDescFontSizeTarget, textObstaclesFontSizeTarget, textDescObstaclesTargetSize, textObstaclesTargetSize));
        
        yield return new WaitForSeconds(timeAnimation);

        time.SetActive(true);
        yield return new WaitForSeconds(timePrinting);
        StartCoroutine(StartObjectAnimation(time, textTimeDesc, textTime, timeTargetPosition, timeTargetSize, timeTargetRotation, textTimeDescFontSizeTarget, textTimeFontSizeTarget, textDescTimeTargetSize, textTimeTargetSize));
        
        yield return new WaitForSeconds(timeAnimation);

        distance.SetActive(true);
        yield return new WaitForSeconds(timePrinting);
        StartCoroutine(StartObjectAnimation(distance, textDistanceDesc, textDistance, distanceTargetPosition, distanceTargetSize, distanceTargetRotation, textDistanceDescFontSizeTarget, textDistanceFontSizeTarget, textDescDistanceTargetSize, textDistanceTargetSize));
    }

    IEnumerator StartObjectAnimation(GameObject gameObject, TextMeshProUGUI textDesc, TextMeshProUGUI text, Vector2 targetPosition, Vector2 targetSize, float targetRotation, float descFontSizeTarget, float fontSizeTarget, Vector2 textDescTargetSize, Vector2 textTargetSize)
    {
        float t = 0f;

        Vector2 initialPosition = gameObject.GetComponent<RectTransform>().anchoredPosition;
        Vector2 initialSize = gameObject.GetComponent<RectTransform>().sizeDelta;
        Quaternion initialRotation = gameObject.GetComponent<RectTransform>().rotation;
        float initialDescFontSize = textDesc.fontSize;
        float initialFontSize = text.fontSize;
        Vector2 textDescSizeInitialSize = textDesc.gameObject.GetComponent<RectTransform>().sizeDelta;
        Vector2 textSizeInitialSize = text.gameObject.GetComponent<RectTransform>().sizeDelta;

        while (t < timeAnimation)
        {
            t += Time.deltaTime;

            float smoothStep = Mathf.SmoothStep(edge0, edge1, t / timeAnimation);

            gameObject.GetComponent<RectTransform>().anchoredPosition = Vector2.Lerp(initialPosition, targetPosition, smoothStep);
            gameObject.GetComponent<RectTransform>().sizeDelta = Vector2.Lerp(initialSize, targetSize, smoothStep);
            gameObject.GetComponent<RectTransform>().rotation = Quaternion.Euler(0f, 0f, Mathf.LerpAngle(initialRotation.eulerAngles.z, targetRotation, smoothStep));
            textDesc.fontSize = Mathf.Lerp(initialDescFontSize, descFontSizeTarget, smoothStep);
            text.fontSize = Mathf.Lerp(initialFontSize, fontSizeTarget, smoothStep);
            textDesc.gameObject.GetComponent<RectTransform>().sizeDelta = Vector2.Lerp(textDescSizeInitialSize, textDescTargetSize, smoothStep);
            text.gameObject.GetComponent<RectTransform>().sizeDelta = Vector2.Lerp(textSizeInitialSize, textTargetSize, smoothStep);

            yield return null;
        }
    }
}
