using UnityEngine;

public class DataTreat : MonoBehaviour
{
    public string playerSide;
    public string movementPerformed;
    public bool leftPlayerPresence;
    public bool rightPlayerPresence;

    public static DataTreat instance;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.Log("Il existe déjà cette instance de DataTreat dans cette scène");
            Destroy(gameObject);
            return;
        }
        instance = this;
    }

    public void TreatTextReceived(string text)
    {
        string[] data = text.Split(':');

        if (data.Length != 2)
        {
            Debug.LogError("Format de données invalide");
            return;
        }

        string side = data[0].ToLower();
        string movement = data[1].ToLower();

        if (movement == "here" || movement == "nothere")
        {
            SetPlayerPresence(side, movement);
        }
        else
        {
            playerSide = side;
            movementPerformed = movement;
        }
    }

    private void SetPlayerPresence(string side, string presence)
    {
        switch (side)
        {
            case "left":
                leftPlayerPresence = presence == "here";
                break;
            case "right":
                rightPlayerPresence = presence == "here";
                break;
        }
    }

    public void ResetTextReceived()
    {
        playerSide = "";
        movementPerformed = "";
        leftPlayerPresence = false;
        rightPlayerPresence = false;
    }
}
