using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class W2D5 : MonoBehaviour
{
    private DialougeManager dialogueManager;

    private bool DP1 = true;
    private bool isStarted = false;

    bool startChoiceDetection = false;
    bool END = false;
    // Start is called before the first frame update
    void Start()
    {
        dialogueManager = FindAnyObjectByType<DialougeManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (dialogueManager.isW2D5)
        {
            if (!dialogueManager.isW2D4 && !isStarted)
            {
                StartCoroutine(dialogueManager.LoadAndStartDialoguesSequentially(new string[] { "W2D5" }));
                isStarted = true;
                startChoiceDetection = true;


            }

        }
        if (startChoiceDetection)
        {
            if (!string.IsNullOrEmpty(dialogueManager.selectedOption) && DP1)
            {
                string currselectedOption = dialogueManager.selectedOption;
                Debug.Log("enter this part");

                // restart manager's selected option
                DP1 = false;
                dialogueManager.selectedOption = "";

                // change this to the options that are in your file [up to 4]
                if (currselectedOption == "Let's play it!")
                {
                    DP1 = false;
                    dialogueManager.selectedOption = "";

                    startChoiceDetection = false;
                    END = true;


                }
                else if (currselectedOption == "I wanna hear!")
                {
                    DP1 = false;

                    dialogueManager.selectedOption = "";

                    startChoiceDetection = false;
                    END = true; ;
                }
            }
        }


        if (END && !dialogueManager.activeDialogue)
        {

            END = false;
            dialogueManager.isW2D5 = true;
            dialogueManager.UpdatePPref(11);

        }
    }
}
