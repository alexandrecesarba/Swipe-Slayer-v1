using UnityEngine;
using UnityEngine.UI;

public class TimeBar : MonoBehaviour
{
    public Slider slider;
    public Gradient gradient;
    public Image fill;

    public void SetTime(float time)
    {
        if (slider != null && fill != null) {
            slider.value = time;
            fill.color = gradient.Evaluate(slider.normalizedValue);
        } else {
            Debug.LogWarning("Slider or Fill is null in TimeBar script.");
        }
    }

    public void SetMaxTime(float time)
    {
        if (slider != null && fill != null) {
            slider.maxValue = time;
            slider.value = time;
            fill.color = gradient.Evaluate(1f);
        } else {
            Debug.LogWarning("Slider or Fill is null in TimeBar script.");
        }
    }
}
