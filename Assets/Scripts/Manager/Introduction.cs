using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Introduction : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip introSound;

    public GameObject textNootNoot;
    public GameObject logo;

    public float showTextDelay = 2.5f; // Délai avant d'afficher le texte
    public float hideTextDelay = 3.4f; // Délai avant de cacher le texte
    public float showLogoDelay = 5.9f; // Délai avant d'afficher le logo
    public float hideLogoDelay = 5f; // Délai avant de cacher le logo
    public float fadeOutDelay = 8.5f; // Délai avant de débuter le fondu

    // Start is called before the first frame update
    void Start()
    {
        textNootNoot.SetActive(false);
        logo.SetActive(false);
        
        StartCoroutine(PlayIntroSound());
        StartCoroutine(ShowAndHideText());
        StartCoroutine(ShowAndHideLogo());
        StartCoroutine(FadeOut());
    }

    // Coroutine pour jouer le son avec une petite pause
    IEnumerator PlayIntroSound()
    {
        yield return new WaitForSeconds(1f); // Attends 1 seconde avant de jouer le son

        if (audioSource != null && introSound != null)
        {
            audioSource.PlayOneShot(introSound);
        }
    }

    // Coroutine pour afficher et cacher le texte
    IEnumerator ShowAndHideText()
    {
        yield return new WaitForSeconds(showTextDelay); // Attends avant d'afficher le texte
        textNootNoot.SetActive(true);

        yield return new WaitForSeconds(hideTextDelay); // Attends avant de cacher le texte
        textNootNoot.SetActive(false);
    }

    // Coroutine pour afficher et cacher le logo
    IEnumerator ShowAndHideLogo()
    {
        yield return new WaitForSeconds(showLogoDelay); // Attends avant d'afficher le logo
        logo.SetActive(true);

        StartCoroutine(StartLogoAnimation());

        yield return new WaitForSeconds(hideLogoDelay); // Attends avant de cacher le logo
        logo.SetActive(false);
    }

    // Coroutine pour le fondu (fade) vers l'opacité 0
    IEnumerator FadeOut()
    {
        yield return new WaitForSeconds(hideLogoDelay + fadeOutDelay); // Attends avant de débuter le fondu

        Image logoImage = logo.GetComponent<Image>();
        Color logoColor = logoImage.color;

        float t = 0f;
        while (t < fadeOutDelay)
        {
            t += Time.deltaTime;

            float alpha = Mathf.Lerp(1f, 0f, t / fadeOutDelay);
            logoImage.color = new Color(logoColor.r, logoColor.g, logoColor.b, alpha);
            yield return null;
        }

        SceneManager.LoadScene("MainMenu");
    }

    // Coroutine pour l'animation du logo (à compléter si nécessaire)
    IEnumerator StartLogoAnimation()
    {
        Vector2 initialSize = logo.GetComponent<RectTransform>().sizeDelta;
        Vector2 targetSize = new Vector2(905.2627f, 460.1752f);

        float t = 0f;
        while (t < hideLogoDelay)
        {
            t += Time.deltaTime;

            float smoothStep = Mathf.SmoothStep(0f, 1f, t / hideLogoDelay);
            logo.GetComponent<RectTransform>().sizeDelta = Vector2.Lerp(initialSize, targetSize, smoothStep);
            yield return null;
        }
    }
}
