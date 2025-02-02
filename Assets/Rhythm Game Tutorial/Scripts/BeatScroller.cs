using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BeatScroller : MonoBehaviour
{
    public float anchorYStart;
    public float anchorYEnd;

    public float totalTime;

    RectTransform rt;
    Image image;
    float t = 0;

    Coroutine destroyCoroutine = null;

    void Start()
    {
        LeanTween.init(800);
        rt = GetComponent<RectTransform>();

        // This is to prevent the note from briefly popping up before
        // its position can be set
        image = GetComponent<Image>();
        image.enabled = false;
    }

    void Update()
    {
        if(!image.enabled) image.enabled = true;
        t += Time.deltaTime;
        float anchorY = Mathf.Lerp(anchorYStart, anchorYEnd, t / totalTime);
        rt.anchorMin = new Vector3(rt.anchorMin.x, anchorY);
        rt.anchorMax = new Vector3(rt.anchorMax.x, anchorY);
        
        if(t >= totalTime && totalTime > 0 && destroyCoroutine == null)
        {
            destroyCoroutine = StartCoroutine(Destroy());
        }

    }
    IEnumerator Destroy()
    {
        LeanTween.cancel(gameObject);

        RectTransform rt = gameObject.GetComponent<RectTransform>();
        LeanTween.alpha(rt, 0f, 0.1f);
        LeanTween.scale(rt, Vector3.zero, 0.3f);

        yield return new WaitForSeconds(0.1f);
        Destroy(gameObject);
    }
}
