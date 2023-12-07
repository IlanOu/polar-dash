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
    public float timeBeforeLoadScene = 3f;
    
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
                // SceneManager.LoadScene(gameScene);
                float timeBeforeLoadScene = StartingGameAnim.instance.StartTransitionToGameScene();
                StartCoroutine(WaitingBeforeLoadScene(timeBeforeLoadScene));
            }
        }
        if(Input.GetKeyDown(KeyCode.G))
        {
            float timeBeforeLoadScene = StartingGameAnim.instance.StartTransitionToGameScene();
            
            StartCoroutine(WaitingBeforeLoadScene(timeBeforeLoadScene));
        }
    }

    
    IEnumerator WaitingBeforeLoadScene(float delay)
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(gameScene);
    }
}
