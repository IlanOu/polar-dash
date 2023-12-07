using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DeathUI : MonoBehaviour
{
    public TMP_Text scoreText;
    public TMP_Text LancementPartie;

    public int timeBeforeReplay = 5;
    bool coroutineStarted = false;

    void Update()
    {
        if (DyingSystem.instance.isDead && !coroutineStarted){
            StartCoroutine(waitBeforeRestartGame());
        }
    }

    IEnumerator waitBeforeRestartGame()
    {
        for (int i = timeBeforeReplay ; i > 0 ; i--)
        {
            coroutineStarted = true;
            LancementPartie.text = "La nouvelle partie reprendra dans " + i;
            yield return new WaitForSeconds(1f);
        }
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
        Debug.Log("Temps écoulé ! Lancement d'une nouvelle partie...");
    }
}
