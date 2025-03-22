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

    private DialougeManager dialogueManager;
    private MapManager mapManager;
    public GameObject saveSlotPanel;
    public GameObject saveSlotButtonPrefab;
    public Button SaveButton;
    //public Button Delete;
    public int saveSlotIndex = 0;
    public int saveSlotCount = 0;
    // Start is called before the first frame update
    void Start()
    {
        PlayerPrefs.DeleteAll();
        // 0 is VN
        // 1 is map
        // 2 is Rhythm
        PlayerPrefs.SetInt("SceneToLoad", 0);
        dialogueManager = FindAnyObjectByType<DialougeManager>();
        mapManager = FindAnyObjectByType<MapManager>();
        DisplaySaves();
        PlayerPrefs.SetInt(SLOT_INDEX_KEY, 1);
        SaveButton.onClick.AddListener(() => SaveGame(PlayerPrefs.GetInt(SLOT_INDEX_KEY)));
        DisplaySaves();
    }

    // Update is called once per frame
    void Update()
    {

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

        PlayerPrefs.SetString(PLAYER_NAME_KEY + saveName.ToString(), "Yow");
        Debug.Log($"Save Count: {saveSlotCount}");
        PlayerPrefs.SetInt("Index" + saveName.ToString(), saveSlotCount);
        PlayerPrefs.SetInt(SLOT_INDEX_KEY, PlayerPrefs.GetInt(SLOT_INDEX_KEY) + 1);
        PlayerPrefs.Save();
        DisplaySaves();

    }

    public void LoadData(int saveName)
    {
        Debug.Log("data loaded " + saveName.ToString());
        //dialogueManager.inputtedName = PlayerPrefs.GetString(PLAYER_NAME_KEY + saveName);
        saveSlotIndex = PlayerPrefs.GetInt("Index" + saveName.ToString());

        SceneManager.LoadScene("Splash");
    }

    public void DeleteData(int saveName)
    {
        PlayerPrefs.DeleteKey(PLAYER_NAME_KEY + saveName);
        PlayerPrefs.DeleteKey(SLOT_NAME_KEY + saveName);

    }

}

