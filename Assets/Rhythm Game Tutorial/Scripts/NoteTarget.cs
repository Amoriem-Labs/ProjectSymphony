using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class NoteTarget : MonoBehaviour
{
    public KeyCode keyCode;
    public float pressedScaleAmount;
    Vector3 initialScale;
    
    void Start()
    {
        initialScale = transform.localScale;
    }

    void Update()
    {
        transform.localScale = initialScale * (Input.GetKey(keyCode) ? pressedScaleAmount : 1);
    }
}
