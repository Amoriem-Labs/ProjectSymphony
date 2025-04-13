using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class W3D1A : MonoBehaviour
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
        if (dialogueManager.isW3D1A)
        {
            if (dialogueManager.isW3D1A && !isStarted)
            {
                // start first texx file
                StartCoroutine(dialogueManager.LoadAndStartDialoguesSequentially(new string[] { "W3D1A.0" }));
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

                if (!string.IsNullOrEmpty(dialogueManager.selectedOption) && DP2)
                {
                    string currselectedOption = dialogueManager.selectedOption;

                    // restart manager's selected option
                    DP2 = false;
                    dialogueManager.selectedOption = "";

                    // change this to the options that are in your file [up to 4]
                    if (currselectedOption == "Leave them alone.")
                    {
                        // load the next dialogue
                        StartCoroutine(dialogueManager.LoadAndStartDialoguesSequentially(new string[] { "W3D1A.1A" }));
                        END = true;

                    }
                    else if (currselectedOption == "Help them with their plan.")
                    {
                        DP3 = true;
                        // load the next dialogue
                        StartCoroutine(dialogueManager.LoadAndStartDialoguesSequentially(new string[] { "W3D1A.1B" }));
                    }

                }


                if (!string.IsNullOrEmpty(dialogueManager.selectedOption) && DP3)
                {
                    string currselectedOption = dialogueManager.selectedOption;

                    // restart manager's selected option
                    DP3 = false;
                    dialogueManager.selectedOption = "";

                    // change this to the options that are in your file [up to 4]
                    if (currselectedOption == "Tell Sam about the prank.")
                    {
                        // load the next dialogue
                        StartCoroutine(dialogueManager.LoadAndStartDialoguesSequentially(new string[] { "W3D1A.1C", "W3D1A.2" }));
                        DP4 = true;

                    }
                    else if (currselectedOption == "Don't tell Sam about the prank.")
                    {
                        // load the next dialogue
                        StartCoroutine(dialogueManager.LoadAndStartDialoguesSequentially(new string[] { "W3D1A.2" }));

                        // restart manager's selected option
                        dialogueManager.selectedOption = "";

                        //StartCoroutine(dialogueManager.LoadAndStartDialoguesSequentially(new string[] { "W1D3.3" }));
                        startChoiceDetection = false;
                        DP4 = true;
                    }

                }

            }

            if (!string.IsNullOrEmpty(dialogueManager.selectedOption) && DP4)
            {
                string currselectedOption = dialogueManager.selectedOption;

                // restart manager's selected option
                dialogueManager.selectedOption = "";

                if (currselectedOption == "Ok." || currselectedOption == "See you then!")
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
                    PlayerPrefs.SetInt("CurrentWeek.", 21);
                    PlayerPrefs.SetInt("SceneToLoad", 3);
                    //SceneManager.LoadScene("Splash");
                    SceneManager.LoadScene("MapScreen");
                }


            }

        }
    }
}
