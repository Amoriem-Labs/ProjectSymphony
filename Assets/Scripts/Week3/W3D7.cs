using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class W3D7 : MonoBehaviour
{
    private DialougeManager dialogueManager;
    private AffectionManager affectionManager;


    private bool isStarted = false;
    bool startChoiceDetection = false;

    private bool DP1 = true;


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
        if (dialogueManager.isW3D7)
            if (!isStarted)
            {
                if (affectionManager.CharacterIsUnlocked("Carter"))
                {
                    Debug.Log("!!! Carter");
                    StartCoroutine(dialogueManager.LoadAndStartDialoguesSequentially(new string[] { "W3D7A.0" }));
                    isStarted = true;
                    startChoiceDetection = true;

                }
                else if (affectionManager.CharacterIsUnlocked("Pauline"))
                {
                    Debug.Log("!!! Pauline");

                    StartCoroutine(dialogueManager.LoadAndStartDialoguesSequentially(new string[] { "W3D7B.0" }));
                    isStarted = true;
                    startChoiceDetection = true;

                }
                else if (affectionManager.CharacterIsUnlocked("Daylo"))
                {
                    Debug.Log("!!! daylo");

                    StartCoroutine(dialogueManager.LoadAndStartDialoguesSequentially(new string[] { "W3D7C.0" }));
                    isStarted = true;
                    startChoiceDetection = true;

                }
                else
                {
                    StartCoroutine(dialogueManager.LoadAndStartDialoguesSequentially(new string[] { "W3D7C.0" }));
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
                if (currselectedOption == "Just like we practiced guys.")
                {
                    DP1 = false;
                    dialogueManager.selectedOption = "";

                    startChoiceDetection = false;
                    END = true;


                }
                else if (currselectedOption == "We got this.")
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

            // end, reset here 
            END = false;
            isStarted = false;
            GameStateManager.Instance.DemoComplete = true;
            GameStateManager.Instance.LoadCharacterSelect("Week 3");


        }
    }
}
