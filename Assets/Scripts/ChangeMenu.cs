using UnityEngine;

public class ChangeMenu : MonoBehaviour
{
    public GameObject selectionMenu;
    public GameObject waitingMenu;
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
    }
}
