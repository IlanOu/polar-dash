using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ChangeLevelBar : MonoBehaviour
{
    public Slider slider;
    public float smoothSpeed = 5f;

    public void SetNewValues(int minValue, int maxValue)
    {
        slider.minValue = minValue;
        slider.maxValue = maxValue;
        slider.value = minValue;
    }

    public void SetValue(int value)
    {
        slider.value = value;
    }
}
