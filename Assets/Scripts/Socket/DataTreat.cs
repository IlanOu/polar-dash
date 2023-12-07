using UnityEngine;

public class DataTreat : MonoBehaviour
{
    public string playerSide;
    public string movementPerformed;
    public bool leftPlayerPresence;
    public bool rightPlayerPresence;
    public UdpSocket udpSocket;
    public string imagePath = null;

    public static DataTreat instance;

    private void Awake()
    {

        udpSocket = GetComponent<UdpSocket>();

        if (instance != null)
        {
            Debug.Log("Il existe déjà cette instance de DataTreat dans cette scène");
            Destroy(this.gameObject);
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

        if(data[0].ToLower() == "image")
        {
            imagePath = data[1];
        }
        else
        {
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
                ScoreManager.instance.UpdateDictionnary(side, movement);
            }
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

    public void SendMessageToTakePhoto()
    {
        udpSocket.SendData("takePhoto");
    }
}
