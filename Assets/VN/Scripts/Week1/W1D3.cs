using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class W1D3 : MonoBehaviour
{
    private DialougeManager dialogueManager;
    private bool isStartedD3 = false;
    private bool DP1_1 = true;
    private bool DP1_2 = false;
    private bool DP1_3 = false;
    private bool DP1_4 = false;
    private bool DP1_5 = false;
    bool startTESTS;
    bool END;

    // Start is called before the first frame update
    void Start()
    {
        dialogueManager = FindAnyObjectByType<DialougeManager>();

    }

    // Update is called once per frame
    void Update()
    {

        if (dialogueManager.isW1D3 && !isStartedD3)
        {
            StartCoroutine(dialogueManager.LoadAndStartDialoguesSequentially(new string[] { "W1D3.0" }));
            isStartedD3 = true;
            startTESTS = true;
        }

        if (startTESTS)
        {

            if (!string.IsNullOrEmpty(dialogueManager.selectedOption) && DP1_1)
            {
                string currselectedOption = dialogueManager.selectedOption;

                // restart manager's selected option
                DP1_1 = false;
                dialogueManager.selectedOption = "";

                if (currselectedOption == "That sounds intense. I'm glad I'll have you guys to help me through it.")
                {
                    StartCoroutine(dialogueManager.LoadAndStartDialoguesSequentially(new string[] { "W1D3.0A", "W1D3.1" }));
                    DP1_2 = true;

                }
                else if (currselectedOption == "That sounds like fun! Good thing we're in this together.")
                {
                    DP1_2 = true;
                    StartCoroutine(dialogueManager.LoadAndStartDialoguesSequentially(new string[] { "W1D3.0B", "W1D3.1" }));
                }


            }


            if (!string.IsNullOrEmpty(dialogueManager.selectedOption) && DP1_2)
            {
                string currselectedOption = dialogueManager.selectedOption;

                // restart manager's selected option
                DP1_2 = false;
                dialogueManager.selectedOption = "";

                if (currselectedOption == "It was better than I thought it would be.")
                {
                    StartCoroutine(dialogueManager.LoadAndStartDialoguesSequentially(new string[] { "W1D3.1A", "W1D3.2" }));
                    DP1_3 = true;

                }
                else if (currselectedOption == "It was worse than I thought it would be.")
                {
                    DP1_3 = true;
                    StartCoroutine(dialogueManager.LoadAndStartDialoguesSequentially(new string[] { "W1D3.1B", "W1D3.2" }));
                }

           
            }


            if (!string.IsNullOrEmpty(dialogueManager.selectedOption) && DP1_3)
            {
                string currselectedOption = dialogueManager.selectedOption;

                // restart manager's selected option
                DP1_3 = false;
                dialogueManager.selectedOption = "";

                if (currselectedOption == "Of course, I'll send you a photo!")
                {
                    StartCoroutine(dialogueManager.LoadAndStartDialoguesSequentially(new string[] { "W1D3.2A", "W1D3.3" }));

                }
                else if (currselectedOption == "Sorry, but not today.")
                {
                    StartCoroutine(dialogueManager.LoadAndStartDialoguesSequentially(new string[] { "W1D3.2B", "W1D3.3" }));
                }


            }

            if(dialogueManager.activeDialogue ==  false && END)
            {
                Debug.Log("ENTERED");
                SceneManager.LoadScene("Start copy");
            }

        }



    }
}
