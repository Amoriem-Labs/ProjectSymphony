using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class W2D6 : MonoBehaviour
{

    private DialougeManager dialogueManager;
    private bool isStarted = false;
    private bool DP1 = true;
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
        if (dialogueManager.isW2D6)
            if (dialogueManager.isW2D6 && !isStarted)
            {
                StartCoroutine(dialogueManager.LoadAndStartDialoguesSequentially(new string[] { "W2D6" }));
                isStarted = true;
                startChoiceDetection = true;

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
                if (currselectedOption == "Go!")
                {
                    Debug.Log("selected go");
                    DP1 = false;
                    dialogueManager.selectedOption = "";

                    startChoiceDetection = false;
                    END = true;
                }
                else
                {
                    DP1 = false;
                    dialogueManager.selectedOption = "";

                    startChoiceDetection = false;
                    END = true;
                }

            }
        }

        if (END && !dialogueManager.activeDialogue)
        {

            // scene transition to map?
            END = false;
            // add scene management stuff
            //update map week num
            PlayerPrefs.SetInt("CurrentWeek.", 3);
            dialogueManager.isW3D2B = true;


            GameStateManager.Instance.LoadCharacterSelect("Week 2");
            PlayerPrefs.SetInt("SceneIndex.", 12);


            //dialogueManager.UpdatePPref(12);
            //PlayerPrefs.SetInt("Looping3", 1);


        }
    }
}
