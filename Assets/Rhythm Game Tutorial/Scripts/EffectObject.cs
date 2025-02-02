using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectObject : MonoBehaviour
{   
    public AnimationCurve scaleCurve;
    public float lifetime = 1.0f; 
    
    float t = 0;
    Vector3 startingSize;

    void Start()
    {
        startingSize = transform.localScale;
        transform.localScale = Vector3.zero;
    }
    
    void Update()
    {
        transform.localScale = startingSize * scaleCurve.Evaluate(t / lifetime);
        t += Time.deltaTime;
        if(t > lifetime) Destroy(gameObject);
    }
}
