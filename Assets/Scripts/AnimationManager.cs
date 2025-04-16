using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnimationManager : MonoBehaviour
{

    public bool playAnimation;
    public string animationName;
    public GameObject background;
    // Shake variables
    float Shakeduration = 1f;
    public AnimationCurve Shakecurve;
    List<string> AnimationDictionary;

    public Image whiteScreen;

    CanvasGroup screenGroup;


    // Start is called before the first frame update
    void Start()
    {

        // dictionary with all the animation names
        AnimationDictionary = new List<string> { "ScreenShake", "ScreenFlash" };

        screenGroup = whiteScreen.GetComponent<CanvasGroup>();
        screenGroup.alpha = 0;

    }

    // Update is called once per frame
    void Update()
    {
        if (playAnimation)
        {

            playAnimation = false;
            Debug.Log("playing animation");
            if (AnimationDictionary.Contains(animationName))
            {
                Debug.Log("playing animation2" + animationName);
                if (animationName == "ScreenShake")
                {
                    StartCoroutine(animationName);
                }
                else if (animationName == "ScreenFlash")
                {
                    StartCoroutine(animationName);
                }
            }
            else
            {
                Debug.LogError($"Animation of name {animationName} does not exist");
                return;
            }

        }

    }

    IEnumerator ScreenShake()
    {
        Vector2 startPosition = background.transform.position;
        float elapsedTime = 0f;

        while (elapsedTime < Shakeduration)
        {
            elapsedTime += Time.deltaTime;
            float strength = Shakecurve.Evaluate(elapsedTime / Shakeduration);
            background.transform.position = startPosition + Random.insideUnitCircle * 2;
            yield return null;
        }

        background.transform.position = new Vector2(0, 0);
    }
    IEnumerator ScreenFlash()
    {
        Debug.Log("playing screen flash");

        // Set the alpha to fully visible (1)
        screenGroup.alpha = 1f;

        // Fade out over flashDuration
        LeanTween.alphaCanvas(screenGroup, 0f, 0.2f).setEase(LeanTweenType.easeOutQuad);

        yield return new WaitForSeconds(0.2f);

    }

}

