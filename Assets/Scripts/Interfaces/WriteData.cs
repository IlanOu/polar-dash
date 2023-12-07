using System.Collections;
using TMPro;
using UnityEngine;

public class WriteData : MonoBehaviour
{
    [Header("Data smooth animation")]
    public float time;
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
        StartCoroutine(StartCaloriesAnimation());

        yield return new WaitForSeconds(time);

        score.SetActive(true);
        yield return new WaitForSeconds(waiting);
        StartCoroutine(StartScoreAnimation());

        yield return new WaitForSeconds(time);

        obstacles.SetActive(true);
        yield return new WaitForSeconds(waiting);
        StartCoroutine(StartObstaclesAnimation());
    }

    IEnumerator StartCaloriesAnimation()
    {
        float t = 0f;

        Vector2 initialPosition = calories.GetComponent<RectTransform>().anchoredPosition;
        Vector2 initialSize = calories.GetComponent<RectTransform>().sizeDelta;
        Quaternion initialRotation = calories.GetComponent<RectTransform>().rotation;
        float initialDescFontSize = textCaloriesDesc.fontSize;
        float initialFontSize = textCalories.fontSize;

        while (t < time)
        {
            t += Time.deltaTime;

            float smoothStep = Mathf.SmoothStep(edge0, edge1, t / time);

            calories.GetComponent<RectTransform>().anchoredPosition = Vector2.Lerp(initialPosition, caloriesTargetPosition, smoothStep);
            calories.GetComponent<RectTransform>().sizeDelta = Vector2.Lerp(initialSize, caloriesTargetSize, smoothStep);
            calories.GetComponent<RectTransform>().rotation = Quaternion.Euler(0f, 0f, Mathf.LerpAngle(initialRotation.eulerAngles.z, caloriesTargetRotation, smoothStep));
            textCaloriesDesc.fontSize = Mathf.Lerp(initialDescFontSize, textCaloriesDescFontSizeTarget, smoothStep);
            textCalories.fontSize = Mathf.Lerp(initialFontSize, textCaloriesFontSizeTarget, smoothStep);

            yield return null;
        }
    }
    
    IEnumerator StartScoreAnimation()
    {
        float t = 0f;

        Vector2 initialPosition = score.GetComponent<RectTransform>().anchoredPosition;
        Vector2 initialSize = score.GetComponent<RectTransform>().sizeDelta;
        Quaternion initialRotation = score.GetComponent<RectTransform>().rotation;
        float initialDescFontSize = textScoreDesc.fontSize;
        float initialFontSize = textScore.fontSize;

        while (t < time)
        {
            t += Time.deltaTime;

            float smoothStep = Mathf.SmoothStep(edge0, edge1, t / time);

            score.GetComponent<RectTransform>().anchoredPosition = Vector2.Lerp(initialPosition, scoreTargetPosition, smoothStep);
            score.GetComponent<RectTransform>().sizeDelta = Vector2.Lerp(initialSize, scoreTargetSize, smoothStep);
            score.GetComponent<RectTransform>().rotation = Quaternion.Euler(0f, 0f, Mathf.LerpAngle(initialRotation.eulerAngles.z, scoreTargetRotation, smoothStep));
            textScoreDesc.fontSize = Mathf.Lerp(initialDescFontSize, textScoreDescFontSizeTarget, smoothStep);
            textScore.fontSize = Mathf.Lerp(initialFontSize, textScoreFontSizeTarget, smoothStep);

            yield return null;
        }
    }
    
    IEnumerator StartObstaclesAnimation()
    {
        float t = 0f;

        Vector2 initialPosition = obstacles.GetComponent<RectTransform>().anchoredPosition;
        Vector2 initialSize = obstacles.GetComponent<RectTransform>().sizeDelta;
        Quaternion initialRotation = obstacles.GetComponent<RectTransform>().rotation;
        float initialDescFontSize = textObstaclesDesc.fontSize;
        float initialFontSize = textObstacles.fontSize;

        while (t < time)
        {
            t += Time.deltaTime;

            float smoothStep = Mathf.SmoothStep(edge0, edge1, t / time);

            obstacles.GetComponent<RectTransform>().anchoredPosition = Vector2.Lerp(initialPosition, obstaclesTargetPosition, smoothStep);
            obstacles.GetComponent<RectTransform>().sizeDelta = Vector2.Lerp(initialSize, obstaclesTargetSize, smoothStep);
            obstacles.GetComponent<RectTransform>().rotation = Quaternion.Euler(0f, 0f, Mathf.LerpAngle(initialRotation.eulerAngles.z, obstaclesTargetRotation, smoothStep));
            textObstaclesDesc.fontSize = Mathf.Lerp(initialDescFontSize, textObstaclesDescFontSizeTarget, smoothStep);
            textObstacles.fontSize = Mathf.Lerp(initialFontSize, textObstaclesFontSizeTarget, smoothStep);


            yield return null;
        }
    }
}
