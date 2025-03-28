using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.IO;

public class W2D1A : MonoBehaviour
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
        if (dialogueManager.isW2D1A && !isStarted)
        {
            // start first texx file
            StartCoroutine(dialogueManager.LoadAndStartDialoguesSequentially(new string[] { "W2D1.2.1" }));
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
                if (currselectedOption == "Accept the assignment.")
                {
                    // load the next dialogue
                    StartCoroutine(dialogueManager.LoadAndStartDialoguesSequentially(new string[] { "W2D1.2.1a", "W1D1.2.2" }));
                    DP2 = true;

                }
                else if (currselectedOption == "Question Sam's sanity.")
                {
                    DP3 = true;
                    // load the next dialogue
                    StartCoroutine(dialogueManager.LoadAndStartDialoguesSequentially(new string[] { "W2D1.2.1b", "W1D1.2.2" }));
                }


            }


        }

    }
}
