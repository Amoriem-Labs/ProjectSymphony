using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.IO;

public class W2D2A : MonoBehaviour
{
    private DialougeManager dialogueManager;
    private bool isStarted = false;
    bool startChoiceDetection = false;
    bool Jump2Game =  false;
    bool isGameComplete =  false;
    // make one bool for each decision point
    private bool DP1 = false;
    private bool DP2 = false;
    private bool DP3 = false;

    // detect when to switch C# scripts
    bool END = false;

    // Start is called before the first frame update
    void Start()
    {
        dialogueManager = FindAnyObjectByType<DialougeManager>();

    }

    // Update is called once per frame
    void Update()
    {
        // change isW2D2 to whatever your week and day are 
        if (dialogueManager.isW2D2A && !isStarted)
        {
            Debug.Log("started WTF WTFWTF");
            // start first texx file
            StartCoroutine(dialogueManager.LoadAndStartDialoguesSequentially(new string[] { "W2D2.0" ,"W2D2.0A" }));
            // is started triggers the choice tests
            isStarted = true;
            isGameComplete = true;
            // add logic for jumpinhg **************** TO DO ***************
            //make data structure that stores name of song and name of last script
        }

        if (isGameComplete)
        {
            //StartCoroutine(dialogueManager.LoadAndStartDialoguesSequentially(new string[] { "W2D2.0A" }));
            startChoiceDetection = true; 
            DP1 = true;
        }
        if (startChoiceDetection )
        {

            // decision point 1
            if (!string.IsNullOrEmpty(dialogueManager.selectedOption) && DP1)
            {
                string currselectedOption = dialogueManager.selectedOption;

                // restart manager's selected option
                DP1 = false;
                dialogueManager.selectedOption = "";

                // change this to the options that are in your file [up to 4]
                if (currselectedOption == "So... when's it going to be done?")
                {
                    // load the next dialogue
                    StartCoroutine(dialogueManager.LoadAndStartDialoguesSequentially(new string[] { "W2D2.1"}));
                    DP2 = true;

                }
                else if (currselectedOption == "I especially liked the first pre-chorus, it was really cool!")
                {
                    // load the next dialogue
                    StartCoroutine(dialogueManager.LoadAndStartDialoguesSequentially(new string[] { "W2D2.1" }));
                    DP2 = true;
                }


            }


            // decision point 2
            if (!string.IsNullOrEmpty(dialogueManager.selectedOption) && DP2)
            {
                string currselectedOption = dialogueManager.selectedOption;

                // restart manager's selected option
                DP2 = false;
                dialogueManager.selectedOption = "";

                // change this to the options that are in your file [up to 4]
                if (currselectedOption == "It'll be fun, Howard.")
                {
                    // load the next dialogue
                    StartCoroutine(dialogueManager.LoadAndStartDialoguesSequentially(new string[] { "W2D2.1A" }));
                    END = true;

                }
                else if (currselectedOption == "Okay... so, are we going?")
                {
                    // load the next dialogue
                    StartCoroutine(dialogueManager.LoadAndStartDialoguesSequentially(new string[] { "W2D2.1A" }));
                    END = true;
                }

                if (END)
                {
                    if (dialogueManager.activeDialogue == false)
                    {
                        END = false;
                        // change to map screen
                    }
                }
            }

        }
    }
}
