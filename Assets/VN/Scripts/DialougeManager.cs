using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;
using UnityEngine.SceneManagement;



public class DialougeManager : MonoBehaviour
{

    // day activators
    public bool activeDialogue = false;
    public bool isDay0 = false;
    public bool isW1D1 = false;
    public bool isW1D2 = false;
    public bool isW1D3 = false;
    public bool isW2D1A = false;
    public bool isW2D1B = false;
    public bool isW2D2A = false;
    public bool isW2D2B = false;
    public bool isW2D3 = false;
    public bool isW2D4 = false;
    public bool isW2D5 = false;

    public bool isW3D1 = false;
    public bool isW3D2 = false;
    public bool isW3D3 = false;
    public bool isW3D4 = false;
    bool loadedEvents = false;


    // default name 
    public string inputtedName = "Chris";

    // Start is called before the first frame update
    private Dialouge loadedDialogue;
    private List<string> currentSentances;
    private List<string> currentOptions;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI dialougeText;

    public Button continueButton;

    public Animator animator;
    public Animator characterAnimator;

    public Queue<string> sentences;

    public Image characterSpriteImage;
    public Image backgroudSpriteImage;

    public List<Dialouge> dialogueSequence;
    public int currentDialogueIndex = 0;
    public bool isDialogueActive = false;
    public bool isEndofScene = false;
    public bool choiceSelected = false;
    public string selectedOption;

    // audio sources
    public AudioSource SFX;
    public AudioSource BGM;
    public AudioSource SecondaryBG;

    // buttons and choices
    public Button[] dialougueButtons;
    private List<Button> activeButtons = new List<Button>();

    private AnimationManager animationManager;

    private bool startedFade = false;
    private UnityEvent onFinishedDialogeLoading = new UnityEvent();
    // animations

    // UI Manager
    public GameObject UIManager;
    UIManager uiManager;
    private List<bool> SceneList;
    public Button saveScreen;
    void Awake()
    {

        SceneList = new List<bool>();
        loadedEvents = false; // DELETE THIS IF BUGSS

        animationManager = GetComponent<AnimationManager>();
        uiManager = UIManager.GetComponent<UIManager>();

        // create a new queue for the dialogue 
        sentences = new Queue<string>();
        Debug.Log("DialougeManager Awake method called, sentences queue initialized");

        // locate all dialouge managers 
        DialougeManager[] managers = FindObjectsOfType<DialougeManager>();

        //singleton patter to remove duplicate managers 
        if (managers.Length > 1)
        {
            Debug.LogError($"Found {managers.Length} DialougeManager instances in the scene. There should only be one.");

            for (int i = 1; i < managers.Length; i++)
            {
                Destroy(managers[i].gameObject);
            }
        }
        saveScreen.onClick.AddListener(SaveScreen);
        InitArr(SceneList);
        SetSceneBools();

    }



    private void InitArr(List<bool> arr)
    {
        arr.Add(isDay0);
        arr.Add(isW1D1);
        arr.Add(isW1D2);
        arr.Add(isW1D3);
        arr.Add(isW2D1A);
        arr.Add(isW2D1B);
        arr.Add(isW2D2A);
        arr.Add(isW2D2B);
        arr.Add(isW2D3);
        arr.Add(isW2D4);
        arr.Add(isW2D5);
        arr.Add(isW3D1);
        arr.Add(isW3D2);
        arr.Add(isW3D3);
        arr.Add(isW3D4);
    }
    public void SetSceneBools()
    {
        for (int i = 0; i < SceneList.Count; i++)
        {
            if (i == PlayerPrefs.GetInt("SceneIndex."))
            {
                SceneList[i] = true;
            }
            else
            {
                SceneList[i] = false;
            }
        }
        isDay0 = SceneList[0];
        isW1D1 = SceneList[1];
        isW1D2 = SceneList[2];
        isW1D3 = SceneList[3];
        isW2D1A = SceneList[4];
        isW2D1B = SceneList[5];
        isW2D2A = SceneList[6];
        isW2D2B = SceneList[7];
        isW2D3 = SceneList[8];
        isW2D4 = SceneList[9];
        isW2D5 = SceneList[10];
        isW3D1 = SceneList[11];
        isW3D2 = SceneList[12];
        isW3D3 = SceneList[13];
        isW3D4 = SceneList[14];
    }
    public void UpdatePPref(int index)
    {
        //int index = SceneList.IndexOf(sceneBool);
        PlayerPrefs.SetInt("SceneIndex.", index);
        SetSceneBools();
    }

    public void StartDialogueSequence()
    {
        if (dialogueSequence != null && dialogueSequence.Count > 0)
        {
            dialougeText.enabled = true;
            continueButton.gameObject.SetActive(true);
            currentDialogueIndex = 0;
            StartDialouge(dialogueSequence[currentDialogueIndex]);
            isDialogueActive = true;
        }
        else
        {
            Debug.LogError("No dialogue sequence loaded");
        }
    }


    public void StartDialouge(Dialouge curr_dialogue)
    {

        // spite changing


        if (!string.IsNullOrEmpty(curr_dialogue.sfx))
        {
            PlaySFX(curr_dialogue.sfx);
        }
        if (!string.IsNullOrEmpty(curr_dialogue.bgm))
        {
            PlayBGM(curr_dialogue.bgm);

        }

        if (!string.IsNullOrEmpty(curr_dialogue.bgm2))
        {
            Debug.Log("ENTERED");
            PlayBGM2(curr_dialogue.bgm2);
        }


        if (!string.IsNullOrEmpty(curr_dialogue.sprite) && characterSpriteImage != null)
        {
            Sprite newSprite = AssetCache.GetSprite($"Sprites/{curr_dialogue.sprite}");
            if (newSprite != null)
            {
                LeanTween.scale(characterSpriteImage.rectTransform, Vector3.zero, 0.5f)
               .setEase(LeanTweenType.easeInElastic)
               .setOnComplete(() =>
               {
                    // Change sprite after scaling down
                    characterSpriteImage.sprite = newSprite;

                    // Animate character sprite in
                    LeanTween.scale(characterSpriteImage.rectTransform, new Vector3(16f, 12.5f, 1f), 0.5f)
                       .setEase(LeanTweenType.easeOutElastic);
               });
                // characterSpriteImage.sprite = newSprite;
            }
            else
            {
                Debug.LogWarning($"Failed to load sprite: {curr_dialogue.sprite}");
            }
        }
        if (!string.IsNullOrEmpty(curr_dialogue.sprite) && backgroudSpriteImage != null)
        {
            if (curr_dialogue == null)
            {
                Debug.LogError("curr_dialogue is null in StartDialouge!");
                return;
            }

            Sprite newBG = Resources.Load<Sprite>($"Backgrounds/{curr_dialogue.background}");

            //Debug.Log("background is" + curr_dialogue.background);
            uiManager.ShowLocation(curr_dialogue.background);

            if (newBG != null)
            {
              backgroudSpriteImage.sprite = newBG;

                //// Fade out current background
                //LeanTween.alpha(backgroudSpriteImage.rectTransform, 0f, 0.5f).setOnComplete(() =>
                //{
                //    // Change background sprite after fade out
                //    backgroudSpriteImage.sprite = newBG;

                //    // Fade in the new background
                //    LeanTween.alpha(backgroudSpriteImage.rectTransform, 1f, 0.5f);
                //});
                // backgroudSpriteImage.sprite = newBG;
            }
            else
            {
                Debug.LogWarning($"Failed to load background: {curr_dialogue.background}");
            }
        }


        // error checking and set variables
        if (curr_dialogue == null)
        {
            //Debug.Log("Dialouge is null");
            return;
        }

        //Debug.Log($"Dialogue name: {curr_dialogue.name}, Sentences count: {curr_dialogue.sentences.Length}");

        // set the text animator 
        if (animator != null)
        {
            animator.SetBool("IsOpen", true);
        }
        else
        {
            Debug.LogWarning("Animator is not set in DialougeManager");
        }


        // set the name 
        if (nameText != null)
        {
            if (curr_dialogue.name != " PC" && curr_dialogue.name != "PC" && curr_dialogue.name != " MC" && curr_dialogue.name != "MC")
            {
                nameText.text = curr_dialogue.name;
            }
            else
            {
                nameText.text = inputtedName;
            }

        }
        else
        {
            Debug.LogWarning("nameText is not set in DialougeManager");
        }

        if (dialougeText == null)
        {
            Debug.LogError("dialougeText is null");
        }

        // reinitilize if null
        if (sentences == null)
        {
            Debug.LogError("sentences queue is null, reinitializing");
            sentences = new Queue<string>();
        }
        // clear the queue 
        sentences.Clear();

        // add the new dialouge
        foreach (string sentance in curr_dialogue.sentences)
        {
            // add to the queue
            string replacedSentence = sentance.Replace("PC", inputtedName).Replace(" PC", inputtedName);
            sentences.Enqueue(replacedSentence);
        }

        Debug.Log($"Enqueued {sentences.Count} sentences");

        DisplayNextSentence();
       

        // set the charcater animator
        if (characterAnimator != null)
        {
            characterAnimator.SetBool("DialougeActive", true);
        }
        else
        {
            Debug.LogWarning("Character animator is not set in DialougeManager");
        }
    }

    public void DisplayNextSentence()
    {

        if (sentences.Count > 0)
        {
            string current_sentence = sentences.Dequeue();
            // clear animations 
            StopAllCoroutines();
            StartCoroutine(TypeSentence(current_sentence));
        }
        else
        {
            if (currentDialogueIndex == dialogueSequence.Count - 1)
            {
                isEndofScene = true;
            }
            MoveToNextDialogueEntry();
        }
    }

    private void MoveToNextDialogueEntry()
    {
        choiceSelected = false;

        currentDialogueIndex++;
        if (currentDialogueIndex < dialogueSequence.Count)
        {
            Dialouge nextDialogue = dialogueSequence[currentDialogueIndex];
            if (nextDialogue.name == "Options:")
            {
                ShowChoiceOptions(nextDialogue.options);
            }
            else
            {
                dialougeText.enabled = true;
                continueButton.gameObject.SetActive(true);
                isDialogueActive = true;
                StartDialouge(nextDialogue);
            }
        }
        else
        {
            isEndofScene = true;
            //EndDialouge();
        }
    }
    private readonly WaitForSeconds letterDelay = new WaitForSeconds(0.01f);
    private readonly WaitForSeconds secondDelay = new WaitForSeconds(1.0f);
    IEnumerator TypeSentence(string sentence)
    {
        dialougeText.text = "";
        int visibleCharacters = 0;
        while (visibleCharacters <= sentence.Length)
        {
            dialougeText.text = sentence.Substring(0, visibleCharacters);
            visibleCharacters++;
            yield return letterDelay;
        }
    }


    //IEnumerator TypeSentence(string sentence)
    //{
    //    dialougeText.text = "";

    //    // <i> hid if < until > 
    //    for (int i = 0; i <= sentence.Length; i++)
    //    {
    //        dialougeText.text = sentence.Substring(0, i);
    //        yield return new WaitForSeconds(0.01f);
    //    }
    //}

    public void EndDialouge()
    {
        if (isEndofScene)
        {
            //animator.SetBool("IsOpen", false);
            characterAnimator.SetBool("DialougeActive", false);
        }
        isDialogueActive = false;
        activeDialogue = false;

    }

    [System.Serializable]
    private class DialogoueData
    {
        public string name;
        public string[] sentences;
    }



    public void LoadDialogueSequence(string dialogueFile)
    {
        TextAsset jsonDialogueFile = Resources.Load<TextAsset>
    ($"Dialogue/{dialogueFile}");


        if (jsonDialogueFile == null)
        {
            Debug.LogError($"Failed to load dialogue file: {dialogueFile}");
            return;
        }

        dialogueSequence = new List<Dialouge>();

        //Parse the dialogue
        string[] lines = jsonDialogueFile.text.Split(new[]
        { "\r\n", "\r", "\n" },
        System.StringSplitOptions.RemoveEmptyEntries);


        // get the data from the json file
        loadedDialogue = null;
        currentSentances = new List<string>();
        currentOptions = new List<string>();
        bool isparsingOptions = false;

        foreach (string line in lines)
        {
            if (line.StartsWith("Name: "))
            {
                AddCurrentDialogue();
                loadedDialogue = new Dialouge();
                loadedDialogue.name = line.Substring(6);
                currentSentances = new List<string>();
                currentOptions = new List<string>();
                isparsingOptions = false;

            }
            else if (line.StartsWith("Sprite: "))
            {
                if (loadedDialogue != null)
                {
                    loadedDialogue.sprite = line.Substring(8);
                }
            }
            else if (line == "Options: ")
            {
                AddCurrentDialogue();
                loadedDialogue = new Dialouge();
                loadedDialogue.name = "Options:";
                isparsingOptions = true;
            }
            else if (line.StartsWith("Animation: "))
            {
                animationManager.animationName = line.Substring(11);
                Debug.Log($"Playing animation {animationManager.animationName}");
                //animationManager.playAnimation = true;

            }
            else if (line.StartsWith("BG: "))
            {
                if (loadedDialogue != null)
                {
                    loadedDialogue.background = line.Substring(4);
                }
            }
            else if (line.StartsWith("BGM: "))
            {
                loadedDialogue.bgm = line.Substring(5);

            }
            else if (line.StartsWith("SFX: "))
            {
                Debug.Log("SFX loaded");
                loadedDialogue.sfx = line.Substring(5);

            }
            else if (line.StartsWith("BG2: "))
            {
                loadedDialogue.bgm2 = line.Substring(5);
                //string bgm2 = line.Substring(5);
                //Debug.Log($"Playing BGM2: {bgm2}");
                //AudioClip newBGM2 = Resources.Load<AudioClip>($"Audio/Music/SecondaryBGM/{bgm2}");
                //SecondaryBG.clip = newBGM2;
            }
            else if (!string.IsNullOrWhiteSpace(line))
            {
                if (isparsingOptions)
                {
                    currentOptions.Add(line);
                }
                else
                {
                    currentSentances.Add(line);
                }

            }
        }
        AddCurrentDialogue();

        Debug.Log($"Loaded {dialogueSequence.Count} dialogue entries");
        
    }

    private void AddCurrentDialogue()
    {
        if (loadedDialogue != null)
        {
            loadedDialogue.sentences = currentSentances.ToArray();
            if (currentOptions.Count > 0)
            {
                loadedDialogue.options = currentOptions.ToArray();
            }
            dialogueSequence.Add(loadedDialogue);
        }
        loadedEvents = true;
    }

    public void ShowChoiceOptions(string[] options)
    {
        dialougeText.enabled = false;
        continueButton.gameObject.SetActive(false);
        nameText.text = "What should I do?";

        Debug.Log($"ShowChoiceOptions called with {options?.Length ?? 0} options");

        if (dialougueButtons == null || options == null)
        {
            Debug.LogError("Options or buttons were found null");
            return;
        }

        foreach (Button button in dialougueButtons)
        {
            button.gameObject.SetActive(false);
        }

        for (int i = 0; i < options.Length && i < dialougueButtons.Length; i++)
        {
            Button button = dialougueButtons[i];

            if (button != null)
            {
                button.gameObject.SetActive(true);

                TextMeshProUGUI buttonText = button.GetComponentInChildren<TextMeshProUGUI>();

                if (buttonText != null)
                {
                    buttonText.text = options[i];
                }
                else
                {
                    Debug.LogWarning($"Button {i} is missing text component");
                }

                int optionIndex = i; // index for selection

                button.onClick.RemoveAllListeners();
                button.onClick.AddListener(() => HandleOptionSelected(optionIndex));

                activeButtons.Add(button);

            }
            else
            {
                Debug.LogWarning($"Button at index {i} is null");
            }

        }
    }

    private void HandleOptionSelected(int optionIndex)
    {
        Debug.Log($"Option {optionIndex} selected");
        ClearActiveButtons();

        selectedOption = dialogueSequence[currentDialogueIndex].options[optionIndex];
        Debug.Log($"Selected {selectedOption}");
        //currentDialogueIndex = 0;
        choiceSelected = true;
        MoveToNextDialogueEntry();
    }

    private void ClearActiveButtons()
    {
        foreach (Button button in activeButtons)
        {
            button.gameObject.SetActive(false);
        }
        activeButtons.Clear();
    }

    private void PlaySFX(string sfxName)
    {
        SFX.Stop();

        AudioClip clip = AssetCache.GetAudioClip($"Audio/SFX/{sfxName}"); if (clip != null)
        {
            Debug.Log($"Playing {clip}");
            SFX.PlayOneShot(clip);
        }
    }

    private void PlayBGM(string bgmName)
    {
        AudioClip clip = Resources.Load<AudioClip>($"Audio/Music/BGM/{bgmName}");
        if (clip != null)
        {
            BGM.clip = clip;
            Debug.Log($"Playing {clip}");
            BGM.Play();
        }
    }


    private void PlayBGM2(string bgm2Name)
    {
        AudioClip clip = Resources.Load<AudioClip>($"Audio/Music/SecondaryBGM/{bgm2Name}");
        if (clip != null)
        {
            SecondaryBG.clip = clip;
            Debug.Log($"Playing {clip}");
            SecondaryBG.Play();
        }
    }

    public IEnumerator LoadAndStartDialoguesSequentially(string[] dialogueFiles)
    {
        activeDialogue = true;
        for (int i = 0; i < dialogueFiles.Length; i++)
        {
            LoadDialogueSequence(dialogueFiles[i]);
            //yield return new WaitForSeconds(4f);
            yield return new WaitUntil(() => loadedEvents);
            yield return new WaitUntil(() => uiManager.IsReady);

            //yield return new WaitForSeconds(1f);
            loadedEvents = false; // DELETE THIS IF BUGSS
            StartDialogueSequence();
            yield return new WaitUntil(() => currentDialogueIndex >= dialogueSequence.Count);

            if (i < dialogueFiles.Length - 1)
            {
                currentDialogueIndex = 0;
                isEndofScene = false;
                isDialogueActive = false;
            }
            else
            {
                isEndofScene = true;
                EndDialouge();
            }

            yield return secondDelay;
        }
        activeDialogue = false;
    }


    public void SaveScreen()
    {
        SceneManager.LoadScene("SaveScreen");
    }


}

