using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public GameObject location;
    public GameObject time;

    [System.Serializable]
    public class UIImage
    {
        public string name; // Identifier for the UI element
        public Sprite sprite; // image
         // The UI GameObject
    }
    public UIImage[] uiLocations; // Array of UI Images

    public UIImage[] uiTimes;  // Array of UI Backgrounds
    public void ShowLocation(string name) // PLACEHOLDER: come back to add variability for showing other elements
    {
        Debug.Log("show location called, calling" + name);
        foreach (var uiImage in uiLocations)
        {
            Debug.Log("image name is "+ uiImage.name);
            if (uiImage.name == name)
            {
                location.GetComponent<RectTransform>().localScale = Vector3.zero;
                // Animate and show the element
                LeanTween.scale(location.GetComponent<RectTransform>(), Vector3.one, 1.5f) // Increase duration
                .setEaseOutElastic() // Experiment with different easing functions
                .setDelay(0.1f); // Optional: add a slight delay before the animation starts for more emphasis

                // Change the sprite
                var imageComponent = location.GetComponent<Image>();
                if (imageComponent != null)
                {
                    imageComponent.sprite = uiImage.sprite;
                }
                else
                {
                    Debug.LogWarning($"No Image component found on");
                }
                return;
            }
            
        }
    }  

    public void ShowTime(string name) // PLACEHOLDER: come back to add variability for showing other elements
    {
        foreach (var uiImage in uiTimes)
        {
            if (uiImage.name == name)
            {
                time.GetComponent<RectTransform>().localScale = Vector3.zero;
                // Animate and show the element
                LeanTween.scale(time.GetComponent<RectTransform>(), Vector3.one, 1.5f) // Increase duration
                .setEaseOutElastic() // Experiment with different easing functions
                .setDelay(0.1f); // Optional: add a slight delay before the animation starts for more emphasis

                // Change the sprite
                var imageComponent = time.GetComponent<Image>();
                if (imageComponent != null)
                {
                    imageComponent.sprite = uiImage.sprite;
                }
                else
                {
                    Debug.LogWarning($"No Image component found on");
                }
                return;
            }
             
        }
    }  

}

