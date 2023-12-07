using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ObstacleCounter : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player" && !DyingSystem.instance.isDead)
        {
            DataStorage.instance.dodgedObstacle++;
        }
    }
}
