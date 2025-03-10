using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;
using System;
using Unity.VisualScripting;

/* NOTES

- Requires Input and Output 
- The character class is for testing purposes only - ideally, this script would read from the save file/persistent data

*/

public class PartyManager : MonoBehaviour
{

    [System.Serializable]

    // Testing Purposes - need to replaced by persistent data //
    public class character{ // for debugging only! will need to reference the persistent data
        public string name;
        public string instrument;
        public int role; // 1 = Melody, 2 = CounterMelody, 3 = Percussion, 4 = Harmony
        public float affection; 
        public bool isUnlocked;
        public bool isRequired; 

        public bool isAvailable; 
    }
    [SerializeField]
    public character[] characterList; 

    public string songName; // will need to be inputted later by song select/ VN

    // The Dictionary that will be read into // 
    private Dictionary<string, character> characterDictionary;

    // UI Objects // 

    public GameObject[] characterSlots;

    public GameObject[] nameTags;

    public GameObject[] backgrounds;

    public GameObject GoButton;

    public GameObject NotEnoughAffection;

    public GameObject ChemistryBar;

    ComboBar chemistryBar;

    // Important Values // 
    public float threshold; // can be edited

    [SerializeField]
    private float chemistry; 


    // Input // 

    public int numberInBand; // number of musicians required for this song

    public GameObject SongText; 
    

    // Output// 

    public string[] selectedCharacters = new string[4]; // placeholder output 

    void Start()
    {
        characterDictionary = new Dictionary<string, character>();

        // Populate the dictionary with characterList data
        PopulateCharacterDictionary();
        DisplayCharacters();

        SongText.GetComponent<TextMeshProUGUI>().text = songName;

        chemistryBar = ChemistryBar.GetComponent<ComboBar>();

    }

        void PopulateCharacterDictionary()
        {
        foreach (var character in characterList)
        {
            if (!characterDictionary.ContainsKey(character.name)) 
            {
                characterDictionary.Add(character.name, character);
                Debug.Log("Added character: " + character.name);
            }
            else
            {
                Debug.LogWarning("Character with name " + character.name + " already exists in the dictionary.");
            }
        }
        }

   public void DisplayCharacters()
   {
    Debug.Log("Display Characters is called");

    
    foreach (GameObject slot in characterSlots)
    {
         Debug.Log("entered for loop");

         CharacterSlot slotInfo = slot.GetComponent<CharacterSlot>();

         character characterInfo = characterDictionary[slotInfo.characterName];

         if (characterInfo.isUnlocked)
         {
            Debug.Log("adding entry to CharacterSlot, as it is unlocked");
            slotInfo.UnlockCharacter(characterInfo.instrument, characterInfo.role, characterInfo.affection, characterInfo.isUnlocked, characterInfo.isRequired, characterInfo.isAvailable);
        
        }
    }

   }

   public void DeselectAllSlots(int role)
    {
    foreach (GameObject slot in characterSlots)
    {
        // Get the CharacterSlot component
        CharacterSlot slotInfo = slot.GetComponent<CharacterSlot>();
        Image characterImage = slot.GetComponent<Image>();   

        // Check if the role matches
        if (role == slotInfo.role)
        {
            Debug.Log("deselected + " + slotInfo.characterName);
            // Disable selection indicator
            slotInfo.thisSelected = false;

            // If the GameObject has a Renderer, adjust the outline material
            
            //Material mat = Instantiate(characterImage.material);
            characterImage.material.SetFloat("_OutlineAlpha", 0f);
            //mat.
        }
    }
    }

   public void SelectCharacter(int role, string name, string instrument)
   {
        selectedCharacters[role - 1] = name;
        Debug.Log("Role" + role + " is given to" + name);

        Transform imageTransform = nameTags[role - 1].transform.Find("Image");

        if (imageTransform.Find("NameText") != null)
        {
            Debug.Log("found name text");
        }

        if (imageTransform.Find("NameText").GetComponent<TextMeshPro>() != null)
        {
            Debug.Log("text mesh pro is found");
        }

        // Set the text of 'NameText' child
        imageTransform.Find("NameText").GetComponent<TextMeshProUGUI>().text = name;

        // Set the text of 'InstrumentText' child
        imageTransform.Find("InstrumentText").GetComponent<TextMeshProUGUI>().text = instrument;

        // change character image below

         Sprite newSprite = Resources.Load<Sprite>($"RhythmSprites/{name}");
         if (newSprite == null)
        {
            Debug.Log("did not find sprite of" + name);
            newSprite = Resources.Load<Sprite>("RhythmSprites/EMPTY");
        }
        else
        {
            Debug.Log("found sprite of" + name);
        }
      
        Transform labelImage = backgrounds[role - 1].transform.Find("LabelImage");
        Transform characterImage = backgrounds[role - 1].transform.Find("CharacterImage");

        if (labelImage != null)
        {
            // Animate labelImage disappearing (scaling down before deactivating)
            LeanTween.scale(labelImage.GetComponent<RectTransform>(), Vector3.zero, 0.5f)
                .setEase(LeanTweenType.easeInElastic)
                .setOnComplete(() => labelImage.gameObject.SetActive(false)); // Deactivate after animation
        }

        if (characterImage != null)
        {
            Debug.Log("Updating sprite of " + name);

            Image imgComponent = characterImage.GetComponent<Image>();
            if (imgComponent != null)
            {
                LeanTween.scale(characterImage.GetComponent<RectTransform>(), Vector3.zero, 0.5f)
                    .setEase(LeanTweenType.easeInElastic)
                    .setOnComplete(() =>
                    {
                        
                        // Change sprite after scaling down
                        imgComponent.sprite = newSprite;
                        characterImage.gameObject.SetActive(true);

                        // Animate characterImage appearing
                        LeanTween.scale(characterImage.GetComponent<RectTransform>(), new Vector3(1f, 1f, 1f), 0.5f)
                            .setEase(LeanTweenType.easeOutElastic);
                    });

                 // Ensure it's visible before animation starts
            }
            else
            {
                Debug.LogError("characterImage does not have an Image component!");
            }
        }


        checkArray();
   }
   public void checkArray()
    {
    int nonEmptyCount = 0;  // To count the number of non-empty entries

    // Loop through the selected characters
    foreach (string character in selectedCharacters)
    {
        if (!string.IsNullOrEmpty(character))  // Check if the character is not null or empty
        {
            nonEmptyCount++;
        }

        // If we've already counted enough non-empty entries, we can break early
        if (nonEmptyCount >= numberInBand)
        {
            break;
        }
    }

    if (nonEmptyCount >= numberInBand)
    {
        // Enough entries are non-empty
        Debug.Log("Enough characters are selected.");
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
    else
    {
        // Not enough entries are non-empty
        Debug.Log("Not enough characters are selected.");
    }
    }
    public bool CalculateChemistry()
    {
        chemistry = 0; 

        foreach (string bandmate in selectedCharacters)
        {
            chemistry += characterDictionary[bandmate].affection; 
            Debug.Log("chemistry is now" + chemistry);
        }
        chemistry = chemistry / numberInBand; 

        chemistryBar.SetScore((int)chemistry);

        if (chemistry >= threshold)
        {
            Debug.Log("chemistry is above threshold");
            return true;
        }
        else
        {
            Debug.Log("chemistry is below threshold");
            return false; 
        }
    }
    public void ProceedToRhythm()
    {
        Debug.Log("Will head to the rhythm screen with these characters:");
        foreach (string bandmate in selectedCharacters)
        {
            Debug.Log(bandmate);
        }
    }


}
