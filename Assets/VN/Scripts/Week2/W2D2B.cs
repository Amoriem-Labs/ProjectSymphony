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
    private bool END = false;

    // Start is called before the first frame update
    void Start()
    {
        dialogueManager = FindAnyObjectByType<DialougeManager>();

    }

    // Update is called once per frame
    void Update()
    {
        if (dialogueManager.isW2D2B && !isStarted)
        {
            StartCoroutine(dialogueManager.LoadAndStartDialoguesSequentially(new string[] { "W2D2B.0" }));
            isStarted = true; //  make sure isStarted is AFTER the first dialogye load 
            DP1 = true; // Start at Decision Point 1 after Initial Dialogue
        }
        if (!string.IsNullOrEmpty(dialogueManager.selectedOption) && DP1)
        {
            DP1 = false;  // Prevents re-entry
            string currselectedOption = dialogueManager.selectedOption;
            dialogueManager.selectedOption = "";  // Clear IMMEDIATELY

            if (currselectedOption == "They're going great!")
            {
                StartCoroutine(dialogueManager.LoadAndStartDialoguesSequentially(new string[] { "W2D2B.0A", "W2D2B.1" }));
                END = true;

            }

            if (currselectedOption == "They could be going better...")
            {
                StartCoroutine(dialogueManager.LoadAndStartDialoguesSequentially(new string[] { "W2D2B.0B", "W2D2B.1" }));
                END = true;

            }

        }

        if (END)
        {
            dialogueManager.isW2D3 = true;
            dialogueManager.UpdatePPref(8);

            END = false;
        }

    }
}
