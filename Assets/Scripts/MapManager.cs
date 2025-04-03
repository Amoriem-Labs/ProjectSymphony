using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class MapManager : MonoBehaviour
{
    [System.Serializable]
    public class mapObject
    {

        public Button button;
        public bool isUnlocked;
        public bool isComplete;
        public bool isRequired;
        public string locationName;
        public string EventText;
        public int SceneTransition;
        public int week;
        public mapObject(Button btn, bool complete, bool unlocked, bool required, string name, int wk, string EventTxt, int sceneName)
        {
            button = btn;
            isComplete = complete;
            isUnlocked = unlocked;
            isRequired = required;
            locationName = name;
            EventText = EventTxt;
            SceneTransition = sceneName;

            week = wk;
        }
    }

    public int weekNum;
    public TextMeshProUGUI Creds;
    public Canvas loadingScreen; 
    public Canvas YesNoUI;
    public Button Yes;
    public Button No;
    public Button School;
    public Button Gym;
    public Button Hanger;
    public Button Trees;
    public Button Tower;
    public Button Islands;
    Button lastButtonClicked;
    bool mainEventSelected;
    bool YesNoClickable = false;
    public TextMeshProUGUI DecisionText;
    int credits;

    //stored event items 
    public Dictionary<string, mapObject> allEvents;
    public Dictionary<string, mapObject> weekEvents;
    public Dictionary<string, mapObject> finishedEvents;


    private LoadingScreens LS;


    // Start is called before the first frame update
    private void Awake()
    {
        LS = FindAnyObjectByType<LoadingScreens>();
        // locate all dialouge managers 
        MapManager[] managers = FindObjectsOfType<MapManager>();

        //singleton pattern to remove duplicate managers 
        if (managers.Length > 1)
        {
            Debug.LogError($"Found {managers.Length} Map Manager instances in the scene. There should only be one.");

            for (int i = 1; i < managers.Length; i++)
            {
                Destroy(managers[i].gameObject);
            }
        }
    }
    //private DialougeManager dialogueManager;

    void Start()
    {
        //dialogueManager = FindAnyObjectByType<DialougeManager>();
        mainEventSelected = false;
        credits = 3;
        PlayerPrefs.SetInt("CurrentWeek.", 2);
        if (PlayerPrefs.HasKey("CurrentWeek."))
        {
            weekNum = PlayerPrefs.GetInt("CurrentWeek.");

        }
        else
        {
            weekNum = 1;

        }

        //weekNum = 2;
        allEvents = new Dictionary<string, mapObject>();
        weekEvents = new Dictionary<string, mapObject>();
        finishedEvents = new Dictionary<string, mapObject>();
        initializeEvents();
        Image SchoolIm = School.GetComponentInChildren<Image>();
        Image GymIm = Gym.GetComponentInChildren<Image>();
        Image HangerIm = Hanger.GetComponentInChildren<Image>();
        Image TreesIm = Trees.GetComponentInChildren<Image>();
        Image TowerIm = Tower.GetComponentInChildren<Image>();
        Image IslandsIm = Islands.GetComponentInChildren<Image>();
        Image YesIm = Yes.GetComponentInChildren<Image>();
        Image NoIm = No.GetComponentInChildren<Image>();

        SchoolIm.enabled = (true);
        GymIm.enabled = (true);
        HangerIm.enabled = (true);
        TreesIm.enabled = (true);
        TowerIm.enabled = (true);
        IslandsIm.enabled = (true);

        SchoolIm.alphaHitTestMinimumThreshold = 0.1f;
        GymIm.alphaHitTestMinimumThreshold = 0.1f;
        HangerIm.alphaHitTestMinimumThreshold = 0.1f;
        TreesIm.alphaHitTestMinimumThreshold = 0.1f;
        TowerIm.alphaHitTestMinimumThreshold = 0.1f;
        IslandsIm.alphaHitTestMinimumThreshold = 0.1f;
        YesIm.alphaHitTestMinimumThreshold = 0.1f;
        NoIm.alphaHitTestMinimumThreshold = 0.1f;

        School.onClick.AddListener(() => TrClicked(School));
        Gym.onClick.AddListener(() => TrClicked(Gym));
        Hanger.onClick.AddListener(() => TrClicked(Hanger));
        Trees.onClick.AddListener(() => TrClicked(Trees));
        Tower.onClick.AddListener(() => TrClicked(Tower));
        Islands.onClick.AddListener(() => TrClicked(Islands));
        Yes.onClick.AddListener(() => YesNoClicked(Yes));
        No.onClick.AddListener(() => YesNoClicked(No));
        YesNoUI.enabled = false;
        loadingScreen.enabled = false;

    }

    void Update()
    {
        
        Creds.text = "Credits: " + credits;
        if (credits == 1 && mainEventSelected == false)
        {
            nullUnrequiredEvents();
        }
        showEventButtons();

    }

    //Helper functions
    void showEventButtons()
    {
        //School.interactable = false;
        //Gym.interactable = false;
        //Hanger.interactable = false;
        //Trees.interactable = false;
        //Tower.interactable = false;
        //Islands.interactable = false;

        foreach (var kvp in weekEvents)
        {
            kvp.Value.button.GetComponentInChildren<Image>().enabled = true;

            if (!YesNoClickable)
            {
                ButtonSetInteractable(School);
                ButtonSetInteractable(Gym);
                ButtonSetInteractable(Hanger);
                ButtonSetInteractable(Trees);
                ButtonSetInteractable(Tower);
                ButtonSetInteractable(Islands);
            }
            
        }
    }
    void nullUnrequiredEvents()
    {
        foreach (var kvp in weekEvents)
        {
            if (kvp.Value.isRequired == false && kvp.Value.isUnlocked == true)
            {
                kvp.Value.isUnlocked = false;
            }
        }
    }
    void initializeEvents()
    {
        initAllEvents();
        initWeekEvents();
    }

    void initAllEvents()
    {
        //void addEvent(Button location, complete?, unlocked?, required?, string loc, int weeknum)
        addEvent(School, false, true, true, "Library", 1, "Should I meet Carter at the School?", 1);
        addEvent(Gym, false, true, false, "Gym", 1, "Should I train with Sam in the gym?", 2);
        addEvent(Hanger, false, true, false, "Hanger", 1, "Should I meet Daylo at the hanger?", 3);
        addEvent(Trees, false, true, false, "Trees", 1, "Should I meet Howard by the park?", 6);
        addEvent(Tower, false, true, false, "Tower", 1, "Should I meet Carter at the tower?", 5);
        addEvent(Islands, false, true, true, "Islands", 1, "Should I meet my rival at the Islands?", 6);
        addEvent(Trees, false, true, true, "Trees2", 2, "Should I help Sam with her event?", 4);
        addEvent(Islands, false, true, true, "Islands2", 2, "Should I help Howard watch Ina?", 5);
        addEvent(Trees, false, true, true, "Trees3", 21, "Should I meet  Howard at the park?", 6);
        addEvent(Islands, false, true, true, "Islands3", 21, "Should I meet Sam at the lake?", 7);


    }

    void initWeekEvents()
    {
        //iterate through all events and populate weekly events which are unlocked
        foreach (var kvp in allEvents)
        {
            if (kvp.Value.week == weekNum && kvp.Value.isUnlocked == true)
            {
                weekEvents.Add(kvp.Key, kvp.Value);
                Debug.Log("Added event for week " + weekNum + ": " + kvp.Key);
            }
        }
    }
    void addEvent(Button location, bool comp, bool Unlock, bool require, string loc, int weeknum, string txtevent, int transitionName)
    {
        if (allEvents.ContainsKey(loc))
        {
            Debug.LogWarning($"Event for location '{loc}' already exists. Not adding duplicate.");
            return;
        }

        mapObject ob = new mapObject(location, comp, Unlock, require, loc, weeknum, txtevent, transitionName);
        allEvents.Add(loc, ob);

    }

    void markCompleteEvents()
    {
        foreach (var kvp in weekEvents)
        {
            if (kvp.Value.isComplete == true)
            {
                finishedEvents.Add(kvp.Key, kvp.Value);
            }
        }
    }

    private void TrClicked(Button button)
    {

        YesNoClickable = true;
        //enable the ui and enable yes no clicked
        YesNoUI.enabled = true;
        lastButtonClicked = button;
        mapObject obj = getObjFromButton(lastButtonClicked);
        if (obj == null)
        {
            Debug.Log("Error null instance of obj\n");
            return;
        }
        DecisionText.text = obj.EventText;

    }

    private void YesNoClicked(Button button)
    {
        mapObject obj = getObjFromButton(lastButtonClicked);
        if (obj == null)
        {
            Debug.Log("Error null instance of obj\n");
            return;
        }

        DecisionText.text = obj.EventText;
        if (button == Yes)
        {
            obj.isComplete = true;
            if (obj.isRequired == true)
            {
                mainEventSelected = true;
            }

            Debug.Log("Transition to " + obj.SceneTransition);
            //dialogueManager.UpdatePPref(obj.SceneTransition);
            PlayerPrefs.SetInt("SceneIndex.", obj.SceneTransition);
            credits -= 1;
            PlayerPrefs.SetInt("SceneToLoad", 0);
            LS.loaded = false;
            loadingScreen.enabled = true;
            //SceneManager.LoadScene("Splash");        
        }
        else if (button == No)
        {
            // Add No button logic here
            Debug.Log("No button clicked. Cancelling action.");
        }
        YesNoClickable = false;
            YesNoUI.enabled = false;
    }

    mapObject getObjFromButton(Button button)
    {
        foreach (var kvp in weekEvents)
        {
            if (kvp.Value.button == button)
            {
                return kvp.Value;
            }
        }
        return null;
    }
    void ButtonSetInteractable(Button button)
    {
        bool detected = false;
        if (credits != 0)
        {
            foreach (var kvp in weekEvents)
            {
                if (button == kvp.Value.button)
                {
                    detected = true;
                    if (kvp.Value.isComplete == false && kvp.Value.isUnlocked == true)
                    {
                        kvp.Value.button.interactable = true;
                    }
                    else
                    {
                        kvp.Value.button.interactable = false;
                    }
                }

            }
            if(detected == false)
            {
                button.interactable = false;
            }
        }
        else
        {
            button.interactable = false;
        }

    }
    //void TransitionScenes(mapObject event)
    //{


    //}
}
