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

    public Animator transition;
    public float transitionTime = 1f;

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
        StartCoroutine(LoadScene("PartyScreen"));
    }

    public void LoadVN()
    {
        Time.timeScale = 1;
        StartCoroutine(LoadScene("VN"));
    }

    public void StartRhythmGame(CharacterData[] selectedCharacters)
    {
        this.selectedCharacters = selectedCharacters;
        RhythMidiController.Instance.ClearCallbacks();
        StartCoroutine(LoadScene("RhythmGame"));
    }

    public bool SelectedCharactersContainsRole(CharacterRole role) =>
        selectedCharacters.Any(c => c.character.role == role);

    public CharacterData GetSelectedCharacterWithRole(CharacterRole role) =>
        selectedCharacters.First(c => c.character.role == role);

    public void LoadNewScene(string sceneName)
    {
        StartCoroutine(LoadScene(sceneName));
    }

    IEnumerator LoadScene(string sceneName)
    {
        if (transition != null)
        {
            transition.SetTrigger("Start");
            yield return new WaitForSeconds(transitionTime);
        }

        SceneManager.LoadScene(sceneName);

        if (transition != null)
        {
            yield return new WaitForSeconds(transitionTime);
            transition.SetTrigger("EnterNewScene");
        }
    }
}
