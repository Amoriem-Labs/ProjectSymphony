using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;
using System;
using Unity.VisualScripting;



// Logic here

// check what characters are unlocked, if they are required for this song, and if they are available for this song 
// call displayItems accordingly, displaying new data

// on character slot click -> set partyManager array 
// the inventory of characters needs to have:
// character name, character role, character image to replace with, character affection 
// character instrument 
// update the namecard at the button and update selectedUI
// edit the chemistry bar based on affection
// make sure there is one character per role
// once array is full, set go 

public class PartyManager : MonoBehaviour
{

    [System.Serializable]
    public class character{ // for debugging only! will need to reference the persistent data
        public string name;
        public string instrument;
        public int role;
        public float affection; 
        public bool isUnlocked;
        public bool isRequired; 

        public bool isAvailable; 
    }
    [SerializeField]
    public character[] characterList; 
    private Dictionary<string, character> characterDictionary;

    public GameObject[] characterSlots;

    public GameObject[] nameTags;

    public TextMeshPro songText;

    public string[] selectedCharacters = new string[4]; // placeholder output 



    void Start()
    {
        characterDictionary = new Dictionary<string, character>();

        // Populate the dictionary with characterList data
        PopulateCharacterDictionary();
        DisplayCharacters();

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
   }

}
