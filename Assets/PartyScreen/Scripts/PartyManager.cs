using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;
using RhythMidi;

public class PartyManager : MonoBehaviour
{
    public Transform[] roleColumns;
    public TextMeshProUGUI[] characterNameLabels;
    public TextMeshProUGUI[] characterInstrumentLabels;
    public Image[] backgroundHeros;
    public Image[] backgroundLabels;
    public GameObject GoButton;
    public GameObject NotEnoughAffection;
    public GameObject ChemistryBar;
    public GameObject characterSlotPrefab;
    
    ComboBar chemistryBar;
    List<CharacterSlot> characterSlots = new List<CharacterSlot>();

    public AudioSource audioSource;
    public AudioClip select;

    // Important Values // 
    public float threshold; // can be edited

    [SerializeField]
    private float chemistry; 


    // Input // 
    public int numberInBand; // number of musicians required for this song
    public GameObject SongText;

    void Start()
    {
        print(roleColumns[0]);
        InstantiateCharacterSlots(CharacterRole.Melodist, roleColumns[0]);
        InstantiateCharacterSlots(CharacterRole.Counter, roleColumns[1]);
        InstantiateCharacterSlots(CharacterRole.Percussion, roleColumns[2]);
        InstantiateCharacterSlots(CharacterRole.Harmony, roleColumns[3]);
        SongText.GetComponent<TextMeshProUGUI>().text = GameStateManager.Instance.currentSongName;
        chemistryBar = ChemistryBar.GetComponent<ComboBar>();
    }

    public void InstantiateCharacterSlots(CharacterRole role, Transform column)
    {
        CharacterData[] characterData = GameStateManager.Instance.characterData
            .Values
            .Where(c => c.character.role == role)
            .ToArray();

        float y = 0;
        foreach (CharacterData data in characterData)
        {
            GameObject slot = Instantiate(characterSlotPrefab, column);
            CharacterSlot script = slot.GetComponent<CharacterSlot>();
            script.characterData = data;
            script.isRequired = true;
            script.isAvailable = GameStateManager.Instance.CurrentChart.Tracks.ContainsKey(data.character.name);
            script.partyManager = this;
            characterSlots.Add(script);
            slot.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, y);
            y -= 220f;
        }
    }

    public void DeselectAllSlots(CharacterRole role)
    {
        foreach (CharacterSlot c in characterSlots)
        {
            if (c.characterData.character.role == role)
            {
                c.Deselect();
            }
        }
    }

    public void SelectCharacter(CharacterData characterData)
    {
        audioSource.PlayOneShot(select);
        int idx = (int)characterData.character.role - 1;
        characterNameLabels[idx].text = characterData.character.name;
        characterInstrumentLabels[idx].text = characterData.character.instrument;

        RectTransform backgroundLabelRT = backgroundLabels[idx].GetComponent<RectTransform>();
        LeanTween.scale(backgroundLabelRT, Vector3.zero, 0.5f)
            .setEase(LeanTweenType.easeInElastic)
            .setOnComplete(() => backgroundLabelRT.gameObject.SetActive(false));

        
        RectTransform backgroundHeroRT = backgroundHeros[idx].GetComponent<RectTransform>();
        LeanTween.scale(backgroundHeroRT, Vector3.zero, 0.5f)
            .setEase(LeanTweenType.easeInElastic)
            .setOnComplete(() =>
            {
                backgroundHeros[idx].sprite = characterData.character.backgroundHero;
                backgroundHeroRT.gameObject.SetActive(true);

                LeanTween.scale(backgroundHeroRT, new Vector3(1f, 1f, 1f), 0.5f)
                    .setEase(LeanTweenType.easeOutElastic);
            });
        
        CheckIfReady();
    }

    public void CheckIfReady()
    {
        int selectedCount = characterSlots.Count(c => c.thisSelected);

        if (selectedCount >= numberInBand)
        {
            if (CalculateChemistry())
            {
                LeanTween.scale(GoButton.GetComponent<RectTransform>(), Vector3.zero, 0.2f)
                        .setEase(LeanTweenType.easeInCubic)
                        .setOnComplete(() =>
                        {
                            GoButton.SetActive(true);
                            LeanTween.scale(GoButton.GetComponent<RectTransform>(), new Vector3(1f, 1f, 1f), 0.2f)
                                .setEase(LeanTweenType.easeOutElastic);
                        });
                LeanTween.scale(NotEnoughAffection.GetComponent<RectTransform>(), Vector3.zero, 0.2f)
                    .setEase(LeanTweenType.easeInCubic)
                    .setOnComplete(() =>
                NotEnoughAffection.SetActive(false));
            }
            else
            {
                LeanTween.scale(NotEnoughAffection.GetComponent<RectTransform>(), Vector3.zero, 0.2f)
                        .setEase(LeanTweenType.easeInCubic)
                        .setOnComplete(() =>
                        {
                            NotEnoughAffection.SetActive(true);
                            LeanTween.scale(NotEnoughAffection.GetComponent<RectTransform>(), new Vector3(1f, 1f, 1f), 0.2f)
                                .setEase(LeanTweenType.easeOutElastic);
                        });
                LeanTween.scale(GoButton.GetComponent<RectTransform>(), Vector3.zero, 0.2f)
                        .setEase(LeanTweenType.easeInCubic)
                        .setOnComplete(() =>
                GoButton.SetActive(false));
            }
        }
    }
    public bool CalculateChemistry()
    {
        chemistry = characterSlots
            .Where(c => c.thisSelected)
            .Aggregate(0f, (acc, c) => acc + c.characterData.affection)
            / numberInBand;
        
        chemistryBar.SetScore((int)chemistry);
        return chemistry >= threshold;
    }

    public void ProceedToRhythm()
    {
        GameStateManager.Instance.StartRhythmGame(
            characterSlots
                .Where(c => c.thisSelected)
                .Select(c => c.characterData)
                .ToArray()
        );
    }


}
