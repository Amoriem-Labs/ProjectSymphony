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
    public float transitionTime;

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

    public CharacterRole[] ActiveRoles =>
        selectedCharacters.Select(c => c.character.role).ToArray();

    public CharacterRole[] AllowedRolesInChart =>
        CurrentChart.Tracks.Keys
            .Select(characterName => characters.FirstOrDefault(c => c.name == characterName))
            .Select(character => character.role)
            .Distinct()
            .ToArray();

    public void LoadNewScene(string sceneName)
    {
        StartCoroutine(LoadScene(sceneName));
    }

    IEnumerator LoadScene(string sceneName)
    {
        AudioSource audio = FindObjectOfType<AudioSource>();

        if (audio != null)
            yield return StartCoroutine(FadeOutAudio(audio, 1f)); // 1 second fade-out

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

        AudioSource newAudio = FindObjectOfType<AudioSource>();

        if (newAudio != null)
            yield return StartCoroutine(FadeInAudio(newAudio, 1f)); // 1 second fade-in
    }

    IEnumerator FadeOutAudio(AudioSource audio, float duration)
    {
        float startVolume = audio.volume;

        for (float t = 0; t < duration; t += Time.deltaTime)
        {
            audio.volume = Mathf.Lerp(startVolume, 0f, t / duration);
            yield return null;
        }

        audio.volume = 0f;
        audio.Pause(); // optional: stop playback
    }

    IEnumerator FadeInAudio(AudioSource audio, float duration)
    {
        audio.Play(); // make sure it starts playing
        audio.volume = 0f;
        float targetVolume = 1f;

        for (float t = 0; t < duration; t += Time.deltaTime)
        {
            audio.volume = Mathf.Lerp(0f, targetVolume, t / duration);
            yield return null;
        }

        audio.volume = targetVolume;

        GameObject buttonManager = GameObject.Find("ButtonManager");
        if (buttonManager != null)
        {
            AudioSource buttonAudio = buttonManager.GetComponent<AudioSource>();
            if (buttonAudio != null)
            {
                buttonAudio.volume = 0.5f; // or fade it in if you want:
                                           // yield return StartCoroutine(FadeInAudio(buttonAudio, 0.5f, 0.5f));
            }
        }
    }


}


