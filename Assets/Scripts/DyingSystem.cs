using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DyingSystem : MonoBehaviour
{
    public bool isDead = false;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Obstacle"){
            GameManager.instance.isRunning = false;
            isDead = true;
            Debug.Log("Vous avez perdu !!");
        }
    }
}
