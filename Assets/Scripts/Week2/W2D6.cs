using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class W2D6 : MonoBehaviour
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
        if(dialogueManager.isW2D6)
            if (dialogueManager.isW2D6 && !isStarted)
            {
                StartCoroutine(dialogueManager.LoadAndStartDialoguesSequentially(new string[] { "W2D6" }));
                isStarted = true;
                END = true;

            }

            if (END && !dialogueManager.activeDialogue)
            {

                // scene transition to map?
            }
    }
}
