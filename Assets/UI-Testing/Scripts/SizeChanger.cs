using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SizeChanger : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        transform.localScale = Vector2.zero;
    }

    // Update is called once per frame
    public void Open()
    {
        transform.LeanScale(Vector2.one, 0.8f); // will smooth an enlargening effect
    }
    public void Close()
    {
        transform.LeanScale(Vector2.zero, 0.1f).setEaseInBack();
    }
}

