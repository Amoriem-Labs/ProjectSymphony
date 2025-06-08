using System.Collections;
using System.Collections.Generic;
using System.Linq;
using RhythMidi;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

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

    // Variables for leaving results screen 

    public bool DemoComplete = false; // will be set true in W3D7

    public bool freePlay = false;

    public Image[] panels;
    public GameObject panel;


    // WARNING METHOD

    public GameObject warningObject;

    public GameObject warningBackground;

    public TMP_Text warningText;

    public Button confirmWarning;

    public Button cancelWarning;

    private string pendingSceneName;


    public Button closeButton;

    private Vector3 hiddenPosition;
    private Vector3 centerPosition;

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

        centerPosition = new UnityEngine.Vector3(0, 0, 0);
        hiddenPosition = centerPosition + new UnityEngine.Vector3(0, -Screen.height, 0);
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
        transitionTime = 0.5f;
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

        if (DemoComplete && SceneManager.GetActiveScene().name == "TitleScene")
        {
            SetPanelActive(); // or whichever panel index you want to activate
        }
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

        audio.volume = 0f;
        audio.Play(); // make sure it starts playing
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
                buttonAudio.volume = 0.3f; // or fade it in if you want:
                                           // yield return StartCoroutine(FadeInAudio(buttonAudio, 0.5f, 0.5f));
            }
        }
    }
    public void SetPanelActive()
    {
        panel.SetActive(true);
        closeButton.onClick.AddListener(ClosePanel);
        DemoComplete = false;
    }
    public void ClosePanel()
    {
        panel.SetActive(false);
        //hello
    }
    public void displayWarning(string sceneName, string warningMessage)
    {
        pendingSceneName = sceneName;

        warningText.text = warningMessage;
        warningBackground.SetActive(true);
        warningObject.SetActive(true);

        // Reset position offscreen
        RectTransform rect = warningObject.GetComponent<RectTransform>();
        rect.anchoredPosition = hiddenPosition;

        // Animate to center
        LeanTween.move(rect, centerPosition, 0.4f).setEaseOutBack();

        // Setup button listeners
        confirmWarning.onClick.RemoveAllListeners();
        cancelWarning.onClick.RemoveAllListeners();

        confirmWarning.onClick.AddListener(OnConfirm);
        cancelWarning.onClick.AddListener(OnCancel);
    }

    private void OnConfirm()
    {
        RectTransform rect = warningObject.GetComponent<RectTransform>();
        LeanTween.move(rect, hiddenPosition, 0.3f).setEaseInBack().setOnComplete(() =>
        {
            warningObject.SetActive(false);
            warningBackground.SetActive(false);
        });
        GameStateManager.Instance.LoadNewScene(pendingSceneName);
    }

    private void OnCancel()
    {
        RectTransform rect = warningObject.GetComponent<RectTransform>();
        LeanTween.move(rect, hiddenPosition, 0.3f).setEaseInBack().setOnComplete(() =>
        {
            warningObject.SetActive(false);
            warningBackground.SetActive(false);
        });
    }


}


