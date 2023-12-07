using System.Collections;
using TMPro;
using UnityEngine;

public class WriteData : MonoBehaviour
{
    [Header("Data smooth animation")]
    public float timeAnimation;
    public float edge0;
    public float edge1;

    [Header("Calories")]
    public Vector2 caloriesTargetPosition;
    public Vector2 caloriesTargetSize;
    public float caloriesTargetRotation;
    public float textCaloriesDescFontSizeTarget;
    public float textCaloriesFontSizeTarget;
    public GameObject calories;
    public TextMeshProUGUI textCaloriesDesc;
    public TextMeshProUGUI textCalories;

    [Header("Score")]
    public Vector2 scoreTargetPosition;
    public Vector2 scoreTargetSize;
    public float scoreTargetRotation;
    public float textScoreDescFontSizeTarget;
    public float textScoreFontSizeTarget;
    public GameObject score;
    public TextMeshProUGUI textScoreDesc;
    public TextMeshProUGUI textScore;

    [Header("Obstacles")]
    public Vector2 obstaclesTargetPosition;
    public Vector2 obstaclesTargetSize;
    public float obstaclesTargetRotation;
    public float textObstaclesDescFontSizeTarget;
    public float textObstaclesFontSizeTarget;
    public GameObject obstacles;
    public TextMeshProUGUI textObstaclesDesc;
    public TextMeshProUGUI textObstacles;
    
    [Header("Time")]
    public Vector2 timeTargetPosition;
    public Vector2 timeTargetSize;
    public float timeTargetRotation;
    public float textTimeDescFontSizeTarget;
    public float textTimeFontSizeTarget;
    public GameObject time;
    public TextMeshProUGUI textTimeDesc;
    public TextMeshProUGUI textTime;
    
    [Header("Distance")]
    public Vector2 distanceTargetPosition;
    public Vector2 distanceTargetSize;
    public float distanceTargetRotation;
    public float textDistanceDescFontSizeTarget;
    public float textDistanceFontSizeTarget;
    public GameObject distance;
    public TextMeshProUGUI textDistanceDesc;
    public TextMeshProUGUI textDistance;

    private string defaultTextObstacles = " OBSTACLES";
    void Start()
    {
        // textScore.SetText(DataStorage.instance.score.ToString());
        // textCalories.SetText(DataStorage.instance.burnedCalories.ToString());
        // textObstacles.SetText(DataStorage.instance.dodgedObstacle.ToString() + defaultTextObstacles);
        // TEST
        textScore.SetText("123");
        textCalories.SetText("321");
        textObstacles.SetText("12" + defaultTextObstacles);
        StartCoroutine(StartDataAnimation());
    }

    IEnumerator StartDataAnimation()
    {
        float waiting = 0.8f;
        yield return new WaitForSeconds(waiting);

        calories.SetActive(true);
        yield return new WaitForSeconds(waiting);
        StartCoroutine(StartObjectAnimation(calories, textCaloriesDesc, textCalories, caloriesTargetPosition, caloriesTargetSize, caloriesTargetRotation, textCaloriesDescFontSizeTarget, textCaloriesFontSizeTarget));

        yield return new WaitForSeconds(timeAnimation);

        score.SetActive(true);
        yield return new WaitForSeconds(waiting);
        StartCoroutine(StartObjectAnimation(score, textScoreDesc, textScore, scoreTargetPosition, scoreTargetSize, scoreTargetRotation, textScoreDescFontSizeTarget, textScoreFontSizeTarget));

        yield return new WaitForSeconds(timeAnimation);

        obstacles.SetActive(true);
        yield return new WaitForSeconds(waiting);
        StartCoroutine(StartObjectAnimation(obstacles, textObstaclesDesc, textObstacles, obstaclesTargetPosition, obstaclesTargetSize, obstaclesTargetRotation, textObstaclesDescFontSizeTarget, textObstaclesFontSizeTarget));
        
        time.SetActive(true);
        yield return new WaitForSeconds(waiting);
        StartCoroutine(StartObjectAnimation(time, textTimeDesc, textTime, timeTargetPosition, timeTargetSize, timeTargetRotation, textTimeDescFontSizeTarget, textTimeFontSizeTarget));
        
        distance.SetActive(true);
        yield return new WaitForSeconds(waiting);
        StartCoroutine(StartObjectAnimation(distance, textDistanceDesc, textDistance, distanceTargetPosition, distanceTargetSize, distanceTargetRotation, textDistanceDescFontSizeTarget, textDistanceFontSizeTarget));
    }

    IEnumerator StartObjectAnimation(GameObject gameObject, TextMeshProUGUI textDesc, TextMeshProUGUI text, Vector2 targetPosition, Vector2 targetSize, float targetRotation, float descFontSizeTarget, float fontSizeTarget)
    {
        float t = 0f;

        Vector2 initialPosition = gameObject.GetComponent<RectTransform>().anchoredPosition;
        Vector2 initialSize = gameObject.GetComponent<RectTransform>().sizeDelta;
        Quaternion initialRotation = gameObject.GetComponent<RectTransform>().rotation;
        float initialDescFontSize = textCaloriesDesc.fontSize;
        float initialFontSize = text.fontSize;

        while (t < timeAnimation)
        {
            t += Time.deltaTime;

            float smoothStep = Mathf.SmoothStep(edge0, edge1, t / timeAnimation);

            gameObject.GetComponent<RectTransform>().anchoredPosition = Vector2.Lerp(initialPosition, targetPosition, smoothStep);
            gameObject.GetComponent<RectTransform>().sizeDelta = Vector2.Lerp(initialSize, targetSize, smoothStep);
            gameObject.GetComponent<RectTransform>().rotation = Quaternion.Euler(0f, 0f, Mathf.LerpAngle(initialRotation.eulerAngles.z, targetRotation, smoothStep));
            textDesc.fontSize = Mathf.Lerp(initialDescFontSize, descFontSizeTarget, smoothStep);
            text.fontSize = Mathf.Lerp(initialFontSize, fontSizeTarget, smoothStep);

            yield return null;
        }
    }
}
