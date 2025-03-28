using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.IO;

public class W2D1B : MonoBehaviour
{

    private DialougeManager dialogueManager;
    private bool isStarted = false;
    bool startChoiceDetection = false;

    // make one bool for each decision point
    private bool DP1 = true;
    private bool DP2 = false;
    private bool DP3 = false;

    bool END = false;

    // Start is called before the first frame update
    void Start()
    {
        dialogueManager = FindAnyObjectByType<DialougeManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (dialogueManager.isW2D1B && !isStarted)
        {
            // start first texx file
            StartCoroutine(dialogueManager.LoadAndStartDialoguesSequentially(new string[] { "W2D1.1" }));
            // is started triggers the choice tests
            isStarted = true; // MAKE SURE THIS IS AFTER THE FIRST LOAD 
            startChoiceDetection = true;
        }


        if (startChoiceDetection)
        {

            // decision point 1
            if (!string.IsNullOrEmpty(dialogueManager.selectedOption) && DP1)
            {
                string currselectedOption = dialogueManager.selectedOption;

                // restart manager's selected option
                DP1 = false;
                dialogueManager.selectedOption = "";

                // change this to the options that are in your file [up to 4]
                if (currselectedOption == "Let Howard take Ina's leash off.")
                {
                    // load the next dialogue
                    StartCoroutine(dialogueManager.LoadAndStartDialoguesSequentially(new string[] { "W2D1.1a", "W1D1.2" }));
                    DP2 = true;

                }
                else if (currselectedOption == "Do not Let Howard take Ina's leash off.")
                {
                    DP2 = true;
                    // load the next dialogue
                    StartCoroutine(dialogueManager.LoadAndStartDialoguesSequentially(new string[] { "W2D1.1b", "W1D1.2" }));
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
                if (currselectedOption == "Go to the Park.")
                {
                    // load the next dialogue
                    StartCoroutine(dialogueManager.LoadAndStartDialoguesSequentially(new string[] { "W2D1.2a", "W1D1.3" }));
                    DP3 = true;

                }
                else if (currselectedOption == "Go to the Hallway.")
                {
                    DP3 = true;
                    // load the next dialogue
                    StartCoroutine(dialogueManager.LoadAndStartDialoguesSequentially(new string[] { "W2D1.2b", "W1D1.3" }));
                }


            }

            if (!string.IsNullOrEmpty(dialogueManager.selectedOption) && DP3)
            {
                string currselectedOption = dialogueManager.selectedOption;

                // restart manager's selected option
                DP3 = false;
                dialogueManager.selectedOption = "";

                // change this to the options that are in your file [up to 4]
                if (currselectedOption == "Catch the Ina by surprise.")
                {
                    // load the next dialogue
                    StartCoroutine(dialogueManager.LoadAndStartDialoguesSequentially(new string[] { "W2D1.2a", "W1D1.3" }));
                    DP3 = true;

                }
                else if (currselectedOption == "Go to the Hallway.")
                {
                    DP3 = true;
                    // load the next dialogue
                    StartCoroutine(dialogueManager.LoadAndStartDialoguesSequentially(new string[] { "W2D1.2b", "W1D1.3" }));
                }


            }

            if (END)
            {
                // go back to map 
            }


        }
    }
}
