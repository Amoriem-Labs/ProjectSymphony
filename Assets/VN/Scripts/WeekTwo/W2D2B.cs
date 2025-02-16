using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.IO;
public class W2D2B : MonoBehaviour
{
    private DialougeManager dialogueManager;
    private bool isStarted = false;
    private bool DP1 = false;
    public bool DP2 = false;
    private bool END = false;

    // Start is called before the first frame update
    void Start()
    {
        dialogueManager = FindAnyObjectByType<DialougeManager>();

    }

    // Update is called once per frame
    void Update()
    {
        if (dialogueManager.isW2D2A && !isStarted)
        {
            StartCoroutine(dialogueManager.LoadAndStartDialoguesSequentially(new string[] { "W2D2.0", "W2D2.0A" }));
            isStarted = true; //  make sure isStarted is AFTER the first dialogye load 
            DP1 = true; // Start at Decision Point 1 after Initial Dialogue
        }

    }
}
