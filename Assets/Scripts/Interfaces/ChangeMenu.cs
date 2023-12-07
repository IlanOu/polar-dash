using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;


public class ChangeMenu : MonoBehaviour
{
    [Header("Menus")]
    public GameObject introMenu;
    public GameObject waitingMenu;
    public GameObject selectionMenu;

    [Header("Objects")]
    public GameObject objectTextWaitingForPlayer;
    private TextMeshProUGUI textWaitingForPlayer;

    private enum MenuState
    {
        Intro,
        Waiting,
        Selection
    }

    private float delayBetweenStates = 2f;

    private MenuState currentMenuState = MenuState.Intro;

    void Start()
    {
        textWaitingForPlayer = objectTextWaitingForPlayer.GetComponent<TextMeshProUGUI>();
        // Commencez par le premier état
        StartCoroutine(ChangeState());
    }

    void Update()
    {
        // Logique d'état mise en œuvre ici
    }

    IEnumerator ChangeState()
    {
        // Coroutine pour changer l'état

        yield return new WaitForSeconds(delayBetweenStates); // Attendre le délai spécifié

        bool leftPlayerPresent = DataTreat.instance.leftPlayerPresence;
        bool rightPlayerPresent = DataTreat.instance.rightPlayerPresence;

        if (!leftPlayerPresent && !rightPlayerPresent)
        {
            currentMenuState = MenuState.Intro;
        }
        else if (!leftPlayerPresent || !rightPlayerPresent)
        {
            currentMenuState = MenuState.Waiting;
        }
        else
        {
            currentMenuState = MenuState.Selection;
        }

        switch (currentMenuState)
        {
            case MenuState.Intro:
                HandleIntroState();
                break;
            case MenuState.Waiting:
                HandleWaitingState(leftPlayerPresent, rightPlayerPresent);
                break;
            case MenuState.Selection:
                HandleSelectionState(leftPlayerPresent, rightPlayerPresent);
                break;
        }

        // Changer à l'état suivant après le délai
        yield return new WaitForSeconds(delayBetweenStates);
        StartCoroutine(ChangeState());
    }

    void HandleIntroState()
    {
        // Logique pour l'état Intro
        ChangeImage(introMenu);
    }

    void HandleWaitingState(bool leftPlayerPresent, bool rightPlayerPresent)
    {
        if (leftPlayerPresent && rightPlayerPresent){
            textWaitingForPlayer.text = "Lancement du jeu";
            objectTextWaitingForPlayer.SetActive(true);
        }else if ((leftPlayerPresent && !rightPlayerPresent) || (!leftPlayerPresent && rightPlayerPresent)){
            textWaitingForPlayer.text = "En attente d’un deuxième joueur";
            objectTextWaitingForPlayer.SetActive(true);
        }else{
            objectTextWaitingForPlayer.SetActive(false);
        }
        // Logique pour l'état Waiting
        ChangeImage(waitingMenu);
    }

    void HandleSelectionState(bool leftPlayerPresence, bool rightPlayerPresent)
    {
        // Logique pour l'état Selection
        ChangeImage(selectionMenu);
    }

    void ChangeImage(GameObject objectToActivate)
    {
        List<GameObject> objectsToDeactivate = new List<GameObject> { introMenu, waitingMenu, selectionMenu };

        foreach (GameObject objectToDeactivate in objectsToDeactivate)
        {
            objectToDeactivate.SetActive(false);
        }

        objectToActivate.SetActive(true);
    }
}
