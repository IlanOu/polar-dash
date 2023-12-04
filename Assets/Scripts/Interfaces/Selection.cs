using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Selection : MonoBehaviour
{
    public string HowToPlaySelection = "bottom";
    public string PlaySelection = "top";
    private string leftMovement;
    private string rightMovement;
    public string gameScene = "Game";
    
    void Update()
    {
        if(DataTreat.instance.playerSide == "left")
        {
            leftMovement = DataTreat.instance.movementPerformed;
        }
        if(DataTreat.instance.playerSide == "right")
        {
            rightMovement = DataTreat.instance.movementPerformed;
        }
        if(leftMovement == rightMovement)
        {
            if(leftMovement == HowToPlaySelection)
            {
                // SCENE DE TUTO
                Debug.Log("TUTO");
            }
            if(leftMovement == PlaySelection)
            {
                // SCENE JEU
                Debug.Log("GAME");
                SceneManager.LoadScene(gameScene);
            }
        }
        if(Input.GetKeyDown(KeyCode.G))
        {
            SceneManager.LoadScene(gameScene);
        }
    }
}
