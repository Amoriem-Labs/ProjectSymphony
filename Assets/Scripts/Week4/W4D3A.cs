using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class W4D3A : MonoBehaviour
{
    private DialougeManager dialogueManager;
    private AffectionManager affectionManager;
    private bool isStarted = false;
    bool startChoiceDetection = false;

    private bool DP1 = true;
    private bool DP2 = false;
    private bool DP3 = false;
    private bool DP4 = false;

    bool END = false;

    // Start is called before the first frame update
    void Start()
    {
        dialogueManager = FindAnyObjectByType<DialougeManager>();
        affectionManager = FindAnyObjectByType<AffectionManager>();
    }

    void Update()
    {
        if (dialogueManager.isW4D3A)
        {
            if (dialogueManager.isW4D3A && !isStarted)
            {
                // start first texx file
                StartCoroutine(dialogueManager.LoadAndStartDialoguesSequentially(new string[] { "W4D3A.0" }));
                // is started triggers the choice tests
                isStarted = true; // MAKE SURE THIS IS AFTER THE FIRST LOAD 
                startChoiceDetection = true;
            }

            if (startChoiceDetection)
            {
                if (!string.IsNullOrEmpty(dialogueManager.selectedOption) && DP1)
                {
                    string currselectedOption = dialogueManager.selectedOption;

                    // restart manager's selected option
                    DP1 = false;
                    dialogueManager.selectedOption = "";

                    // change this to the options that are in your file [up to 4]
                    if (currselectedOption == "Call them out.")
                    {
                        affectionManager.UpdateCharacterAffection("Daylo", -1);
                        // load the next dialogue
                        StartCoroutine(dialogueManager.LoadAndStartDialoguesSequentially(new string[] { "W3D1A.0A", "W3D1A.1" }));
                        DP2 = true;

                    }
                    else if (currselectedOption == "Approach them.")
                    {
                        DP2 = true;
                        // load the next dialogue
                        StartCoroutine(dialogueManager.LoadAndStartDialoguesSequentially(new string[] { "W3D1A.0B", "W3D1A.1" }));
                    }

                }
            }

        }

    }

}