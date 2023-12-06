using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TransitionUI : MonoBehaviour
{
    public float speed, time;
    [Header("Text title")]
    public GameObject textTitle;
    public Vector2 endingPositionTextTitle;

    [Header("Polaroid")] 
    public GameObject polaroid;
    public Vector2 endingPositionPolaroid;
    void Start()
    {
        StartCoroutine(TransitionTitlePolaroid());
    }

    IEnumerator TransitionTitlePolaroid()
    {
        float t = 0f;

        RectTransform textTitleRectTransform = textTitle.GetComponent<RectTransform>();
        Vector2 textTitleStartPosition = textTitleRectTransform.anchoredPosition3D;

        RectTransform polaroidRectTransform = polaroid.GetComponent<RectTransform>();
        Vector2 polaroidStartPosition = polaroidRectTransform.anchoredPosition3D;

        while (t < time)
        {
            t += Time.deltaTime * speed;

            textTitleRectTransform.anchoredPosition3D = Vector3.Lerp(textTitleStartPosition, endingPositionTextTitle, t);
            polaroidRectTransform.anchoredPosition3D = Vector3.Lerp(polaroidStartPosition, endingPositionPolaroid, t);
            yield return null;
        }
    }
}
