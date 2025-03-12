using System.Collections;
using System.Collections.Generic;
using Microsoft.Unity.VisualStudio.Editor;
using Unity.VisualScripting.ReorderableList.Element_Adder_Menu;
using UnityEngine;
using TMPro;
using UnityEditor.PackageManager.UI;

public class CharacterDisplayUI : MonoBehaviour
{
    public RectTransform[] images;

    public GameObject[] spotlights;

    public string[] names = new string[4]; // Placeholder 
    public float animationTime;
    public Vector2 smallScale;
    public Vector2 largeScale;

    Coroutine coroutine;

    void Start()
    {
        StartCoroutine(Animate(0));

        for (int i = 0; i < images.Length; i++)
        {
            Transform childTransform = spotlights[i].transform.GetChild(0); // Index starts at 0
            GameObject childGameObject = childTransform.gameObject;
            TMP_Text characterText = childGameObject.GetComponent<TMP_Text>();
            characterText.text = names[i];

        }
    }

    public void SwitchToCharacter(CharacterRole character)
    {
        if(coroutine != null) StopCoroutine(coroutine);
        if(character == CharacterRole.Melodist) coroutine = StartCoroutine(Animate(0));
        else if(character == CharacterRole.Counter) coroutine = StartCoroutine(Animate(1));
        else if(character == CharacterRole.Harmony) coroutine = StartCoroutine(Animate(2));
        else if(character == CharacterRole.Percussion) coroutine = StartCoroutine(Animate(3));
    }

    IEnumerator Animate(int featured)
{
    float t = 0;

    // Start LeanTween animations once for all elements
    for (int i = 0; i < images.Length; i++)
    {
        if (i == featured)
        {
            // Animate the spotlight to scale up
            spotlights[i].SetActive(true);
            spotlights[i].GetComponent<RectTransform>().localScale = Vector3.zero;
            LeanTween.scale(spotlights[i].GetComponent<RectTransform>(), Vector3.one, 1.5f)
                .setEaseOutElastic()
                .setDelay(0.1f);
        }
        else
        {
            // Animate the spotlight to scale down
            LeanTween.scale(spotlights[i].GetComponent<RectTransform>(), Vector3.zero, 1.5f)
                .setEaseOutElastic()
                .setDelay(0.1f);
                //.setOnComplete(() => spotlights[i].SetActive(false));
        }
    }

    // Handle smooth scaling for images over time
    while (t < animationTime)
    {
        t += Time.deltaTime;

        for (int i = 0; i < images.Length; i++)
        {
            if (i == featured)
            {
                images[i].sizeDelta = Vector2.Lerp(smallScale, largeScale, t / animationTime);
            }
            else
            {
                images[i].sizeDelta = Vector2.Lerp(largeScale, smallScale, t / animationTime);
            }
        }

        yield return new WaitForEndOfFrame();
    }
}

}
