using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GhostInputHint : MonoBehaviour
{
    public KeyCode key;
    public float animationTime;

    float t;
    Color startingColor;
    Image image;

    void Start()
    {
        image = GetComponent<Image>();
        startingColor = image.color;
        image.color = Color.clear;
    }

    void Update()
    {
        image.color = Color.Lerp(Color.clear, startingColor, t / animationTime);
        t -= Time.deltaTime;

        if(Input.GetKeyDown(key))
        {
            t = animationTime;
        }
    }
}
