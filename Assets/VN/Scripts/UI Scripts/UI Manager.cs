using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public GameObject element;
    public GameObject background;
    [System.Serializable]
    public class UIImage
    {
        public string name; // Identifier for the UI element
        public Sprite sprite; // image
         // The UI GameObject
    }

    public class UIBackground
    {
        public string name; // Identifier for the UI element
        public Sprite sprite; // image
         // The UI GameObject
    }

    public UIImage[] uiImages; // Array of UI Images

    public UIImage[] uiBackgrounds;  // Array of UI Backgrounds
    public void ShowLocation(string name) // PLACEHOLDER: come back to add variability for showing other elements
    {
        foreach (var uiImage in uiImages)
        {
            if (uiImage.name == name)
            {
                element.GetComponent<RectTransform>().localScale = Vector3.zero;
                // Animate and show the element
                LeanTween.scale(element.GetComponent<RectTransform>(), Vector3.one, 1.5f) // Increase duration
                .setEaseOutElastic() // Experiment with different easing functions
                .setDelay(0.1f); // Optional: add a slight delay before the animation starts for more emphasis

                // Change the sprite
                var imageComponent = element.GetComponent<Image>();
                if (imageComponent != null)
                {
                    imageComponent.sprite = uiImage.sprite;
                }
                else
                {
                    Debug.LogWarning($"No Image component found on");
                }
            }
            else
            {
                Debug.LogWarning($"Invalid sprite index");
            }
             return; // Exit the method after finding and processing the correct element
        }
    }  

    public void ShowBackground(string name)
    {
        foreach (var uiBackground in uiBackgrounds)
        {
            if (uiBackground.name == name)
            {
                // no animation
                var imageComponent = background.GetComponent<Image>();
                if (imageComponent != null)
                {
                    imageComponent.sprite = uiBackground.sprite;
                }
                else
                {
                    Debug.LogWarning($"No Image component found on");
                }
            }
            else
            {
                Debug.LogWarning($"Invalid sprite index");
            }
             return; // Exit the method after finding and processing the correct element
        }
    }  
}

