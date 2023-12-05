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
            CollisionWithObstacle();
        }
    }

    private void CollisionWithObstacle()
    {
        if(PlayerHealth.instance.TakeDamage(1) <= 0)
        {
            isDead = true;
            DeathPlayer();
        }
        
    }

    void DeathPlayer()
    {
        ScoreManager.instance.UpdateBestScore();
        GameManager.instance.isRunning = false;
        StartCoroutine(WaitingForTakingPhoto());
        StartCoroutine(WaitingForLoadScene());
    }

    IEnumerator WaitingForTakingPhoto()
    {
        yield return new WaitForSeconds(1f);
        DataTreat.instance.SendMessageToTakePhoto();
    }

    IEnumerator WaitingForLoadScene()
    {
        yield return new WaitForSeconds(3f);
        GameManager.instance.DisableBeforeGameOverScene();
        GameManager.instance.AddToDestroyOnLoad();
        SceneManager.LoadScene("GameOver");
    }
}
