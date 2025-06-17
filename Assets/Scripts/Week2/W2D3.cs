using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class W2D3 : MonoBehaviour
{
    private DialougeManager dialogueManager;
    private bool isStarted = false;

    // make one bool for each decision point
    private bool DP1 = true;
    private bool DP2 = false;
    private bool DP3 = false;
    private bool DP4 = false;
    private bool DP5 = false;
    private bool DP6 = false;
    private bool DP7 = false;

    bool END = false;

    // Start is called before the first frame update
    void Start()
    {
        dialogueManager = FindAnyObjectByType<DialougeManager>();

    }

    // Update is called once per frame
    void Update()
    {
        if (dialogueManager.isW2D3)
        {
            if (dialogueManager.isW2D3 && !isStarted)
            {
                Debug.Log("PPPPP");
                StartCoroutine(dialogueManager.LoadAndStartDialoguesSequentially(new string[] { "W2D3.0" }));
                // is started triggers the choice tests
                isStarted = true; // MAKE SURE THIS IS AFTER THE FIRST LOAD 
            }


            if (!string.IsNullOrEmpty(dialogueManager.selectedOption) && DP1)
            {
                string currselectedOption = dialogueManager.selectedOption;

                // restart manager's selected option
                DP1 = false;
                dialogueManager.selectedOption = "";

                // change this to the options that are in your file [up to 4]
                if (currselectedOption == "That's not true Sam - you're an amazing guitarist!!")
                {
                    // load the next dialogue
                    StartCoroutine(dialogueManager.LoadAndStartDialoguesSequentially(new string[] { "W2D3.1" }));
                    DP2 = true;

                }
                else if (currselectedOption == "Well then Roxanne doesn't deserve you.")
                {
                    DP2 = true;
                    // load the next dialogue
                    StartCoroutine(dialogueManager.LoadAndStartDialoguesSequentially(new string[] { "W2D3.1" }));
                }


            }

            if (!string.IsNullOrEmpty(dialogueManager.selectedOption) && DP2 == true)
            {
                string currselectedOption = dialogueManager.selectedOption;

                // restart manager's selected option
                DP2 = false;
                dialogueManager.selectedOption = "";

                // change this to the options that are in your file [up to 4]
                if (currselectedOption == "Who's that?")
                {
                    // load the next dialogue
                    StartCoroutine(dialogueManager.LoadAndStartDialoguesSequentially(new string[] { "W2D3.2" }));
                    DP3 = true;

                }
                else if (currselectedOption == "Oh, I think I recognize her from my audition.")
                {
                    DP3 = true;
                    // load the next dialogue
                    StartCoroutine(dialogueManager.LoadAndStartDialoguesSequentially(new string[] { "W2D3.2" }));
                }

            }
            if (!string.IsNullOrEmpty(dialogueManager.selectedOption) && DP3 == true)
            {
                string currselectedOption = dialogueManager.selectedOption;

                // restart manager's selected option
                DP3 = false;
                dialogueManager.selectedOption = "";

                // change this to the options that are in your file [up to 4]
                if (currselectedOption == "It's been... confusing.")
                {
                    // load the next dialogue
                    StartCoroutine(dialogueManager.LoadAndStartDialoguesSequentially(new string[] { "W2D3.2A", "W2D3.3" }));
                    DP4 = true;

                }
                else if (currselectedOption == "Honestly no, it's been great!")
                {
                    DP4 = true;
                    // load the next dialogue
                    StartCoroutine(dialogueManager.LoadAndStartDialoguesSequentially(new string[] { "W2D3.2B", "W2D3.3" }));
                }

            }

            if (!string.IsNullOrEmpty(dialogueManager.selectedOption) && DP4 == true)
            {
                string currselectedOption = dialogueManager.selectedOption;

                // restart manager's selected option
                DP4 = false;
                dialogueManager.selectedOption = "";

                // change this to the options that are in your file [up to 4]
                if (currselectedOption == "I have Howard and Sam!")
                {
                    // load the next dialogue
                    StartCoroutine(dialogueManager.LoadAndStartDialoguesSequentially(new string[] { "W2D3.4" }));
                    DP5 = true;

                }
                else if (currselectedOption == "No...")
                {
                    DP5 = true;
                    // load the next dialogue
                    StartCoroutine(dialogueManager.LoadAndStartDialoguesSequentially(new string[] { "W2D3.4" }));
                }

            }
            if (!string.IsNullOrEmpty(dialogueManager.selectedOption) && DP5 == true)
            {
                string currselectedOption = dialogueManager.selectedOption;

                // restart manager's selected option
                DP5 = false;
                dialogueManager.selectedOption = "";

                // change this to the options that are in your file [up to 4]
                if (currselectedOption == "A legacy?")
                {
                    // load the next dialogue
                    StartCoroutine(dialogueManager.LoadAndStartDialoguesSequentially(new string[] { "W2D3.4A", "W2D3.5" }));
                    DP6 = true;

                }
                else if (currselectedOption == "I promise.")
                {
                    DP6 = true;
                    // load the next dialogue
                    StartCoroutine(dialogueManager.LoadAndStartDialoguesSequentially(new string[] { "W2D3.5" }));
                }

            }
            if (!string.IsNullOrEmpty(dialogueManager.selectedOption) && DP6 == true)
            {
                string currselectedOption = dialogueManager.selectedOption;

                // restart manager's selected option
                DP6 = false;
                dialogueManager.selectedOption = "";

                // change this to the options that are in your file [up to 4]
                if (currselectedOption == "Did you wish to be with Roxanne?")
                {
                    // load the next dialogue
                    StartCoroutine(dialogueManager.LoadAndStartDialoguesSequentially(new string[] { "W2D3.5A", "W2D3.6" }));
                    DP7 = true;

                }
                else if (currselectedOption == "I think you should never reveal your wishes to anyone until they come true.")
                {
                    DP7 = true;
                    // load the next dialogue
                    StartCoroutine(dialogueManager.LoadAndStartDialoguesSequentially(new string[] { "W2D3.5B", "W2D3.6" }));
                }

            }
            if (!string.IsNullOrEmpty(dialogueManager.selectedOption) && DP7 == true)
            {
                string currselectedOption = dialogueManager.selectedOption;

                // restart manager's selected option
                DP7 = false;
                dialogueManager.selectedOption = "";

                // change this to the options that are in your file [up to 4]
                if (currselectedOption == "Oh, well... she basically said I needed to get more friends.")
                {
                    // load the next dialogue
                    StartCoroutine(dialogueManager.LoadAndStartDialoguesSequentially(new string[] { "W2D3.6A", "W2D3.7" }));
                    END = true;

                }
                else if (currselectedOption == "Nothing much! She just asked how classes were going and stuff.")
                {
                    END = true;
                    // load the next dialogue
                    StartCoroutine(dialogueManager.LoadAndStartDialoguesSequentially(new string[] { "W2D3.6B", "W2D3.7" }));
                }

            }

            if (END)
            {
                if (dialogueManager.activeDialogue == false)
                {
                    END = false;
                    dialogueManager.isW2D4 = true;
                    dialogueManager.UpdatePPref(9);

                }


            }



        }
    }
}
