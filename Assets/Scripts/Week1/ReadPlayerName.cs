using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ReadPlayerName : MonoBehaviour
{
    private string input;
    private DialougeManager dialogueManager;
    public TMP_InputField playerNameReader;
    public GameObject dialoguebox;

    // Start is called before the first frame update
    void Start()
    {
        dialogueManager = FindAnyObjectByType<DialougeManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ReadInput(string player_input)
    {
        dialoguebox.SetActive(false);
        input = player_input;
        if(player_input == "")
        {
            dialogueManager.inputtedName = "Chris";
            Debug.Log("player name entered was null default name is : " + dialogueManager.inputtedName);
            dialogueManager.isW1D1 = true;
            dialogueManager.UpdatePPref(1);
            playerNameReader.gameObject.SetActive(false);


        }
        else
        {
            dialogueManager.inputtedName = player_input;
            Debug.Log("player name is: " + dialogueManager.inputtedName);

            //dialogueManager.isW1D1 = true;
            dialogueManager.isW3D2A = true;
            dialogueManager.UpdatePPref(15);
            playerNameReader.gameObject.SetActive(false);
        }

        if(dialogueManager.isW1D1 == true)
        {
            dialoguebox.SetActive(true);
        }

    } 

}
