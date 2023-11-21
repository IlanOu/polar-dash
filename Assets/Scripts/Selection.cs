using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Selection : MonoBehaviour
{
    public string HowToPlaySelection;
    public string PlaySelection;
    private string leftMovement;
    private string rightMovement;
    
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
                Debug.Log("JEU");
            }
        }
    }
}
