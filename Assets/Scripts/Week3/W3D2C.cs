using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class W3D2C : MonoBehaviour
{
    private DialougeManager dialogueManager;
    private AffectionManager affectionManager;
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
        affectionManager = FindAnyObjectByType<AffectionManager>();


    }

    // Update is called once per frame
    void Update()
    {

        if (dialogueManager.isW3D2C)
        {
            if (dialogueManager.isW3D2C && !isStarted)
            {
                // start first texx file
                StartCoroutine(dialogueManager.LoadAndStartDialoguesSequentially(new string[] { "W3D2.1.1" }));
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
                    if (currselectedOption == "Do what he says.")
                    {
                        affectionManager.UpdateCharacterAffection("Carter", 4);
                        // load the next dialogue
                        StartCoroutine(dialogueManager.LoadAndStartDialoguesSequentially(new string[] { "W3D2.1.1a", "W3D2.1.2" }));
                        DP4 = true;

                    }
                    else if (currselectedOption == "Walk straight towards him anyway.")
                    {
                        affectionManager.UpdateCharacterAffection("Carter", -2);
                        DP4 = true;
                        // load the next dialogue
                        StartCoroutine(dialogueManager.LoadAndStartDialoguesSequentially(new string[] { "W3D2.1.1b", "W3D2.1.2" }));
                    }
                }
            }

            if (!string.IsNullOrEmpty(dialogueManager.selectedOption) && DP4)
            {
                string currselectedOption = dialogueManager.selectedOption;

                // restart manager's selected option
                dialogueManager.selectedOption = "";

                if (currselectedOption == "Okay!" || currselectedOption == "Whatever.")
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

                    // add scene management stuff
                    ////update map week num
                    //dialogueManager.isW3D5 = true;
                    //dialogueManager.UpdatePPref(18);
                    //END = false;
                    END = false;
                    // add scene management stuff
                    //update map week num
                    PlayerPrefs.SetInt("CurrentWeek.", 3);
                    PlayerPrefs.SetInt("SceneToLoad", 3);
                    //SceneManager.LoadScene("Splash");
                    SceneManager.LoadScene("MapScreen");
                }


            }

        }
    }
}
