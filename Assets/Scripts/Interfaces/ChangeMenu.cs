using UnityEngine;

public class ChangeMenu : MonoBehaviour
{
    public GameObject selectionMenu;
    public GameObject waitingMenu;
    public GameObject textWaitingForPlayer;

    private float timeSinceBothPlayersPresent = 0f;
    private float timeSincePlayerAbsent = 0f;
    private float activationDelay = 3f;

    void Update()
    {
        bool leftPlayerPresent = DataTreat.instance.leftPlayerPresence;
        bool rightPlayerPresent = DataTreat.instance.rightPlayerPresence;

        if (leftPlayerPresent && rightPlayerPresent)
        {
            timeSincePlayerAbsent = 0f;
            timeSinceBothPlayersPresent += Time.deltaTime;

            if (timeSinceBothPlayersPresent >= activationDelay)
            {
                ActivateMenus(true, false);
                ActivateTextWaiting(false); // Désactiver instantanément le texte
            }
        }
        else
        {
            timeSinceBothPlayersPresent = 0f;
            timeSincePlayerAbsent += Time.deltaTime;

            ActivateTextWaiting((leftPlayerPresent && !rightPlayerPresent) || (rightPlayerPresent && !leftPlayerPresent));
            if (timeSincePlayerAbsent >= activationDelay)
            {
                ActivateMenus(false, true);
            }
            else
            {
                ActivateTextWaiting(false); // Désactiver instantanément le texte
            }
        }
    }

    void ActivateMenus(bool activateSelectionMenu, bool activateWaitingMenu)
    {
        selectionMenu.SetActive(activateSelectionMenu);
        waitingMenu.SetActive(activateWaitingMenu);
    }

    void ActivateTextWaiting(bool activate)
    {
        if (waitingMenu.activeSelf)
        {
            // Utilisation de SetActive(false) pour désactiver instantanément le texte
            textWaitingForPlayer.SetActive(activate);
        }
    }
}
