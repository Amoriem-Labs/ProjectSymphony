using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.IO;

public class W2D2a1 : MonoBehaviour
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
        if (dialogueManager.isW2D2A)
        {
            // Initial Dialogue
            if (dialogueManager.isW2D2A && !isStarted)
            {
                StartCoroutine(dialogueManager.LoadAndStartDialoguesSequentially(new string[] { "W2D2.0", "W2D2.0A" }));
                isStarted = true; //  make sure isStarted is AFTER the first dialogye load 
                DP1 = true; // Start at Decision Point 1 after Initial Dialogue
            }


            // Decision Point 1

            if (!string.IsNullOrEmpty(dialogueManager.selectedOption) && DP1)
            {
                DP1 = false;  // Prevents re-entry
                string currselectedOption = dialogueManager.selectedOption;
                dialogueManager.selectedOption = "";  // Clear IMMEDIATELY

                if (currselectedOption == "So... when's it going to be done?" || currselectedOption == "I especially liked the first pre-chorus, it was really cool!")
                {
                    DP2 = true;
                    StartCoroutine(dialogueManager.LoadAndStartDialoguesSequentially(new string[] { "W2D2.1" }));
                }
            }

            // Decision Point 2
            if (!string.IsNullOrEmpty(dialogueManager.selectedOption) && DP2)
            {
                DP2 = false;  // Prevents re-entry
                string currselectedOption = dialogueManager.selectedOption;
                dialogueManager.selectedOption = "";  // Clear IMMEDIATELY

                if (currselectedOption == "It'll be fun, Howard." || currselectedOption == "Okay... so, are we going?")
                {
                    END = true;
                    StartCoroutine(dialogueManager.LoadAndStartDialoguesSequentially(new string[] { "W2D2.1A" }));
                }
            }

            // End Condition - Wait for *activeDialogue* to be false
            if (END && !dialogueManager.activeDialogue)
            {
                dialogueManager.isW2D3 = true;
                dialogueManager.UpdatePPref(8);

                END = false;
            }
        }
    }

}
