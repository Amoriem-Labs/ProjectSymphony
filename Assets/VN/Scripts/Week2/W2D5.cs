using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class W2D5 : MonoBehaviour
{
    private DialougeManager dialogueManager;
    private bool isStarted = false;
    //bool startChoiceDetection = false;
    bool END = false;
    // Start is called before the first frame update
    void Start()
    {
        dialogueManager = FindAnyObjectByType<DialougeManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (dialogueManager.isW2D5)
        {
            if (!dialogueManager.isW2D3 && !isStarted)
            {
                StartCoroutine(dialogueManager.LoadAndStartDialoguesSequentially(new string[] { "W2D5" }));
                isStarted = true;
                END = true;

            }

            if (END && !dialogueManager.activeDialogue)
            {

                END = false;
                dialogueManager.isW2D5 = true;
                dialogueManager.UpdatePPref(11);
             
            }
        }
    }
}
