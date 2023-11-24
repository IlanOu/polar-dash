using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{

    [Header("UI elements")]
    public GameObject InGameUI;
    public GameObject DeathUI;

    void Update() {
        if (DyingSystem.instance.isDead){
            InGameUI.SetActive(false);
            DeathUI.SetActive(true);
        }else{
            InGameUI.SetActive(true);
            DeathUI.SetActive(false);
        }
    }
}
