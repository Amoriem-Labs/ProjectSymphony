using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public GameObject location;
    public GameObject time;

    // Readiness flag to indicate that the UI Manager is fully initialized.
    public bool IsReady { get; private set; } = false;

    [System.Serializable]
    public class UIImage
    {
        public string name; // Identifier for the UI element
        public Sprite sprite; // image
    }
    public UIImage[] uiLocations; // Array of UI Images
    public UIImage[] uiTimes;  // Array of UI Backgrounds

    private string currentTimeName = "";

    // Use a coroutine in Start to delay setting IsReady until the end of the first frame,
    // ensuring that all components are initialized.
    private IEnumerator Start()
    {
        // Wait for the end of the frame, which can help ensure that other components have finished initialization.
        yield return new WaitForEndOfFrame();
        IsReady = true;
    }

    public void ShowLocation(string name) // PLACEHOLDER: come back to add variability for showing other elements
    {
        //Debug.Log("ShowLocation called, looking for: " + name);
        foreach (var uiImage in uiLocations)
        {
            //Debug.Log("Image name is " + uiImage.name);
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
                    Debug.LogWarning("No Image component found on location.");
                }
                return;
            }
        }
    }

    public void ShowTime(string name) // PLACEHOLDER: come back to add variability for showing other elements
    {

        if (currentTimeName == name)
        {
            return;
        }
        foreach (var uiImage in uiTimes)
        {
            if (uiImage.name == name)
            {

                currentTimeName = name; // Update current time
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
                    Debug.LogWarning("No Image component found on time.");
                }
                return;
            }
        }
    }
}
