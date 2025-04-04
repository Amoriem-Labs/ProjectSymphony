using System.Collections;
using System.Collections.Generic;
using System.Linq;
using RhythMidi;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum CharacterRole
{
    None = 0,
    Melodist = 1,
    Counter = 2,
    Percussion = 3,
    Harmony = 4
};

[System.Serializable]
public class Character
{
    public CharacterRole role;
    public string name;
    public string instrument;

    public string bio; // PLACEHOLDER: can remove if needed
    public Sprite spriteUnlocked;
    public Sprite spriteLocked;
    public Sprite backgroundHero;
    public int midiPitchStart;
}

public class CharacterData
{
    public Character character;
    public float affection;
    public bool isUnlocked;

    public CharacterData(Character character, float affection, bool isUnlocked)
    {
        this.character = character;
        this.affection = affection;
        this.isUnlocked = isUnlocked;
    }
}

public class GameStateManager : MonoBehaviour
{
    public static GameStateManager Instance { get; private set; }
    public Character[] characters;
    public Dictionary<Character, CharacterData> characterData;
    public ChartResource CurrentChart => RhythMidiController.Instance.GetChartByName(currentSongName);
    public string currentSongName;
    public CharacterData[] selectedCharacters;

    public bool SelectedCharactersContainsRole(CharacterRole role) => selectedCharacters.Any(c => c.character.role == role);
    public CharacterData GetSelectedCharacterWithRole(CharacterRole role) => selectedCharacters.First(c => c.character.role == role);

    void Start()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        LoadPersistentData();
    }
    public void LoadPersistentData()
    {
        // TODO: Load persistent data from file/PlayerPrefs/whatever
        characterData = new Dictionary<Character, CharacterData>();
        foreach (Character c in characters)
        {
            CharacterData cd = new CharacterData(c, 100f, true);
            characterData.Add(c, cd);
        }
    }
    public void SavePersistentData()
    {
        // TODO
    }
    public void LoadCharacterSelect(string song)
    {
        currentSongName = song;
        SceneManager.LoadScene("PartyScreen");
    }


    public void LoadVN()
    {
        //System.GC.Collect();
        StartCoroutine(LoadSceneAsync("VN"));

        //SceneManager.LoadScene("VN");
        Time.timeScale = 1; // Add this line
    }
    IEnumerator LoadSceneAsync(string sceneName)
    {
        
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
        asyncLoad.allowSceneActivation = false;

        while (!asyncLoad.isDone)
        {
            // Wait until the scene is nearly loaded
            if (asyncLoad.progress >= 0.9f)
            {
                asyncLoad.allowSceneActivation = true;
            }
            yield return null;
        }
    }

    public void StartRhythmGame(CharacterData[] selectedCharacters)
    {
        this.selectedCharacters = selectedCharacters;
        RhythMidiController.Instance.ClearCallbacks();
        SceneManager.LoadScene("RhythmGame");
    }


 
}


