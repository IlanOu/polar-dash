using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{

    [Header("UI elements")]
    public GameObject InGameUI;
    public GameObject DeathUI;

    [Header("Transition GameOver")]
    public GameObject gameOverUI;
    public float speed, time, delay;

    public static UIManager instance;

    void Awake()
    {
        if(instance != null)
        {
            Debug.Log("Il existe déjà une instance de UIManager dans cette scène");
            return;
        }
        instance = this;
    }

    void Update() {
        if (DyingSystem.instance.isDead){
            InGameUI.SetActive(false);
            DeathUI.SetActive(true);
        }else{
            InGameUI.SetActive(true);
            DeathUI.SetActive(false);
        }
    }

    public float StartTransitionToGameOverScene()
    {
        float x = 0f, y = -540.26f;
        Vector2 targetPosition = new Vector2(x, y);
        gameOverUI.SetActive(true);
        StartCoroutine(ActiveTransitionGameOver(targetPosition));
        return time + delay;
    }

    IEnumerator ActiveTransitionGameOver(Vector2 targetPosition)
    {
        yield return new WaitForSeconds(delay);
        float t = 0f;

        RectTransform rectTransform = gameOverUI.GetComponent<RectTransform>();
        Vector2 startPosition = rectTransform.anchoredPosition3D;

        while (t < time)
        {
            t += Time.deltaTime * speed;

            rectTransform.anchoredPosition3D = Vector3.Lerp(startPosition, targetPosition, t);
            yield return null;
        }
    }
}
