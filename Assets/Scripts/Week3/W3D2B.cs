using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class W3D2B : MonoBehaviour
{
    private DialougeManager dialogueManager;
    private AffectionManager affectionManager;
    private bool isStarted = false;
    bool startChoiceDetection = false;

    // make one bool for each decision point
    private bool DP1 = true;
    private bool DP2 = false;
    private bool DP4 = false;


    bool END = false;

    // Start is called before the first frame update
    void Start()
    {
        dialogueManager = FindAnyObjectByType<DialougeManager>();
        affectionManager = FindAnyObjectByType<AffectionManager>();

    }

    // Update is called once per frame
    void Update()
    {
        if (dialogueManager.isW3D2B) {


            if (dialogueManager.isW3D2B && !isStarted)
            {
                // start first texx file
                //TODO: change to have condition later
                StartCoroutine(dialogueManager.LoadAndStartDialoguesSequentially(new string[] { "W3D2.2.1a" }));
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
                    if (currselectedOption == "Lie to Daylo.")
                    {
                        affectionManager.UpdateCharacterAffection("Daylo", 2);
                        // load the next dialogue
                        StartCoroutine(dialogueManager.LoadAndStartDialoguesSequentially(new string[] { "W3D2.2.1aa", "W3D2.2.1b" }));
                        DP2 = true;

                    }
                    else if (currselectedOption == "Tell Daylo the truth.")
                    {
                        affectionManager.UpdateCharacterAffection("Daylo", -1);
                        DP2 = true;
                        // load the next dialogue
                        StartCoroutine(dialogueManager.LoadAndStartDialoguesSequentially(new string[] { "W3D2.2.1ab", "W3D2.2.1b" }));
                    }

                }
            }
            if (!string.IsNullOrEmpty(dialogueManager.selectedOption) && DP2 == true)
            {
                string currselectedOption = dialogueManager.selectedOption;

                // restart manager's selected option
                DP2 = false;
                dialogueManager.selectedOption = "";

                // change this to the options that are in your file [up to 4]
                if (currselectedOption == "To become a better musician.")
                {
                    affectionManager.UpdateCharacterAffection("Daylo", 2);
                    // load the next dialogue
                  
                    StartCoroutine(dialogueManager.LoadAndStartDialoguesSequentially(new string[] { "W3D2.2.1ba", "W3D2.2.3" }));
                    DP4 = true;

                }
                else if (currselectedOption == "To make new friends.")
                {
                    affectionManager.UpdateCharacterAffection("Daylo", -1);
                    // load the next dialogue
                    StartCoroutine(dialogueManager.LoadAndStartDialoguesSequentially(new string[]{ "W3D2.2.1bb", "W3D2.2.3" }));
                    DP4 = true;

                }
                else if(currselectedOption == "To form the best band in the multiverse.")
                {
                    affectionManager.UpdateCharacterAffection("Daylo", 1);
                    // load the next dialogue
                    StartCoroutine(dialogueManager.LoadAndStartDialoguesSequentially(new string[]{ "W3D2.2.1bc", "W3D2.2.3" }));
                    DP4 = true;
                }

            }

            if (!string.IsNullOrEmpty(dialogueManager.selectedOption) && DP4)
            {
                string currselectedOption = dialogueManager.selectedOption;

                // restart manager's selected option
                dialogueManager.selectedOption = "";

                if (currselectedOption == "Okay." || currselectedOption == "My pleasure!")
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
                    //END = false;
                    // dialogueManager.isW3D4 = true;
                    // dialogueManager.UpdatePPref(14);

                    dialogueManager.isW3D1B = true;
                    dialogueManager.UpdatePPref(13);
                    //PlayerPrefs.SetInt("Looping3", 1);
                    END = false;

                }


            }


        }
    }
}
