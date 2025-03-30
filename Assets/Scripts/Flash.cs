using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Flash : MonoBehaviour
{
    Image image;
    Color startColor;
    public float time;

    float t;

    void Start()
    {
        image = GetComponent<Image>();
        startColor = image.color;
    }

    void Update()
    {
        t += Time.deltaTime;
        image.color = Color.Lerp(
            startColor,
            Color.clear,
            t > time ? (time * 2 - t) / time : t / time
        );
        if(t >= time * 2) t = 0;
    }
}
