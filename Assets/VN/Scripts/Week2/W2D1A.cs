using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.IO;
using UnityEngine.SceneManagement;

public class W2D1A : MonoBehaviour
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
        if (dialogueManager.isW2D1A)
        {
            if (dialogueManager.isW2D1A && !isStarted)
            {
                // start first texx file
                StartCoroutine(dialogueManager.LoadAndStartDialoguesSequentially(new string[] { "W2D1A.1" }));
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
                        StartCoroutine(dialogueManager.LoadAndStartDialoguesSequentially(new string[] { "W2D1.2.1a", "W2D1.2.2" }));
                        DP2 = true;

                    }
                    else if (currselectedOption == "Question Sam's sanity.")
                    {
                        DP2 = true;
                        // load the next dialogue
                        StartCoroutine(dialogueManager.LoadAndStartDialoguesSequentially(new string[] { "W2D1.2.1b", "W2D1.2.2" }));
                    }


                }

                if (!string.IsNullOrEmpty(dialogueManager.selectedOption) && DP2)
                {
                    string currselectedOption = dialogueManager.selectedOption;

                    // restart manager's selected option
                    DP2 = false;
                    dialogueManager.selectedOption = "";

                    // change this to the options that are in your file [up to 4]
                    if (currselectedOption == "Draw attention to the spectacle.")
                    {
                        // load the next dialogue
                        StartCoroutine(dialogueManager.LoadAndStartDialoguesSequentially(new string[] { "W2D1.2.2a", "W2D1.2.3" }));
                        END = true;

                    }
                    else if (currselectedOption == "Bring the danger into focus.")
                    {
                        // load the next dialogue
                        StartCoroutine(dialogueManager.LoadAndStartDialoguesSequentially(new string[] { "W2D1.2.2B", "W2D1.2.3" }));
                        END = true;

                    }

                    else if (currselectedOption == "Give the crowd some context.")
                    {
                        // load the next dialogue
                        StartCoroutine(dialogueManager.LoadAndStartDialoguesSequentially(new string[] { "W2D1.2.2c", "W2D1.2.3" }));
                        END = true;

                    }
                    else if (currselectedOption == "Hype up how cool Sam is.")
                    {
                        // load the next dialogue
                        StartCoroutine(dialogueManager.LoadAndStartDialoguesSequentially(new string[] { "W2D1.2.2d", "W2D1.2.3" }));
                        END = true;

                    }

                    if ( END)
                    {
                        // add scene management stuff
                        //update map week num
                        Debug.Log("end scene");
                        PlayerPrefs.SetInt("CurrentWeek.", 21);
                        PlayerPrefs.SetInt("SceneToLoad", 3);
                        SceneManager.LoadScene("Splash");
     
                    }

                }


            }

           
        }

    }
}
