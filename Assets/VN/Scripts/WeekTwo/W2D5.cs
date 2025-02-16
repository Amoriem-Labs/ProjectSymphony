using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.IO;

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
        if (!dialogueManager.isW2D3 && !isStarted)
        {
            StartCoroutine(dialogueManager.LoadAndStartDialoguesSequentially(new string[] { "W2D5" }));
            isStarted = true;
            END = true;
            
        }

        if (END)
        {
            if (dialogueManager.activeDialogue == false)
            {
                END = false;
                dialogueManager.isW1D2 = true;
            }


        }
    }
}
/*

public class W2D2 : MonoBehaviour
{
    private DialougeManager dialogueManager;
    private bool isStarted = false;
    bool startChoiceDetection = false;

    // make one bool for each decision point
    private bool DP1 = true;
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
        if (dialogueManager.isW2D2 && !isStarted)
        {
            // start first texx file
            StartCoroutine(dialogueManager.LoadAndStartDialoguesSequentially(new string[] { "W1D1.0" }));
            // is started triggers the choice tests
            isStarted = true;
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
                if (currselectedOption == "Option One.")
                {
                    // load the next dialogue
                    StartCoroutine(dialogueManager.LoadAndStartDialoguesSequentially(new string[] { "W1D1.0a", "W1D1.1" }));
                    DP2 = true;

                }
                else if (currselectedOption == "Option Two.")
                {
                    DP3 = true;
                    // load the next dialogue
                    StartCoroutine(dialogueManager.LoadAndStartDialoguesSequentially(new string[] { "W1D1.1" }));
                }


            }

        }
    }
}

 
 */