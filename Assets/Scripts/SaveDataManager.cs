using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class SaveDataManager : MonoBehaviour
{
    private const string PLAYER_NAME_KEY = "PlayerName.";
    private const string SLOT_NAME_KEY = "SlotName.";
    private const string SLOT_INDEX_KEY = "SlotIndex";
    private const string CURRENT_DAY_KEY = "CurrentDay.";
    private const string CURRENT_WEEK_KEY = "CurrentWeek.";
    // week 1-3 affection keys 
    private const string HOWARD_AFFECTION_KEY = "HowardAffection.";
    private const string SAM_AFFECTION_KEY = "SamAffection.";
    private const string DAYLO_AFFECTION_KEY = "DayloAffection.";
    private const string CARTER_AFFECTION_KEY = "CarterAffection.";
    private const string PAULINE_AFFECTION_KEY = "PaulineAffection.";
    private const string SCENE_INDEX_KEY = "SceneIndex.";
    private const string HOWARD_UNLOCKED_KEY = "HowardUnlocked.";
    private const string SAM_UNLOCKED_KEY = "SamUnlocked.";
    private const string CARTER_UNLOCKED_KEY = "CarterUnlocked.";
    private const string PAULINE_UNLOCKED_KEY = "PaulineUnlocked.";
    private const string DAYLO_UNLOCKED_KEY = "DayloUnlocked.";

    private DialougeManager dialogueManager;
    private MapManager mapManager;
    private AffectionManager affectionManager;
    public GameObject saveSlotPanel;
    public GameObject saveSlotButtonPrefab;
    public Button SaveButton;
    public Canvas LoadingScreen;
    private LoadingScreens LS;
    //public Button SaveButtonVN;

    //public Button Delete;
    public int saveSlotIndex = 0;
    public int saveSlotCount = 0;

    // Start is called before the first frame update
    void Start()
    {
        affectionManager = FindAnyObjectByType<AffectionManager>();
        LoadingScreen.enabled = false;
        LS = FindAnyObjectByType<LoadingScreens>();
        PlayerPrefs.SetInt(CURRENT_WEEK_KEY, 1);
        PlayerPrefs.SetInt("SceneToLoad", 0);
        dialogueManager = FindAnyObjectByType<DialougeManager>();
        mapManager = FindAnyObjectByType<MapManager>();

        if (!PlayerPrefs.HasKey(SLOT_INDEX_KEY))
        {
            PlayerPrefs.SetInt(SLOT_INDEX_KEY, 1);

        }
        if (!PlayerPrefs.HasKey(SCENE_INDEX_KEY))
        {
            Debug.Log("SCEENE INDEX REPLACED");
            PlayerPrefs.SetInt(SCENE_INDEX_KEY, 0);

        }

        if (!PlayerPrefs.HasKey(HOWARD_AFFECTION_KEY) || !PlayerPrefs.HasKey(HOWARD_UNLOCKED_KEY))
        {
            Debug.Log("setting values??");
            PlayerPrefs.SetInt(HOWARD_AFFECTION_KEY, 0);
            PlayerPrefs.SetInt(SAM_AFFECTION_KEY, 0);
            PlayerPrefs.SetInt(CARTER_AFFECTION_KEY, 0);
            PlayerPrefs.SetInt(DAYLO_AFFECTION_KEY, 0);
            PlayerPrefs.SetInt(PAULINE_AFFECTION_KEY, 0);
            PlayerPrefs.SetInt(HOWARD_UNLOCKED_KEY, 1);
            PlayerPrefs.SetInt(SAM_UNLOCKED_KEY, 1);
            PlayerPrefs.SetInt(CARTER_UNLOCKED_KEY, 0);
            PlayerPrefs.SetInt(DAYLO_UNLOCKED_KEY, 0);
            PlayerPrefs.SetInt(PAULINE_UNLOCKED_KEY, 0);
            affectionManager.UpdateGsmAffection();
        }
        else
        {
            Debug.Log("not setting values??");
        }

        SaveButton.onClick.AddListener(() => SaveGame(PlayerPrefs.GetInt(SLOT_INDEX_KEY)));
        //SaveButtonVN.onClick.AddListener(() => SaveGame(PlayerPrefs.GetInt(SLOT_INDEX_KEY)));

        DisplaySaves();
    }

    void DisplaySaves()
    {
        // Clear any existing save slot buttons
        foreach (Transform child in saveSlotPanel.transform)
        {
            Destroy(child.gameObject);
        }

        // Load existing save slots
        int slotIndex = 1;
        Debug.Log("increment");
        bool incremented = false;
        while (PlayerPrefs.HasKey(SLOT_NAME_KEY + slotIndex))
        {
            CreateSaveSlotButton(slotIndex);
            slotIndex++;
            if (!incremented)
            {
                saveSlotCount++;
                incremented = true;
            }

        }
        incremented = false;

        // If no saves exist, create a default button
        if (saveSlotCount == 0)
        {
            CreateSaveSlotButton(1);
            saveSlotCount++;
        }

        Debug.Log(saveSlotCount);

    }

    void CreateSaveSlotButton(int slotIndex)
    {
        var saveSlotButton = Instantiate(saveSlotButtonPrefab, saveSlotPanel.transform);
        saveSlotButton.transform.SetAsFirstSibling();
        var slotText = saveSlotButton.GetComponentInChildren<TMP_Text>();
        if (slotText != null)
        {
            slotText.text = $"Slot {slotIndex} - " + (PlayerPrefs.HasKey(SLOT_NAME_KEY + slotIndex) ? "Saved" : "Empty");
        }
        else
        {
            Debug.LogError("TMP_Text component not found on the prefab!");
        }

        // Add a click handler to load the save when clicked
        var button = saveSlotButton.GetComponent<Button>();
        int currIndex = PlayerPrefs.GetInt(SLOT_INDEX_KEY);
        button.onClick.AddListener(() => LoadData(slotIndex));
    }


    public void SaveGame(int saveName)
    {

        //saveSlotCount += 1;
        Debug.Log($"Save Name: {saveName}");
        //saveName
        Debug.Log($"Slot index key: {PlayerPrefs.GetInt(SLOT_INDEX_KEY)}");
        PlayerPrefs.SetInt(SLOT_NAME_KEY + saveName.ToString(), PlayerPrefs.GetInt(SLOT_INDEX_KEY));

        PlayerPrefs.SetString(PLAYER_NAME_KEY + saveName.ToString(), PlayerPrefs.GetString(PLAYER_NAME_KEY));
        Debug.Log($"Save Count: {saveSlotCount}");
        PlayerPrefs.SetInt("Index" + saveName.ToString(), saveSlotCount);
        PlayerPrefs.SetInt(SLOT_INDEX_KEY, PlayerPrefs.GetInt(SLOT_INDEX_KEY) + 1);
        Debug.Log("Scene index is: " + PlayerPrefs.GetInt(SCENE_INDEX_KEY));
        PlayerPrefs.SetInt(SCENE_INDEX_KEY + saveName.ToString(), PlayerPrefs.GetInt(SCENE_INDEX_KEY));
        //dialogueManager.SetSceneBools();
        //affectionManager.UpdatePlayerPrefAffections();
        PlayerPrefs.SetInt(HOWARD_AFFECTION_KEY + saveName.ToString(), PlayerPrefs.GetInt(HOWARD_AFFECTION_KEY));
        PlayerPrefs.SetInt(SAM_AFFECTION_KEY + saveName.ToString(), PlayerPrefs.GetInt(SAM_AFFECTION_KEY));
        PlayerPrefs.SetInt(CARTER_AFFECTION_KEY + saveName.ToString(), PlayerPrefs.GetInt(CARTER_AFFECTION_KEY));
        PlayerPrefs.SetInt(DAYLO_AFFECTION_KEY + saveName.ToString(), PlayerPrefs.GetInt(DAYLO_AFFECTION_KEY));
        PlayerPrefs.SetInt(PAULINE_AFFECTION_KEY + saveName.ToString(), PlayerPrefs.GetInt(PAULINE_AFFECTION_KEY));
        PlayerPrefs.SetInt(HOWARD_UNLOCKED_KEY + saveName.ToString(), PlayerPrefs.GetInt(HOWARD_UNLOCKED_KEY));
        PlayerPrefs.SetInt(SAM_UNLOCKED_KEY + saveName.ToString(), PlayerPrefs.GetInt(SAM_UNLOCKED_KEY));
        PlayerPrefs.SetInt(CARTER_UNLOCKED_KEY + saveName.ToString(), PlayerPrefs.GetInt(CARTER_UNLOCKED_KEY));
        PlayerPrefs.SetInt(DAYLO_UNLOCKED_KEY + saveName.ToString(), PlayerPrefs.GetInt(DAYLO_UNLOCKED_KEY));
        PlayerPrefs.SetInt(PAULINE_UNLOCKED_KEY + saveName.ToString(), PlayerPrefs.GetInt(PAULINE_UNLOCKED_KEY));


        PlayerPrefs.Save();
        DisplaySaves();

    }

    public void LoadData(int saveName)
    {

        Debug.Log("data loaded " + saveName.ToString());
        saveSlotIndex = PlayerPrefs.GetInt("Index" + saveName.ToString());
        PlayerPrefs.SetInt(SCENE_INDEX_KEY, PlayerPrefs.GetInt(SCENE_INDEX_KEY + saveName.ToString()));
        if (PlayerPrefs.GetInt(SCENE_INDEX_KEY) >= 1)
        {
            PlayerPrefs.SetString(PLAYER_NAME_KEY, PlayerPrefs.GetString(PLAYER_NAME_KEY + saveName));

            //dialogueManager.inputtedName = PlayerPrefs.GetString(PLAYER_NAME_KEY + saveName);

        }
        Debug.Log("Loading:   Scene index is: " + PlayerPrefs.GetInt(SCENE_INDEX_KEY));
        //SceneManager.LoadScene("Splash");
        PlayerPrefs.SetInt(HOWARD_AFFECTION_KEY, PlayerPrefs.GetInt(HOWARD_AFFECTION_KEY + saveName.ToString()));
        PlayerPrefs.SetInt(SAM_AFFECTION_KEY, PlayerPrefs.GetInt(SAM_AFFECTION_KEY + saveName.ToString()));
        PlayerPrefs.SetInt(CARTER_AFFECTION_KEY, PlayerPrefs.GetInt(CARTER_AFFECTION_KEY + saveName.ToString()));
        PlayerPrefs.SetInt(DAYLO_AFFECTION_KEY, PlayerPrefs.GetInt(DAYLO_AFFECTION_KEY + saveName.ToString()));
        PlayerPrefs.SetInt(PAULINE_AFFECTION_KEY, PlayerPrefs.GetInt(PAULINE_AFFECTION_KEY + saveName.ToString()));
        PlayerPrefs.SetInt(HOWARD_UNLOCKED_KEY, PlayerPrefs.GetInt(HOWARD_UNLOCKED_KEY + saveName.ToString()));
        PlayerPrefs.SetInt(SAM_UNLOCKED_KEY, PlayerPrefs.GetInt(SAM_UNLOCKED_KEY + saveName.ToString()));
        PlayerPrefs.SetInt(CARTER_UNLOCKED_KEY, PlayerPrefs.GetInt(CARTER_UNLOCKED_KEY + saveName.ToString()));
        PlayerPrefs.SetInt(DAYLO_UNLOCKED_KEY, PlayerPrefs.GetInt(DAYLO_UNLOCKED_KEY + saveName.ToString()));
        PlayerPrefs.SetInt(PAULINE_UNLOCKED_KEY, PlayerPrefs.GetInt(PAULINE_UNLOCKED_KEY + saveName.ToString()));
        affectionManager.UpdateGsmAffection();

        LoadingScreen.enabled = true;
        LS.loaded = false;

    }

    public void DeleteData(int saveName)
    {
        PlayerPrefs.DeleteKey(PLAYER_NAME_KEY + saveName);
        PlayerPrefs.DeleteKey(SLOT_NAME_KEY + saveName);

    }
    private void Update()
    {

        if (Input.GetKeyDown(KeyCode.D))
        {
            DeleteAllPlayerPrefs();
        }
    }
    void DeleteAllPlayerPrefs()
    {
        PlayerPrefs.DeleteAll();

        Debug.Log("All PlayerPrefs have been deleted.");
        PlayerPrefs.SetInt(SLOT_INDEX_KEY, 1);
        PlayerPrefs.SetInt(SCENE_INDEX_KEY, 0);
        PlayerPrefs.SetInt("SceneToLoad", 0);
        PlayerPrefs.Save();
        DisplaySaves();
    }

    public void ExitScreen()
    {
        GameStateManager.Instance.LoadNewScene("TitleScene");
    }

}

