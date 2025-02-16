using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.IO;
using UnityEngine.SceneManagement;

public class Day0 : MonoBehaviour
{
    private Dialouge currDialogue;

    public TextMeshProUGUI name_text;
    public TextMeshProUGUI dialouge_text;
    private DialougeManager dialogueManager;
    private bool DP0_1;
    public TMP_InputField playerNameReader;
    // Start is called before the first frame update
    void Start()
    {
        DP0_1 = false;
        playerNameReader.gameObject.SetActive(false); 

        // get dialoguemanager and verify
        dialogueManager = FindAnyObjectByType<DialougeManager>();

        if (dialogueManager == null)
        {
            Debug.LogError("DialougeManager not found in the scene");
            return;
        }
        StartCoroutine(dialogueManager.LoadAndStartDialoguesSequentially(new string[] { "W1D0.0" }));
        

    }

    // choices and decision points in update
    void Update()
    {
        // Decision Point 1
        if (!string.IsNullOrEmpty(dialogueManager.selectedOption) && !DP0_1)
        {
            DP0_1 = true;
            string currselectedOption = dialogueManager.selectedOption;

            // restart manager's selected option
            dialogueManager.selectedOption = "";

            Debug.Log(currselectedOption);
            if (currselectedOption == "Yes.")
            {
                Debug.Log("chose yes");

                playerNameReader.gameObject.SetActive(true);
            }
            else if (currselectedOption == "No.")
            {
                StartCoroutine(dialogueManager.LoadAndStartDialoguesSequentially(new string[] { "W1D0.1a" }));
            }


        }
      
        
    }
}