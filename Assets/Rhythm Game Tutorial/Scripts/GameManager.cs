using System;
using System.Collections;
using System.Collections.Generic;
using Melanchall.DryWetMidi.Interaction;
using RhythMidi;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

[Serializable]
/// <summary>
/// Class to keep track of scoring info
/// </summary>
/// 

public class Scoreboard
{

    int scorePerGoodNote;
    int scorePerPerfectNote;
    Text scoreText;
    Text multiplierText;
    public int currentScore;
    public int numGoodHits;
    public int numPerfectHits;
    public int numMissedHits;

    public int currentMultiplier = 1;
    public int[] multiplierThresholds;
    public int multiplierTracker;

    public int largestCombo;

    public int currentCombo;
    ComboBar comboBar;

    // [0] = Stellar ; [1] = Good ; [2] = Missed
    public Dictionary<CharacterRole, List<int>> notesPerCharacter = new Dictionary<CharacterRole, List<int>>(); 
    public Scoreboard(int[] multiplierThresholds, int scorePerGoodNote, int scorePerPerfectNote, Text scoreText, Text multiplierText, ComboBar comboBar)
    {
        this.multiplierThresholds = multiplierThresholds;
        this.scorePerGoodNote = scorePerGoodNote;
        this.scorePerPerfectNote = scorePerPerfectNote;
        this.scoreText = scoreText;
        this.multiplierText = multiplierText;
        this.comboBar = comboBar;

        largestCombo = 0;
        currentCombo = 0;

        notesPerCharacter = new Dictionary<CharacterRole, List<int>>();
        foreach (CharacterRole character in Enum.GetValues(typeof(CharacterRole))){
            if (character != CharacterRole.None) 
            {
                notesPerCharacter[character] = new List<int> { 0, 0, 0 }; 
            }
        }

    }

    public void RegisterPerfectHit(CharacterRole currCharacter)
    {
        numPerfectHits++;
        notesPerCharacter[currCharacter][0] += 1;
        currentScore += scorePerPerfectNote * currentMultiplier;
        UpdateMultiplier();
        UpdateCombo(1);
        UpdateScoreUI();
    }

    public void RegisterGoodHit(CharacterRole currCharacter)
    {
        numGoodHits++;
        notesPerCharacter[currCharacter][1] += 1;
        currentScore += scorePerGoodNote * currentMultiplier;
        UpdateMultiplier();
        UpdateCombo(1);
        UpdateScoreUI();
    }

    public void RegisterNoteMissed(CharacterRole currCharacter)
    {
        numMissedHits++;
        notesPerCharacter[currCharacter][2] += 1;
        currentMultiplier = 1;
        multiplierTracker = 0;
        UpdateCombo(-1);
        UpdateScoreUI();
    }

    //can delete later maybe.
    void UpdateCombo(int num)
    {
        if (num > 0)
        {
            currentCombo += num;
            if (currentCombo > largestCombo)
            {
                largestCombo = currentCombo;
            }
        }
        else
        {
            currentCombo = 0;
        }
    }
    public void UpdateScoreUI()
    {
        //scoreText.text = "Score: " + currentScore; 
        // made change here
        scoreText.text = "" + currentScore; 
        multiplierText.text = "Multiplier: x" + currentMultiplier;
        comboBar.SetScore(currentScore); // will change here

        
    }

    void UpdateMultiplier()
    {
        if(currentMultiplier - 1 < multiplierThresholds.Length)
        {
            multiplierTracker++;

            if(multiplierThresholds[currentMultiplier - 1] <= multiplierTracker)
            {
                multiplierTracker = 0;
                currentMultiplier++;
            }
        }
    }
}

public class GameManager : MonoBehaviour
{


    // currentCharacterInAdvance takes currentCharacter's value after fallingNotesTime seconds
    // In other words, new arrows will spawn for currentChracterInAdvance's beatmap, but
    // the player will be judged based on currentCharacter's beatmap
    public CharacterSelection currentCharacter = CharacterSelection.Melodist;
    public CharacterSelection currentCharacterInAdvance = CharacterSelection.Melodist;
    public CharacterSelection characterToSwitchTo = CharacterSelection.None;
    private Dictionary<CharacterSelection, float> timeSpentOnCharacter = new Dictionary<CharacterSelection, float>();
    public bool startPlaying;
    public string songToPlay;

    public int scorePerGoodNote = 125;
    public int scorePerPerfectNote = 150;
    public int[] multiplierThresholds = new int[]{ 2, 8, 16 };

    public static GameManager instance;

    public Text scoreText;
    public Text multiText;

    public ComboBar comboBar;
    public Scoreboard scoreboard;

    public RhythMidiController rhythMidi;

    public float fallingNotesTime = 1.0f; // how soon the notes appear (at 1.0, they will spawn one second before)

    public GameObject leftNotePrefab;
    public GameObject upNotePrefab;
    public GameObject downNotePrefab;
    public GameObject rightNotePrefab;
    public GameObject characterSwitchIndicatorPrefab;
    
    public RectTransform gameScreen;
    public RectTransform[] beatColumns;
    public KeyCode[] keyBindings;

    public HitWindow perfectHitWindow;
    public HitWindow goodHitWindow;

    public GameObject missEffect;
    public GameObject perfectEffect;
    public GameObject goodEffect;

    public GameObject flash;

    public GameObject characterSwitchWarningEffect;
    public CharacterDisplayUI characterDisplayUI;
    public AudioSource sfxSource;
    public AudioClip[] hitAudioClips;
    public ResultsScreenManager resultsScreenManager;

    // New UI below
    public Transform start;
    

    void Start()
    {
       
        if(instance != null) {
            Destroy(gameObject);
            return;
        }
        instance = this;

        scoreboard = new Scoreboard(multiplierThresholds, scorePerGoodNote, scorePerPerfectNote, scoreText, multiText, comboBar);

        goodHitWindow.OnNoteMissed += OnNoteMissed; //+= is same as .AddListener
        rhythMidi.onFinishedLoading.AddListener(StartGame);
        rhythMidi.CreateNoteNotifier(fallingNotesTime).OnNote += SpawnArrowSprite;

        //tracking character time spent
        foreach (CharacterRole character in Enum.GetValues(typeof(CharacterRole)))
        {
            if(character != CharacterRole.None)
            {
                timeSpentOnCharacter[character] = 0f;
            }
        }

        // Notifies on beat 1 of every measure, but fallingNotesTime seconds in advance
        rhythMidi.CreateNoteNotifier(fallingNotesTime, (note) => note.NoteNumber == 25).OnNote += OnMeasureAdvance;
        // Notifies on beat 1 of every measure
        rhythMidi.CreateNoteNotifier(0f, (note) => note.NoteNumber == 25).OnNote += OnMeasure;

        startPlaying = true;
    }

    void Update()
    {

        if(startPlaying && rhythMidi.IsPlaying)  
        {
            if (!rhythMidi.audioSources[0].isPlaying)
            {
                rhythMidi.StopChart();
                
                ShowResultsScreen();
            }
            timeSpentOnCharacter[currentCharacter] += Time.deltaTime;
        }
        // DEBUGGING 
        if (Input.GetKeyDown(KeyCode.R))
        {
            Debug.Log("Showing Results Screen (R key pressed)");
            ShowResultsScreen();
        }

        RectTransform columnTarget = null;
        bool perfect = false;
        bool good = false;

        for(int i = 0; i < keyBindings.Length; i++)
        {
            if(Input.GetKeyDown(keyBindings[i]))
            {
                if(perfectHitWindow.CheckHit((int)currentCharacter + i)) perfect = true;
                if(goodHitWindow.CheckHit((int)currentCharacter + i)) good = true;
                columnTarget = beatColumns[i];
            }
        }
        
        if(perfect)
        {
            Instantiate(perfectEffect, columnTarget);
            scoreboard.RegisterPerfectHit(currentCharacter);

            Vector3 newPosition = columnTarget.position;
            newPosition.y -= 2.8f; // Adjust this value to move it lower (negative value moves it down)
            GameObject flashObject = Instantiate(flash, newPosition, Quaternion.identity);

            // Scale the flash object smaller
            flashObject.transform.localScale = new Vector3(0.6f, 0.6f, 0.6f);
            PlayHitSoundEffect();
        }
        else if(good)
        {
            Instantiate(goodEffect, columnTarget);
            scoreboard.RegisterGoodHit(currentCharacter);
            
            Vector3 newPosition = columnTarget.position;
            newPosition.y -= 2.8f; // Adjust this value to move it lower (negative value moves it down)
            GameObject flashObject = Instantiate(flash, newPosition, Quaternion.identity);

            // Scale the flash object smaller
            flashObject.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
            PlayHitSoundEffect();
        }

        if(Input.GetKeyDown(KeyCode.Alpha1)) SwitchToCharacter(CharacterSelection.Melodist);
        else if(Input.GetKeyDown(KeyCode.Alpha2)) SwitchToCharacter(CharacterSelection.Drummer);
        else if(Input.GetKeyDown(KeyCode.Alpha3)) SwitchToCharacter(CharacterSelection.Bassist);
        else if(Input.GetKeyDown(KeyCode.Alpha4)) SwitchToCharacter(CharacterSelection.Guitarist);
    }

    private void PlayHitSoundEffect() {
        sfxSource.PlayOneShot(hitAudioClips[UnityEngine.Random.Range(0, hitAudioClips.Length)]);
    }

    private void SwitchToCharacter(CharacterSelection character)
    {
        if(character == currentCharacter) return;
        characterToSwitchTo = character;
        characterSwitchWarningEffect.SetActive(true);
    }

    private void OnMeasureAdvance(Note note)
    {
        if(characterToSwitchTo == CharacterSelection.None) return;
        currentCharacterInAdvance = characterToSwitchTo;
        characterToSwitchTo = CharacterSelection.None;

        GameObject sprite = Instantiate(characterSwitchIndicatorPrefab, gameScreen);
        BeatScroller behavior = sprite.GetComponent<BeatScroller>();
        behavior.totalTime = fallingNotesTime;
    }
    private void OnMeasure(Note note)
    {
        if(currentCharacterInAdvance == currentCharacter) return;
        currentCharacter = currentCharacterInAdvance;
        characterSwitchWarningEffect.SetActive(false);
        characterDisplayUI.SwitchToCharacter(currentCharacter);
    }

    private void SpawnArrowSprite(Note note) 
    {
        if(note.NoteNumber < ((int)currentCharacterInAdvance) || note.NoteNumber > (int)currentCharacterInAdvance + 3) return; // ignore other notes, like pulse and system events
        int i = note.NoteNumber - (int)currentCharacterInAdvance;
        RectTransform column = beatColumns[i];

        // Debug.Log(note.NoteNumber);
        // Debug.Log(i);

        GameObject sprite = null; // Declare sprite outside the switch block.

        switch (i)
        {
            case 0:
                sprite = Instantiate(leftNotePrefab, column);
                break;
            case 1:
                sprite = Instantiate(upNotePrefab, column);
                break;
            case 2:
                sprite = Instantiate(downNotePrefab, column);
                break;
            case 3:
                sprite = Instantiate(rightNotePrefab, column);
                break;
            default:
                Debug.LogError("Invalid case value for i: " + i);
                break;
        }

        // Check if sprite was successfully instantiated to avoid null reference errors.
        if (sprite != null)
        {
            BeatScroller behavior = sprite.GetComponent<BeatScroller>();
            behavior.totalTime = fallingNotesTime;
        }
        else
        {
            Debug.LogError("Sprite was not instantiated. Ensure i has a valid value");
        }
    }

    public void StartGame()
    {
        rhythMidi.PrepareChart(songToPlay);
        rhythMidi.PlayChart();

        Debug.Log("Start Game");
        start.localPosition = new Vector2(0, -Screen.height); // start below the screen
        start.LeanMoveLocalY(0, 0.5f)
        .setEaseOutExpo()
        .setDelay(0.2f)
        .setOnComplete(() =>
        {
            // Pause briefly, then move back down
            LeanTween.delayedCall(1.0f, () =>
            {
                start.LeanMoveLocalY(Screen.height, 0.5f).setEaseInExpo();
            });
        });
    }

    public void OnNoteMissed(Note note)
    {
        if(note.NoteNumber < (int)currentCharacter || note.NoteNumber > (int)currentCharacter + 3) return; // ignore other notes, like pulse and system events
        int i = note.NoteNumber - (int)currentCharacter;
        Transform column = beatColumns[i];
        Instantiate(missEffect, column);

        scoreboard.RegisterNoteMissed(currentCharacter);
    }




    public void ShowResultsScreen(){
        resultsScreenManager.ShowResultsScreen(
        scoreboard.currentScore,
        scoreboard.numPerfectHits,
        scoreboard.numGoodHits,
        scoreboard.numMissedHits,
        scoreboard.largestCombo,
        scoreboard.notesPerCharacter,
        timeSpentOnCharacter
    );
    }
}


