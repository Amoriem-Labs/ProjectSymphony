using System.Collections;
using System.Collections.Generic;
using System.Text;
using RhythMidi;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum CharacterRole {
    None = 0,
    Melodist = 1,
    Counter = 2,
    Percussion = 3,
    Harmony = 4
};

[System.Serializable]
public class Character {
    public CharacterRole role;
    public string name;
    public string instrument;

    public string bio; // PLACEHOLDER: can remove if needed
    public Sprite spriteUnlocked;
    public Sprite spriteLocked;
    public Sprite backgroundHero;
    public int midiPitchStart;
}

public class CharacterData {
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

    // Animators

    public Animator transition;
    public float transitionTime = 1f;

    void Start()
    {
        if(Instance == null)
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
        foreach(Character c in characters) {
            CharacterData cd = new CharacterData(c, 100f, true);
            characterData.Add(c, cd);
        }
    }
    public void LoadCharacterSelect(string song)
    {
        currentSongName = song;

        Debug.Log("loading party screen");
        StartCoroutine(LoadScene("PartyScreen"));

        //SceneManager.LoadScene("PartyScreen");
    }

    public void StartRhythmGame(CharacterData[] selectedCharacters)
    {
        this.selectedCharacters = selectedCharacters;
        RhythMidiController.Instance.ClearCallbacks();
        SceneManager.LoadScene("Start copy");
    }

    public void LoadNewScene(string sceneName)
    {
        
         StartCoroutine(LoadScene(sceneName));

    }
    IEnumerator LoadScene(string sceneName)
    {
        Debug.Log("ienumerator started");
        transition.SetTrigger("Start");
        yield return new WaitForSeconds(transitionTime);
        SceneManager.LoadScene(sceneName);
        transition.SetTrigger("EnterNewScene");
    }
}
