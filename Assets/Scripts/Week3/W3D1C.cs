using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class W3D1C : MonoBehaviour
{

    private DialougeManager dialogueManager;
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

    }

    // Update is called once per frame
    void Update()
    {
        if (dialogueManager.isW3D1C)
        {
            if (dialogueManager.isW3D1C && !isStarted)
            {
                // start first texx file
                StartCoroutine(dialogueManager.LoadAndStartDialoguesSequentially(new string[] { "W3D1C.0" }));
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
                    if (currselectedOption == "Thanks for the help.")
                    {
                        // load the next dialogue
                        StartCoroutine(dialogueManager.LoadAndStartDialoguesSequentially(new string[] { "W3D1C.0A", "W3D1C.1" }));
                        DP2 = true;

                    }
                    else if (currselectedOption == "Ugh, what was that?")
                    {
                        DP2 = true;
                        // load the next dialogue
                        StartCoroutine(dialogueManager.LoadAndStartDialoguesSequentially(new string[] { "W3D1C.0B", "W3D1C.1" }));
                    }
                    else if (currselectedOption == "Who are you?")
                    {
                        DP2 = true;
                        // load the next dialogue
                        StartCoroutine(dialogueManager.LoadAndStartDialoguesSequentially(new string[] { "W3D1C.1" }));
                    }
                }

                if (!string.IsNullOrEmpty(dialogueManager.selectedOption) && DP2)
                {
                    string currselectedOption = dialogueManager.selectedOption;

                    // restart manager's selected option
                    DP2 = false;
                    dialogueManager.selectedOption = "";

                    // change this to the options that are in your file [up to 4]
                    if (currselectedOption == "Leave.")
                    {
                        // load the next dialogue
                        StartCoroutine(dialogueManager.LoadAndStartDialoguesSequentially(new string[] { "W3D1C.1A" }));
                        END = true;

                    }
                    else if (currselectedOption == "Study with Carter.")
                    {
                        DP4= true;
                        // load the next dialogue
                        StartCoroutine(dialogueManager.LoadAndStartDialoguesSequentially(new string[] { "W3D1C.2" }));
                    }

                }


            }

            if (!string.IsNullOrEmpty(dialogueManager.selectedOption) && DP4)
            {
                string currselectedOption = dialogueManager.selectedOption;

                // restart manager's selected option
                dialogueManager.selectedOption = "";

                if (currselectedOption == "Wave back." || currselectedOption == "Ignore him.")
                {

                    // restart manager's selected option
                    DP4 = false;
                    dialogueManager.selectedOption = "";

                    startChoiceDetection = false;
                    END = true;

                }

            }

            if (END)
            {
                if (dialogueManager.activeDialogue == false)
                {
                    END = false;
                    // add scene management stuff
                    //update map week num
                    dialogueManager.isW3D2C = true;
                    dialogueManager.UpdatePPref(17);
                    END = false;
                }


            }

        }
    }
}
