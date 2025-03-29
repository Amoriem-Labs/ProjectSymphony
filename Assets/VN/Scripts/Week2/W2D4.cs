using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class W2D4 : MonoBehaviour
{
    private DialougeManager dialogueManager;
    private bool isStarted = false;
    bool startChoiceDetection = false;

    // make one bool for each decision point
    private bool DP1 = true;
    private bool DP2 = false;
    bool END = false;

    // Start is called before the first frame update
    void Start()
    {
        dialogueManager = FindAnyObjectByType<DialougeManager>();

    }

    // Update is called once per frame
    void Update()
    {
        if (dialogueManager.isW2D4)
        {
            if (dialogueManager.isW2D4 && !isStarted)
            {
                // start first texx file
                StartCoroutine(dialogueManager.LoadAndStartDialoguesSequentially(new string[] { "W2D4.0" }));
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
                    if (currselectedOption == "Something salty")
                    {
                        // load the next dialogue
                        StartCoroutine(dialogueManager.LoadAndStartDialoguesSequentially(new string[] { "W2D4.1A", "W2D4.2" }));
                        DP2 = true;

                    }
                    else if (currselectedOption == "Something sweet")
                    {
                        DP2 = true;
                        // load the next dialogue
                        StartCoroutine(dialogueManager.LoadAndStartDialoguesSequentially(new string[] { "W2D4.1B", "W2D4.2" }));
                    }
                    else if (currselectedOption == "Something spicy")
                    {
                        DP2 = true;
                        // load the next dialogue
                        StartCoroutine(dialogueManager.LoadAndStartDialoguesSequentially(new string[] { "W2D4.1C", "W2D4.2" }));
                    }



                }
                if (!string.IsNullOrEmpty(dialogueManager.selectedOption) && DP2 == true)
                {
                    string currselectedOption = dialogueManager.selectedOption;

                    // restart manager's selected option
                    DP2 = false;
                    dialogueManager.selectedOption = "";

                    // change this to the options that are in your file [up to 4]
                    if (currselectedOption == "They do sound hard to beat.")
                    {
                        // load the next dialogue
                        StartCoroutine(dialogueManager.LoadAndStartDialoguesSequentially(new string[] { "W2D4.2A", "W2D4.3" }));
                        END = true;

                    }
                    else if (currselectedOption == "They're probably just being over-confident")
                    {
                        END = true;
                        // load the next dialogue
                        StartCoroutine(dialogueManager.LoadAndStartDialoguesSequentially(new string[] { "W2D4.2B", "W2D4.3" }));
                    }

                }


                if (END)
                {
                    if (dialogueManager.activeDialogue == false)
                    {
                        END = false;
                        dialogueManager.isW2D5 = true;
                        dialogueManager.UpdatePPref(10);
                        // dialogueManager.isW3D4 = true;
                        // dialogueManager.UpdatePPref(14);

                    }


                }


            }
        }
    }

}
