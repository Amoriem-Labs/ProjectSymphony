using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class CharacterDisplayUI : MonoBehaviour
{
    public RectTransform[] images;

    public GameObject[] spotlights;

    public float animationTime;
    public Vector2 smallScale;
    public Vector2 largeScale;
    
    string[] names = new string[4]; 

    Coroutine coroutine;

    void Start()
    {
        StartCoroutine(Animate(0));

        for (int i = 0; i < images.Length; i++)
        {
            CharacterRole role = (CharacterRole)(i + 1);
            if(!GameStateManager.Instance.SelectedCharactersContainsRole(role))
            {
                images[i].gameObject.SetActive(false);
                continue;
            }
            Image imgScript = images[i].GetComponent<Image>();
            Character c = GameStateManager.Instance.GetSelectedCharacterWithRole(role).character;
            imgScript.sprite = c.spriteUnlocked;
            names[i] = c.name;

            Transform childTransform = spotlights[i].transform.GetChild(0); // Index starts at 0
            GameObject childGameObject = childTransform.gameObject;
            TMP_Text characterText = childGameObject.GetComponent<TMP_Text>();
            characterText.text = names[i];

        }
    }

    public void SwitchToCharacter(CharacterRole character)
    {
        if(coroutine != null) StopCoroutine(coroutine);
        if((int)character < 0 || (int)character > 4) return;
        if(!GameStateManager.Instance.SelectedCharactersContainsRole(character)) return;
        StartCoroutine(Animate((int)character - 1));
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
