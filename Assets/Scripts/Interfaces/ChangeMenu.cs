using UnityEngine;

public class ChangeMenu : MonoBehaviour
{
    public GameObject selectionMenu;
    public GameObject waitingMenu;
    public GameObject textWaitingForPlayer;

    void Update()
    {
        if(DataTreat.instance.leftPlayerPresence == true && DataTreat.instance.rightPlayerPresence == true)
        {
            selectionMenu.SetActive(true);
            waitingMenu.SetActive(false);
        }
        else{
            selectionMenu.SetActive(false);
            waitingMenu.SetActive(true);
        }

        if (DataTreat.instance.leftPlayerPresence == true && DataTreat.instance.rightPlayerPresence == false || DataTreat.instance.leftPlayerPresence == false && DataTreat.instance.rightPlayerPresence == true){
            textWaitingForPlayer.SetActive(true);
        }else{
            textWaitingForPlayer.SetActive(false);
        }
    }
}
