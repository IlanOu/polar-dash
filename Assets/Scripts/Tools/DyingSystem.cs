using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DyingSystem : MonoBehaviour
{
    [HideInInspector]
    public bool isDead = false;
    public static DyingSystem instance;

    private void Awake() {
        if (instance != null){
            Debug.Log("Il existe déjà une instance de GameManager dans cette scene...");
            return;
        }

        instance = this;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Obstacle"){
            GameManager.instance.isRunning = false;
            isDead = true;
        }
    }
}
