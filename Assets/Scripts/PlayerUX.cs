using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUX : MonoBehaviour
{
    public Image present;
    public Image absent;
    public string sideName;

    void Start()
    {
        absent.enabled = false;
    }

    void Update()
    {
        switch(sideName)
        {
            case "left":
                SetImage(DataTreat.instance.leftPlayerPresence);
                break;
            case "right":
                SetImage(DataTreat.instance.rightPlayerPresence);
                break;
        }
    }

    void SetImage(bool presence)
    {
        present.enabled = presence;
        absent.enabled = !presence;
    }
}
