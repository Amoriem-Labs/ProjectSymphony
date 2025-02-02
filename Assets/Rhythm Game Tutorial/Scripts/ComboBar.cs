using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ComboBar : MonoBehaviour
{
    public Slider slider;
    private Coroutine smoothTransitionCoroutine;

    void Start()
    {
        slider.value = 0;
    }

    // Sets the slider value with a smooth transition
    public void SetScore(int score)
    {

        LeanTween.value(slider.gameObject, slider.value, score, 0.5f)
            .setEase(LeanTweenType.easeInOutCubic) // Cubic easing
            .setOnUpdate((float value) =>
            {
                slider.value = value; // Update the slider value during the transition
            });
    }


}
