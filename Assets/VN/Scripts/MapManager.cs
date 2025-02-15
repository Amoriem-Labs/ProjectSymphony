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
    public Button School;
    public Button Gym;
    public Button Hanger;
    public Button Trees;
    public Button Tower;
    public Button Islands;





    //public Button TopRight;
    //public Button TopLeft;
    //public Button BotLeft;
    //public Button BotRight;
    bool mainEventSelected;
    int credits;

    //stored event items 
    public Dictionary<string, mapObject> allEvents;
    public Dictionary<string, mapObject> weekEvents;
    public Dictionary<string, mapObject> finishedEvents;

    // Start is called before the first frame update
    void Start()
    {
        mainEventSelected = false;
        credits = 2;
        weekNum = 1;
        allEvents = new Dictionary<string, mapObject>();
        weekEvents = new Dictionary<string, mapObject>();
        finishedEvents = new Dictionary<string, mapObject>();
        initializeEvents();

        School.GetComponentInChildren<Image>().enabled=(false);
        Gym.GetComponentInChildren<Image>().enabled =(false);
        Hanger.GetComponentInChildren<Image>().enabled= (false);
        Trees.GetComponentInChildren<Image>().enabled= (false);
        Tower.GetComponentInChildren<Image>().enabled= (false);
        Islands.GetComponentInChildren<Image>().enabled =(false);

        School.GetComponentInChildren<Image>().alphaHitTestMinimumThreshold = 0.1f;
        Gym.GetComponentInChildren<Image>().alphaHitTestMinimumThreshold = 0.1f;
        Hanger.GetComponentInChildren<Image>().alphaHitTestMinimumThreshold = 0.1f;
        Trees.GetComponentInChildren<Image>().alphaHitTestMinimumThreshold = 0.1f;
        Tower.GetComponentInChildren<Image>().alphaHitTestMinimumThreshold = 0.1f;
        Islands.GetComponentInChildren<Image>().alphaHitTestMinimumThreshold = 0.1f;


        School.onClick.AddListener(() => TrClicked(School));
        Gym.onClick.AddListener(() => TrClicked(Gym));
        Hanger.onClick.AddListener(() => TrClicked(Hanger));
        Trees.onClick.AddListener(() => TrClicked(Trees));
        Tower.onClick.AddListener(() => TrClicked(Tower));
        Islands.onClick.AddListener(() => TrClicked(Islands));


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
            if (kvp.Value.isUnlocked == true && kvp.Value.isComplete == false && credits != 0)
            {
                //kvp.Value.button.GetComponentInChildren<TMP_Text>().text = kvp.Value.locationName;
                kvp.Value.button.GetComponentInChildren<Image>().enabled = true;
            }
            else
            {
                //kvp.Value.button.gameObject.SetActive(false);
                kvp.Value.button.GetComponentInChildren<Image>().enabled = false;

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
        addEvent(School, false, true, true, "Library", 1, "","isW1D1" );
        //addEvent(Gym, false, true, false, "Gym", 1);
        //addEvent(Hanger, false, true, false, "Hanger", 1);
        //addEvent(Trees, false, true, false, "Trees", 1);
        //addEvent(Tower, false, true, false, "Tower", 1);
        //addEvent(Islands, false, true, true, "Islands", 1);

        //addEvent(TopLeft, false, true, false, "The Green", 1);
        //addEvent(BotLeft, false, true, true, "Fishing Pond", 1);
        //addEvent(BotRight, false, true, false, "Practice Room", 1);
        //addEvent(BotRight, false, true, false, "Hello", 2);
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
        mapObject obj = getObjFromButton(button);
        if (obj.isRequired == true)
        {
            mainEventSelected = true;
        }
        Debug.Log("Transition to " + obj.locationName);
        credits -= 1;
    }

    mapObject getObjFromButton(Button button)
    {
        foreach (var kvp in weekEvents)
        {
            if (kvp.Value.button == button)
            {
                kvp.Value.isComplete = true;
                return kvp.Value;
            }
        }
        return null;
    }
}
