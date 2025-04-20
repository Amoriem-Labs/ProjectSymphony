using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ComboBar : MonoBehaviour
{
    public Slider slider;
    private Coroutine smoothTransitionCoroutine;

    public Image barFill;
    public Text comboText;

    void Start()
    {
        LeanTween.color(barFill.rectTransform, Color.white, 1f)
                    .setEase(LeanTweenType.easeInOutCubic);
        slider.value = 0;
    }

    // Sets the slider value with a smooth transition
    public void SetScore(int score)
    {

        if (comboText != null) comboText.text = score.ToString();
        LeanTween.value(slider.gameObject, slider.value, score, 0.5f)
            .setEase(LeanTweenType.easeInOutCubic) // Cubic easing
            .setOnUpdate((float value) =>
            {
                slider.value = value; // Update the slider value during the transition
            });


    }
    public void ChangeColorBlue()
    {
        LeanTween.color(barFill.rectTransform, Color.blue, 1.5f)
            .setEase(LeanTweenType.easeInOutCubic);
    }
    public void ChanceColorYellow()
    {
        LeanTween.color(barFill.rectTransform, Color.yellow, 1.5f)
                   .setEase(LeanTweenType.easeInOutCubic);
    }


}
