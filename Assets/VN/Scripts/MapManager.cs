using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
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
        public string SceneTransition;
        public int week;
        public mapObject(Button btn, bool complete, bool unlocked, bool required, string name, int wk, string EventTxt, string sceneName)
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

    // Start is called before the first frame update
    void Start()
    {
        mainEventSelected = false;
        credits = 3;
        weekNum = 1;
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
        addEvent(School, false, true, true, "Library", 1, "Should I meet Carter at the School?", "W1D1");
        addEvent(Gym, false, true, false, "Gym", 1, "Should I train with Sam in the gym?", "W1D2");
        addEvent(Hanger, false, true, false, "Hanger", 1, "Should I meet Daylo at the hanger?", "W4D3");
        addEvent(Trees, false, true, false, "Trees", 1, "Should I meet Howard by the park?", "W1D3");
        addEvent(Tower, false, true, false, "Tower", 1, "Should I meet Carter at the tower?", "W4D2");
        addEvent(Islands, false, true, true, "Islands", 1, "Should I meet my rival at the Islands?", "W5D2");
    }

    void initWeekEvents()
    {
        //iterate through all events and populate weekly events which are unlocked
        foreach (var kvp in allEvents)
        {
            if (kvp.Value.week == weekNum && kvp.Value.isUnlocked == true)
            {
                weekEvents.Add(kvp.Key, kvp.Value);
            }
        }
    }
    void addEvent(Button location, bool comp, bool Unlock, bool require, string loc, int weeknum, string txtevent, string transitionName)
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

            credits -= 1;
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
