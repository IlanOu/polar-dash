using UnityEngine;

public class DataTreat : MonoBehaviour
{
    public string playerSide;
    public string movementPerformed;
    
    public static DataTreat instance;

    void Awake()
    {
        if(instance != null)
        {
            Debug.Log("Il existe déjà cette instance de DataTreat dans cette scène");
            return;
        }
        instance = this;
    }

    public void TreatTextReceived(string text)
    {
        string[] data = text.Split(":");
        playerSide = data[0];
        movementPerformed = data[1];
    }

    public void ResetTextReceived()
    {
        playerSide = "";
        movementPerformed = "";
    }
}
