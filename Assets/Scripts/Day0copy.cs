using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Day0copy : MonoBehaviour
{
    private Dialouge sam_test;

    public GameObject choiceBox; // new edit here
    public Button choice1;
    public Button choice2;
    public Button Continuebutton;

   

    public TextMeshProUGUI name_text;
    public TextMeshProUGUI dialouge_text; 
    private DialougeManager dialogueManager;
    private bool DecisionPointOne;

    public UIManager uiManager;
    // Start is called before the first frame update
    void Start()
    {
        DecisionPointOne = false;
        uiManager.ShowLocation("Rehearsal"); // here, UI manager is used to call a new location image 
        sam_test = new Dialouge();
        Debug.Log("arrived ");
        string[] sam_d1 = new string[] { "Hey its me Carter!", "I heard you started the band. Why didn't you invite me?", "END" };
        string Sam = "Carter";
        sam_test.sentences = sam_d1;
        sam_test.name = Sam;

        Debug.Log($"sam_test initialized: Name = {sam_test.name}, Sentences count = {sam_test.sentences.Length}");


        dialogueManager = FindAnyObjectByType<DialougeManager>();
        if (dialogueManager != null)
        {
            dialogueManager.StartDialouge(sam_test);
        }
        else
        {
            Debug.LogError("DialougeManager not found in the scene");
        }

       

    }

    void Update()
    {
        
        if( dialogueManager.sentences.Count == 0 && DecisionPointOne != true)
        {
            sam_test.name = "What should I do?";
            choiceBox.SetActive(true);
            dialouge_text.enabled = false;
            name_text.enabled = false;
            Continuebutton.gameObject.SetActive(false);
            choice1.gameObject.SetActive(true);
            choice2.gameObject.SetActive(true);
            DecisionPointOne = true;
        }
        
    }
}