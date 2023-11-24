using UnityEngine;
using UnityEngine.UI;

public class ActionUX : MonoBehaviour
{
    public Image[] images;

    public void PrintImage(string imageName)
    {
        foreach (Image image in images)
        {
            if (image.name == imageName)
            {
                image.enabled = true;
            }
            else
            {
                image.enabled = false;
            }
        }
    }
}
