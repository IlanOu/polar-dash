using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public bool isRunning = true;
    public static GameManager instance;
    // Start is called before the first frame update
    void Awake()
    {
        if (instance != null){
            Debug.Log("Il existe déjà une instance de GameManager dans cette scene...");
            return;
        }

        instance = this;
    }
}
