using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationManager : MonoBehaviour
{

    public bool playAnimation;
    public string animationName;
    public GameObject background;
    // Shake variables
    float Shakeduration = 1f;
    public AnimationCurve Shakecurve;
    List<string> AnimationDictionary;


    // Start is called before the first frame update
    void Start()
    {
        // dictionary with all the animation names
        AnimationDictionary = new List<string> {"ScreenShake"};
    
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
                Debug.Log("playing animation2" +  animationName);
                if (animationName == "ScreenShake")
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

        transform.position = startPosition;
    }

}
