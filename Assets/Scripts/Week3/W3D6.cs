using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class W3D6 : MonoBehaviour
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
        if (dialogueManager.isW3D6)
        {
            if (dialogueManager.isW3D6 && !isStarted)
            {
                // start first texx file
                StartCoroutine(dialogueManager.LoadAndStartDialoguesSequentially(new string[] { "W3D6.0" }));
                // is started triggers the choice tests
                isStarted = true;
                END = true; // MAKE SURE THIS IS AFTER THE FIRST LOAD            
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
