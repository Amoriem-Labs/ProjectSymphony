using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class W1D2 : MonoBehaviour
{
    private DialougeManager dialogueManager;
    private bool isStartedD2 = false;
    private bool DP1_1 = true;
    private bool DP1_2 = false;
    private bool DP1_3 = false;
    private bool DP1_4 = false;

    bool startTESTS = false;
    bool reherse = false;
    bool library = false;
    bool park = false;
    bool classroom = false;
    bool END =  false;
    // Start is called before the first frame update
    void Start()
    {
        dialogueManager = FindAnyObjectByType<DialougeManager>();

    }

    // Update is called once per frame
    void Update()
    {
        if (dialogueManager.isW1D2)
        {
            if (dialogueManager.isW1D2 && !isStartedD2)
            {
                StartCoroutine(dialogueManager.LoadAndStartDialoguesSequentially(new string[] { "W1D2.0" }));
                isStartedD2 = true;
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

                    if (currselectedOption == "Call out to them.")
                    {
                        StartCoroutine(dialogueManager.LoadAndStartDialoguesSequentially(new string[] { "W1D2.01A", "W1D2.2" }));
                        DP1_2 = true;

                    }
                    else if (currselectedOption == "Sneak up on them.")
                    {
                        DP1_2 = true;
                        StartCoroutine(dialogueManager.LoadAndStartDialoguesSequentially(new string[] { "W1D2.01B", "W1D2.2" }));
                    }


                }

                if (!string.IsNullOrEmpty(dialogueManager.selectedOption) && DP1_2)
                {
                    string currselectedOption = dialogueManager.selectedOption;

                    // restart manager's selected option
                    dialogueManager.selectedOption = "";

                    if (currselectedOption == "Maybe I can see where people rehearse?")
                    {
                        if (classroom && library && park)
                        {
                            StartCoroutine(dialogueManager.LoadAndStartDialoguesSequentially(new string[] { "W1D2.02A" }));
                            DP1_3 = true;
                            DP1_2 = false;

                        }
                        else if (reherse)
                        {
                            StartCoroutine(dialogueManager.LoadAndStartDialoguesSequentially(new string[] { "W1D2.2E" }));
                        }
                        else
                        {
                            reherse = true;
                            StartCoroutine(dialogueManager.LoadAndStartDialoguesSequentially(new string[] { "W1D2.02A", "W1D2.2F" }));
                        }

                    }
                    else if (currselectedOption == "The park!")
                    {
                        if (classroom && library && reherse)
                        {
                            DP1_3 = true;
                            DP1_2 = false;
                            StartCoroutine(dialogueManager.LoadAndStartDialoguesSequentially(new string[] { "W1D2.02B" }));
                        }
                        else if (park)
                        {
                            StartCoroutine(dialogueManager.LoadAndStartDialoguesSequentially(new string[] { "W1D2.2E" }));
                        }
                        else
                        {
                            park = true;
                            StartCoroutine(dialogueManager.LoadAndStartDialoguesSequentially(new string[] { "W1D2.02B", "W1D2.2F" }));
                        }
                    }

                    else if (currselectedOption == "I think seeing a classroom would be nice before I'm forced in there against my will.")
                    {
                        if (library && park && reherse)
                        {
                            DP1_3 = true;
                            DP1_2 = false;
                            StartCoroutine(dialogueManager.LoadAndStartDialoguesSequentially(new string[] { "W1D2.02C" }));
                        }
                        else if (classroom)
                        {
                            StartCoroutine(dialogueManager.LoadAndStartDialoguesSequentially(new string[] { "W1D2.2E" }));
                        }
                        else
                        {
                            classroom = true;
                            StartCoroutine(dialogueManager.LoadAndStartDialoguesSequentially(new string[] { "W1D2.02C", "W1D2.2F" }));
                        }
                    }
                    else if (currselectedOption == "Could we go to the library?")
                    {
                        if (classroom && park && reherse)
                        {
                            StartCoroutine(dialogueManager.LoadAndStartDialoguesSequentially(new string[] { "W1D2.02D" }));
                            DP1_3 = true;
                            DP1_2 = false;
                        }
                        else if (library)
                        {
                            StartCoroutine(dialogueManager.LoadAndStartDialoguesSequentially(new string[] { "W1D2.2E" }));
                        }
                        else
                        {
                            library = true;
                            StartCoroutine(dialogueManager.LoadAndStartDialoguesSequentially(new string[] { "W1D2.02D", "W1D2.2F" }));
                        }
                    }


                    if (classroom && library && park && reherse)
                    {
                        DP1_3 = true;

                    }


                }

                if (DP1_3)
                {
                    if (dialogueManager.activeDialogue == false)
                    {
                        string currselectedOption = dialogueManager.selectedOption;

                        // restart manager's selected option
                        DP1_3 = false;
                        dialogueManager.selectedOption = "";

                        StartCoroutine(dialogueManager.LoadAndStartDialoguesSequentially(new string[] { "W1D2.03" }));

                        DP1_4 = true;

                    }


                }
            }

            if (!string.IsNullOrEmpty(dialogueManager.selectedOption) && DP1_4)
            {
                string currselectedOption = dialogueManager.selectedOption;

                // restart manager's selected option
                if (currselectedOption == "Head back to your room.")
                {
                    DP1_4 = false;

                    dialogueManager.selectedOption = "";
                    startTESTS = false;
                    END = true;

                }

            }



            if (END)
            {
                if (dialogueManager.activeDialogue == false)
                {
                    dialogueManager.isW1D3 = true;
                    dialogueManager.UpdatePPref(3);
                    END = false;


                }
            }


        }
    }

}
