using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

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
        if (player_input == "")
        {
            dialogueManager.inputtedName = "Chris";
            Debug.Log("player name entered was null default name is : " + dialogueManager.inputtedName);

            GameStateManager.Instance.LoadCharacterSelect("Prelude");

            //dialogueManager.isW1D1 = true;
            dialogueManager.UpdatePPref(1);
            playerNameReader.gameObject.SetActive(false);


        }
        else
        {
            dialogueManager.inputtedName = player_input;
            Debug.Log("player name is: " + dialogueManager.inputtedName);
            PlayerPrefs.SetString("PlayerName.", dialogueManager.inputtedName);

            // GameStateManager.Instance.LoadCharacterSelect("Prelude");

            dialogueManager.isW1D1 = true; // commented out bc of rhythm transition 
            // dialogueManager.UpdatePPref(1);

            // StartCoroutine(WaitForSceneLoad());

            //GameStateManager.Instance.LoadCharacterSelect("Prelude");

            dialogueManager.UpdatePPref(1);
            //PlayerPrefs.SetInt("SceneIndex.", 1);
            playerNameReader.gameObject.SetActive(false);

            Debug.Log("set player pref scene index to 1");
            ////dialogueManager.isW = true;
        }

        if (dialogueManager.isW1D1 == true)
        {
            dialoguebox.SetActive(true);
        }

    }

    private IEnumerator WaitForSceneLoad()
    {
        // Trigger scene load
        dialogueManager.UpdatePPref(1);

        GameStateManager.Instance.LoadCharacterSelect("Prelude");

        yield return new WaitForSeconds(2f);

    }

}
