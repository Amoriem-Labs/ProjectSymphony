using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class W3D5 : MonoBehaviour
{

    private DialougeManager dialogueManager;

    private AffectionManager affectionManager;
    private bool isStarted = false;
    bool startChoiceDetection = false;

    private bool DP1 = true;
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
        if (dialogueManager.isW3D5)
        {
            if (dialogueManager.isW3D5 && !isStarted)
            {
                // start first texx file
                float CarterAffection = affectionManager.GetCharacterAffection("Carter");
                float PaulineAffection = affectionManager.GetCharacterAffection("Pauline");
                float DayloAffection = affectionManager.GetCharacterAffection("Daylo");

                Debug.Log("Carter Affection Is " + CarterAffection);
                Debug.Log("Pauline Affection Is " + PaulineAffection);
                Debug.Log("Daylo Affection Is" + DayloAffection);

                // ADDED HERE FOR TESTING
                affectionManager.UpdateCharacterUnlock("Sam", true);
                affectionManager.UpdateCharacterUnlock("Howard", true);

                string winner = "";

                if (CarterAffection > PaulineAffection && CarterAffection > DayloAffection)
                {
                    winner = "Carter";
                    Debug.Log("Carter is winner");
                }
                else if (PaulineAffection > CarterAffection && PaulineAffection > DayloAffection)
                {
                    winner = "Pauline";
                    Debug.Log("Pauline is winner");
                }
                else if (DayloAffection > CarterAffection && DayloAffection > PaulineAffection)
                {
                    winner = "Daylo";
                    Debug.Log("Daylo is winner");
                }
                else
                {
                    // TESTING TESTING!!!
                    affectionManager.UpdateCharacterAffection("Pauline", 100);
                    affectionManager.UpdateCharacterAffection("Carter", 100);
                    affectionManager.UpdateCharacterAffection("Daylo", 100);
                    Debug.Log("Tie, set to Carter as default");
                    winner = "Daylo";


                }

                // based on winner, pick different branches
                if (winner == "Carter")
                {
                    Debug.Log("playing carter branch");
                    StartCoroutine(dialogueManager.LoadAndStartDialoguesSequentially(new string[] { "W3D5.0", "W3D5.0A" }));
                    // is started triggers the choice tests
                    isStarted = true; // MAKE SURE THIS IS AFTER THE FIRST LOAD 
                    startChoiceDetection = true;
                }
                if (winner == "Pauline")
                {
                    Debug.Log("playing Pauline branch");
                    StartCoroutine(dialogueManager.LoadAndStartDialoguesSequentially(new string[] { "W3D5.0", "W3D5.0B" }));
                    // is started triggers the choice tests
                    isStarted = true; // MAKE SURE THIS IS AFTER THE FIRST LOAD 
                    startChoiceDetection = true;
                }
                if (winner == "Daylo")
                {
                    Debug.Log("playing Daylo branch");
                    StartCoroutine(dialogueManager.LoadAndStartDialoguesSequentially(new string[] { "W3D5.0", "W3D5.0C" }));
                    // is started triggers the choice tests
                    isStarted = true; // MAKE SURE THIS IS AFTER THE FIRST LOAD 
                    startChoiceDetection = true;
                }

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
                    if (currselectedOption == "Carter.")
                    {
                        // load the next dialogue
                        DP4 = true;
                        StartCoroutine(dialogueManager.LoadAndStartDialoguesSequentially(new string[] { "W3D5.1" }));

                        affectionManager.UpdateCharacterUnlock("Carter", true);

                        if (affectionManager.CharacterIsUnlocked("Carter"))
                        {
                            Debug.Log("Carter is unlocked");
                        }

                    }
                    else if (currselectedOption == "Pauline.")
                    {
                        DP4 = true;
                        // load the next dialogue
                        StartCoroutine(dialogueManager.LoadAndStartDialoguesSequentially(new string[] { "W3D5.2" }));

                        affectionManager.UpdateCharacterUnlock("Pauline", true);

                        if (affectionManager.CharacterIsUnlocked("Pauline"))
                        {
                            Debug.Log("Pauline is unlocked");
                        }
                    }
                    else if (currselectedOption == "Daylo.")
                    {
                        DP4 = true;
                        // load the next dialogue
                        StartCoroutine(dialogueManager.LoadAndStartDialoguesSequentially(new string[] { "W3D5.3" }));

                        affectionManager.UpdateCharacterUnlock("Daylo", true);

                        if (affectionManager.CharacterIsUnlocked("Daylo"))
                        {
                            Debug.Log("Daylo is unlocked");
                        }
                    }
                }



            }

            if (!string.IsNullOrEmpty(dialogueManager.selectedOption) && DP4)
            {
                string currselectedOption = dialogueManager.selectedOption;

                // restart manager's selected option
                dialogueManager.selectedOption = "";

                if (currselectedOption == "Oh..." || currselectedOption == "I'm glad!")
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
                    PlayerPrefs.SetInt("CurrentWeek.", 3);
                    dialogueManager.isW3D7 = true;
                    dialogueManager.UpdatePPref(20);
                    //dialogueManager.isW3D2C = true;
                    //dialogueManager.UpdatePPref(21);
                    //PlayerPrefs.SetInt("SceneToLoad", 3);
                    ////SceneManager.LoadScene("Splash");
                    //SceneManager.LoadScene("MapScreen");
                    //PlayerPrefs.SetInt("Looping3", 0);

                }


            }

        }
    }
}
