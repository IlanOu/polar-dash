using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DyingSystem : MonoBehaviour
{
    [HideInInspector]
    public bool isDead = false;
    public static DyingSystem instance;

    private void Awake() {
        if (instance != null){
            Debug.Log("Il existe déjà une instance de DyingSystem dans cette scene...");
            return;
        }

        instance = this;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Obstacle"){
            ScoreManager.instance.UpdateBestScore();
            GameManager.instance.isRunning = false;
            GameManager.instance.addToDestroyOnLoad();
            isDead = true;
            DataTreat.instance.SendMessageToTakePhoto();
            SceneManager.LoadScene("GameOver");
        }
    }
}
