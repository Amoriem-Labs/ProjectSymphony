using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.IO;
using UnityEngine.SceneManagement;

public class Day0 : MonoBehaviour
{

    // MACROS 
    private const string CURRENT_DAY_KEY = "CurrentDay.";
    private const string CURRENT_WEEK_KEY = "CurrentWeek.";
    // week 1-3 affection keys 
    private const string HOWARD_AFFECTION_KEY = "HowardAffection.";
    private const string SAM_AFFECTION_KEY = "SamAffection.";
    private const string DAYLO_AFFECTION_KEY = "DayloAffection.";
    private const string CARTER_AFFECTION_KEY = "CarterAffection.";
    private const string PAWLINE_AFFECTION_KEY = "PawlineAffection.";


    // Dialogue Variables

    private Dialouge currDialogue;

    public TextMeshProUGUI name_text;
    public TextMeshProUGUI dialouge_text;
    private DialougeManager dialogueManager;
    private bool DP0_1;
    public TMP_InputField playerNameReader;
    private AffectionManager affectionManager;

    public GameObject dialoguebox;

    // Start is called before the first frame update
    void Start()
    {
       
        affectionManager = FindAnyObjectByType<AffectionManager>();
        affectionManager.InitPPrefs();
        DP0_1 = false;
        playerNameReader.gameObject.SetActive(false);

        // get dialoguemanager and verify
        dialogueManager = FindAnyObjectByType<DialougeManager>();

        if (dialogueManager == null)
        {
            Debug.LogError("DialougeManager not found in the scene");
            return;
        }
       
        //if (dialogueManager.isDay0 == false)
        //{
        //    Debug.Log("!!!!!!!xDAY 0 FALSE AT STARTUP");
        //    dialogueManager.SetSceneBools();
        //}
        //else
        //{
            StartCoroutine(dialogueManager.LoadAndStartDialoguesSequentially(new string[] { "W1D0.0" }));
        //}

    }

    // choices and decision points in update
    void Update()
    {
        if (dialogueManager.isDay0)
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

                    dialoguebox.SetActive(false);

                    LeanTween.scale(dialoguebox.gameObject.GetComponent<RectTransform>(), Vector3.zero, 0.3f)
                        .setEase(LeanTweenType.easeOutCubic);

                    playerNameReader.gameObject.GetComponent<RectTransform>().localScale = Vector3.zero;

                    // Activate it
                    playerNameReader.gameObject.SetActive(true);

                    // Tween scale to 1 with easeOutCubic
                    LeanTween.scale(playerNameReader.gameObject.GetComponent<RectTransform>(), Vector3.one, 0.3f)
                        .setEase(LeanTweenType.easeOutCubic);
                }
                else if (currselectedOption == "No.")
                {
                    StartCoroutine(dialogueManager.LoadAndStartDialoguesSequentially(new string[] { "W1D0.1a" }));

                    GameStateManager.Instance.LoadNewScene("TitleScene");
                }


            }
        }

    }
}