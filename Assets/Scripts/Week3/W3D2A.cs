using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class W3D2A : MonoBehaviour
{
    private DialougeManager dialogueManager;
    private bool isStarted = false;
    bool startChoiceDetection = false;

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
        if (dialogueManager.isW3D2A)
        {
            if (dialogueManager.isW3D2A && !isStarted)
            {
                // start first texx file
                //TODO: change to have condition later
                StartCoroutine(dialogueManager.LoadAndStartDialoguesSequentially(new string[] { "W3D2.3.1" }));
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
                    if (currselectedOption == "Hey Pauline! What are you up to?")
                    {
                        // load the next dialogue
                        StartCoroutine(dialogueManager.LoadAndStartDialoguesSequentially(new string[] { "W3D2.3.2" }));
                        DP2 = true;

                    }
                    else if (currselectedOption == "Hey Pauline, the bassoon is sounding really nice!")
                    {
                        DP2 = true;
                        // load the next dialogue
                        StartCoroutine(dialogueManager.LoadAndStartDialoguesSequentially(new string[] { "W3D2.3.2" }));
                    }

                }
                if (!string.IsNullOrEmpty(dialogueManager.selectedOption) && DP2 == true)
                {
                    string currselectedOption = dialogueManager.selectedOption;

                    // restart manager's selected option
                    DP2 = false;
                    dialogueManager.selectedOption = "";

                    // change this to the options that are in your file [up to 4]
                    if (currselectedOption == "I saw you by the glowing bugs and wondered what was up.")
                    {
                        // load the next dialogue

                        StartCoroutine(dialogueManager.LoadAndStartDialoguesSequentially(new string[] { "W3D2.3.3b" }));
                        DP3 = true;

                    }
                    else if (currselectedOption == "I heard you playing the bassoon! ")
                    {
                        DP3 = true;
                        // load the next dialogue
                        StartCoroutine(dialogueManager.LoadAndStartDialoguesSequentially(new string[] { "W3D2.3.3b" }));
                    }

                }

                if (!string.IsNullOrEmpty(dialogueManager.selectedOption) && DP3 == true)
                {
                    string currselectedOption = dialogueManager.selectedOption;

                    // restart manager's selected option
                    DP3 = false;
                    dialogueManager.selectedOption = "";

                    // change this to the options that are in your file [up to 4]
                    if (currselectedOption == "Why don't we catch some of these lighting bugs?")
                    {
                        // load the next dialogue
                        DP4 = true;
                        StartCoroutine(dialogueManager.LoadAndStartDialoguesSequentially(new string[] { "W3D2.3.3ba" }));


                    }
                    else if (currselectedOption == "You collect bugs right? Do you have any of these flies already in a jar?")
                    {
                        // load the next dialogue
                        DP4 = true;
                        StartCoroutine(dialogueManager.LoadAndStartDialoguesSequentially(new string[] { "W3D2.3.3ba" }));

                    }

                }
                if (!string.IsNullOrEmpty(dialogueManager.selectedOption) && DP4 == true)
                {
                    string currselectedOption = dialogueManager.selectedOption;

                    // restart manager's selected option
                    DP4 = false;
                    dialogueManager.selectedOption = "";

                    // change this to the options that are in your file [up to 4]
                    if (currselectedOption == "See you later!")
                    {
                        // load the next dialogue

                        // StartCoroutine(dialogueManager.LoadAndStartDialoguesSequentially(new string[] { "W3D2.3.3ba" }));
                        END = true;

                    }
                    else if (currselectedOption == "Hopefully we can do this again!")
                    {
                        // load the next dialogue
                        // StartCoroutine(dialogueManager.LoadAndStartDialoguesSequentially(new string[] { "W3D2.3.3ba" }));
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
                        PlayerPrefs.SetInt("CurrentWeek.", 21);
                        PlayerPrefs.SetInt("SceneToLoad", 3);
                        //SceneManager.LoadScene("Splash");
                        SceneManager.LoadScene("MapScreen");
                    }

                }


            }
        }
    }
}
