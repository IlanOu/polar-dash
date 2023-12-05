using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public bool isRunning = false;
    public int timer = 3;
    public TextMeshProUGUI textStartingGame;
    private GameObject GO_Parent;
    public static GameManager instance;
    // Start is called before the first frame update
    void Awake()
    {
        GO_Parent = textStartingGame.transform.parent.gameObject;

        if (instance != null){
            Debug.Log("Il existe déjà une instance de GameManager dans cette scene...");
            return;
        }

        instance = this;
    }

    void Start()
    {
        GO_Parent.SetActive(true);
        isRunning = false;
        StartCoroutine(waitBeforeStartGame());
    }

    IEnumerator waitBeforeStartGame()
    {
        for (int i = timer ; i > 0 ; i--)
        {
            PrintMessage(i.ToString());
            yield return new WaitForSeconds(1f);
        }
        isRunning = true;
        GO_Parent.SetActive(false);
    }

    private void PrintMessage(string number)
    {
        textStartingGame.text = number;
    }
}
