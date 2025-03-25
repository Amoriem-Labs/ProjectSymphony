using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.IO;

public class W1D1 : MonoBehaviour
{
    private DialougeManager dialogueManager;
    private bool isStarted = false;
    private bool DP1_1 = true;
    private bool DP1_2 = false;
    private bool DP1_3 = false;
    private bool DP1_4 = false;
    private bool DP1_5 = false;
    private bool DP1_6 = false;
    private bool DP1_7 = false;
    private bool DP1_8 = false;
    private bool DP1_9 = false;
    bool END = false;
    // Start is called before the first frame update
    void Start()
    {
        dialogueManager = FindAnyObjectByType<DialougeManager>();

    }

    // Update is called once per frame
    void Update()
    {
        if (dialogueManager.isW1D1 && !isStarted)
        {
            StartCoroutine(dialogueManager.LoadAndStartDialoguesSequentially(new string[] { "W1D1.0" }));
            isStarted = true;
        }


        if (!string.IsNullOrEmpty(dialogueManager.selectedOption) && DP1_1)
        {           
            string currselectedOption = dialogueManager.selectedOption;

            // restart manager's selected option
            DP1_1 = false;
            dialogueManager.selectedOption = "";

            if(currselectedOption == "Talk to the strong guys.")
            {
                StartCoroutine(dialogueManager.LoadAndStartDialoguesSequentially(new string[] { "W1D1.0a", "W1D1.1" }));

               
                DP1_2 = true;

            }
            else if(currselectedOption == "Walk past them." )
            {
                DP1_2 = true;
                StartCoroutine(dialogueManager.LoadAndStartDialoguesSequentially(new string[] { "W1D1.1" }));
            }


        }

        if (!string.IsNullOrEmpty(dialogueManager.selectedOption) && DP1_2)
        {
            DP1_2 = false;
            string currselectedOption = dialogueManager.selectedOption;

            // restart manager's selected option
            dialogueManager.selectedOption = "";

            if (currselectedOption == "Talk to the popular kids.")
            {
                StartCoroutine(dialogueManager.LoadAndStartDialoguesSequentially(new string[] { "W1D1.1a", "W1D1.2" }));
                DP1_3 = true;
            }
            else if (currselectedOption == "Walk past them.")
            {

                StartCoroutine(dialogueManager.LoadAndStartDialoguesSequentially(new string[] {"W1D1.2" }));
                DP1_3 = true;
            }

        }

        if (!string.IsNullOrEmpty(dialogueManager.selectedOption) && DP1_3)
        {
            DP1_3 = false;
            string currselectedOption = dialogueManager.selectedOption;

            // restart manager's selected option
            dialogueManager.selectedOption = "";

            if (currselectedOption == "Talk to the nerds.")
            {
                DP1_4 = true;
                StartCoroutine(dialogueManager.LoadAndStartDialoguesSequentially(new string[] { "W1D1.2a", "W1D1.3" }));
                
            }
            else if (currselectedOption == "Walk past them.")
            {
                DP1_4 = true;
                StartCoroutine(dialogueManager.LoadAndStartDialoguesSequentially(new string[] { "W1D1.3" }));
            }

        }



        if (!string.IsNullOrEmpty(dialogueManager.selectedOption) && DP1_4)
        {
            DP1_4 = false;
            string currselectedOption = dialogueManager.selectedOption;

            // restart manager's selected option
            dialogueManager.selectedOption = "";

            if (currselectedOption == "Why is everyone so unapproachable around here?")
            {
                DP1_5 = true;
                StartCoroutine(dialogueManager.LoadAndStartDialoguesSequentially(new string[] { "W1D1.3a" }));


            }
            else if (currselectedOption == "Hey... what are you guys up to?")
            {
                DP1_5 = true;
                StartCoroutine(dialogueManager.LoadAndStartDialoguesSequentially(new string[] { "W1D1.3a" }));
            }

        }

        if (!string.IsNullOrEmpty(dialogueManager.selectedOption) && DP1_5)
        {
            DP1_5 = false;
            string currselectedOption = dialogueManager.selectedOption;

            // restart manager's selected option
            dialogueManager.selectedOption = "";

            if (currselectedOption == "I'm from PLANET ONE.")
            {
                DP1_6 = true;
                StartCoroutine(dialogueManager.LoadAndStartDialoguesSequentially(new string[] { "W1D1.4" }));


            }
            else if (currselectedOption == "I'm from PLANET TWO.")
            {
                DP1_6 = true;
                StartCoroutine(dialogueManager.LoadAndStartDialoguesSequentially(new string[] { "W1D1.4" }));
            }

        }


        if (!string.IsNullOrEmpty(dialogueManager.selectedOption) && DP1_6)
        {
            DP1_6 = false;
            string currselectedOption = dialogueManager.selectedOption;

            // restart manager's selected option
            dialogueManager.selectedOption = "";

            if (currselectedOption == "That'd be so cool!")
            {
                DP1_7 = true;
                StartCoroutine(dialogueManager.LoadAndStartDialoguesSequentially(new string[] { "W1D1.5" }));


            }
            else if (currselectedOption == "Oh, uh, sure! I guess...")
            {
                DP1_7 = true;
                StartCoroutine(dialogueManager.LoadAndStartDialoguesSequentially(new string[] { "W1D1.5" }));
            }

        }


        if (!string.IsNullOrEmpty(dialogueManager.selectedOption) && DP1_7)
        {
            DP1_7 = false;
            string currselectedOption = dialogueManager.selectedOption;

            // restart manager's selected option
            dialogueManager.selectedOption = "";

            if (currselectedOption == "Who's Patricia?")
            {
                DP1_8 = false;
                StartCoroutine(dialogueManager.LoadAndStartDialoguesSequentially(new string[] { "W1D1.5a" }));
                END = true;

            }
            else if (currselectedOption == "Do you guys know how to find all of your classes?")
            {
                DP1_8 = true;
                StartCoroutine(dialogueManager.LoadAndStartDialoguesSequentially(new string[] { "W1D1.5b" }));
            }

        }
        if (!string.IsNullOrEmpty(dialogueManager.selectedOption) && DP1_8)
        {
            DP1_8 = false;
            string currselectedOption = dialogueManager.selectedOption;

            // restart manager's selected option
            dialogueManager.selectedOption = "";

            if (currselectedOption == "That would be awesome! Let's do it!")
            {
                StartCoroutine(dialogueManager.LoadAndStartDialoguesSequentially(new string[] { "W1D1.5c" }));
                //dialogueManager.isW1D2 = true;
                END = true;


            }
            else if (currselectedOption == "That would be great, thanks.")
            {
                StartCoroutine(dialogueManager.LoadAndStartDialoguesSequentially(new string[] { "W1D1.5c" }));
                END = true;
            }

        }

        if (END)
        {
            if(dialogueManager.activeDialogue == false)
            {
                END = false;
                dialogueManager.isW1D2 = true;
                dialogueManager.UpdatePPref(dialogueManager.isW1D2);

            }


        }
    }
}
