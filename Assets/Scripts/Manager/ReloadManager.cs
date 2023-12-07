using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ReloadManager : MonoBehaviour
{
    public float time;
    void Start()
    {
        StartCoroutine(WaitForPresence());
    }

    IEnumerator WaitForPresence()
    {
        yield return new WaitForSeconds(time);
        if(DataTreat.instance.leftPlayerPresence && DataTreat.instance.rightPlayerPresence)
        {
            SceneManager.LoadScene("Game");
        }
        else
        {
            SceneManager.LoadScene("MainMenu");
        }
    }
}
